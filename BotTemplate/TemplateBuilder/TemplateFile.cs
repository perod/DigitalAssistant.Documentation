using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace TemplateBuilder
{
    public class TemplateFile
    {
        private static Regex _projectGuidRegex = new Regex("<ProjectGuid>(.+)</ProjectGuid>");
        private static Regex _rootNamespaceRegex = new Regex("<RootNamespace>(.+)</RootNamespace>");
        private static Regex _assemblyNamepaceRegex = new Regex("<AssemblyName>(.+)</AssemblyName>");
        private static Regex _targetFrameworkVersionRegex = new Regex("<TargetFrameworkVersion>(.+)</TargetFrameworkVersion>");
        private static Regex _hintPathRegex = new Regex("<HintPath>(\\.\\.\\\\)+(packages.*)");
        private static Regex _packagesRegex = new Regex("[>'\"](\\.\\.\\\\)+(packages.*)");
        private static Regex _isImageRegex = new Regex("\\.(png|jpe?g|tiff|gif)");

        public string SourceLocation { get; private set; }
        public string DestinationLocation { get; private set; }
        public bool IsProjectTemplate { get; private set; }
        public string RelativePath { get; private set; }
        public string ItemGroupEntry { get; private set; }
        public string Content { get; private set; }
        public string VsTemplateEntry { get; private set; }
        public bool Created { get; private set; }
        public XmlNodeList ProjectTemplateItemGroups { get; private set; }
        public bool IsImage { get; private set; }

        internal async Task<TemplateFile> CreateFile()
        {
            if (IsProjectTemplate)
            {
                await UpdateProjectTemplateFile().ConfigureAwait(false);
            }
            else
            {
                await StoreTemplateFile().ConfigureAwait(false);
            }

            return this;
        }

        private async Task UpdateProjectTemplateFile()
        {
            var fileInfo = new FileInfo(DestinationLocation);

            try
            {
                if (fileInfo.Exists)
                {
                    var xmlDoc = await ReadXmlDocument(fileInfo).ConfigureAwait(false);
                    var itemGroupElements = xmlDoc.GetElementsByTagName("ItemGroup");
                    for (var i = itemGroupElements.Count - 1; i >= 0; i--)
                    {
                        var currentNode = itemGroupElements[i];
                        currentNode.ParentNode.RemoveChild(currentNode);
                    }

                    foreach (XmlNode itemGroupNode in ProjectTemplateItemGroups)
                    {
                        var importedNode = xmlDoc.ImportNode(itemGroupNode, true);
                        xmlDoc.DocumentElement.AppendChild(importedNode);
                    }
                    xmlDoc.Save(DestinationLocation);
                }
                else
                {
                    Console.WriteLine($"Could not update template file {DestinationLocation}. File does not exist...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error ocurred while updating project template {DestinationLocation}. Error message: {ex.Message}");
            }
        }

        private async Task StoreTemplateFile()
        {
            try
            {
                var fileInfo = new FileInfo(DestinationLocation);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                else
                {
                    var directory = fileInfo.Directory;
                    if (!directory.Exists)
                    {
                        directory.Create();
                    }
                }

                if (IsImage)
                {
                    var sourceFile = new FileInfo(SourceLocation);
                    sourceFile.CopyTo(DestinationLocation);
                    Created = true;
                }
                else
                {
                    using (var fileStream = fileInfo.OpenWrite())
                    {
                        var bytes = Encoding.UTF8.GetBytes(Content);
                        await fileStream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                        Created = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to create file: {RelativePath}. Error: {ex.Message}");
                Created = false;
            }
        }

        public static async Task<TemplateFile> CreateTemplateFile(FileInfo file, DirectoryInfo sourceRootDirectory, DirectoryInfo destinationRootDirectory, string projectNamespace, string solutionRootNamespace, string projectGuid)
        {
            var templateFile = new TemplateFile();

            var isMarkdown = file.Extension == ".md";
            var isHtml = file.Extension == ".html";
            var isCs = file.Extension == ".cs";
            var isCsProj = file.Extension == ".csproj";
            templateFile.SourceLocation = file.FullName;
            templateFile.DestinationLocation = templateFile.SourceLocation.Replace(sourceRootDirectory.FullName, destinationRootDirectory.FullName + "\\" + projectNamespace);

            var targetFileName = templateFile.SourceLocation.Replace(sourceRootDirectory.FullName, "").TrimStart('\\');
            templateFile.RelativePath = projectNamespace + "\\" + targetFileName;

            templateFile.ItemGroupEntry = $"<None Include=\"{templateFile.RelativePath}\" />";
            templateFile.VsTemplateEntry = $"<ProjectItem ReplaceParameters=\"true\">{targetFileName}</ProjectItem>";

            templateFile.Content = string.Empty;

            templateFile.IsImage = _isImageRegex.Match(file.Extension).Success;

            if (!templateFile.IsImage)
            {
                using (var streamReader = file.OpenText())
                {
                    var line = await streamReader.ReadLineAsync().ConfigureAwait(false);
                    while (line != null)
                    {
                        if (isCsProj)
                        {
                            if (_projectGuidRegex.Match(line).Success)
                            {
                                line = "    <ProjectGuid>$guid1$</ProjectGuid>";
                            }
                            else if (_targetFrameworkVersionRegex.Match(line).Success)
                            {
                                line = "    <TargetFrameworkVersion>v$targetframeworkversion$</TargetFrameworkVersion>";
                            }
                            else if (_rootNamespaceRegex.Match(line).Success)
                            {
                                var match = _rootNamespaceRegex.Match(line);
                                var ns = match.Groups[1].Value;
                                line = $"    <RootNamespace>{ns.Replace(solutionRootNamespace, "$ext_safeprojectname$")}</RootNamespace>";
                            }
                            else if (_assemblyNamepaceRegex.Match(line).Success)
                            {
                                var match = _assemblyNamepaceRegex.Match(line);
                                var ns = match.Groups[1].Value;
                                line = $"    <AssemblyName>{ns.Replace(solutionRootNamespace, "$ext_safeprojectname$")}</AssemblyName>";
                            }
                            else if (_packagesRegex.Match(line).Success)
                            {
                                var match = _packagesRegex.Match(line);
                                var relativePath = match.Groups[1].Value;
                                var rest = match.Groups[2].Value;
                                line = $"{line.Substring(0, match.Groups[1].Index)}{relativePath}..\\{rest}";
                            }
                            else if (line.IndexOf(solutionRootNamespace) >= 0)
                            {
                                line = line.Replace(solutionRootNamespace, "$ext_safeprojectname$");
                            }
                        }

                        else if (isCs)
                        {
                            //These might need to be more robust
                            if (line.IndexOf("[assembly: Guid(") >= 0)
                            {
                                line = $"[assembly: Guid(\"$guid1$\")]";
                            }
                            else if (line.IndexOf("AssemblyCompany(\"\")") >= 0)
                            {
                                line = line.Replace("AssemblyCompany(\"\")", "AssemblyCompany(\"$ext_company$\")");
                            }
                            else if (line.IndexOf("AssemblyCopyright(\"Copyright ©  2018\")") >= 0)
                            {
                                line = line.Replace("AssemblyCopyright(\"Copyright ©  2018\")", "AssemblyCopyright(\"Copyright © $ext_company$ $ext_year$\")");
                            }
                        }
                        
                        if (line.IndexOf(solutionRootNamespace) >= 0)
                        {
                            line = line.Replace(solutionRootNamespace, $"$ext_safeprojectname$");
                        }
                        
                        templateFile.Content += line += "\r\n";
                        line = await streamReader.ReadLineAsync().ConfigureAwait(false);
                    }
                }
            }

            return templateFile;
        }

        internal static async Task<XmlDocument> ReadXmlDocument(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                try
                {
                    var doc = new XmlDocument();

                    byte[] result;

                    using (var fs = fileInfo.OpenRead())
                    {
                        result = new byte[fs.Length];
                        await fs.ReadAsync(result, 0, (int)fs.Length).ConfigureAwait(false);

                        using (var ms = new MemoryStream(result))
                        {
                            await ms.FlushAsync().ConfigureAwait(false);
                            ms.Position = 0;
                            doc.Load(ms);
                        }
                    }
                    return doc;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to load contents of file {fileInfo.FullName}. Error: {ex.Message}");
                    return null;
                }
            }

            Console.WriteLine($"Unable to load {fileInfo.FullName}. Error: File does not exist.");
            return null;
        }

        internal static void CleanDirectory(DirectoryInfo directory)
        {
            foreach (var file in directory.EnumerateFiles())
            {
                file.Delete();
            }

            foreach (var childDirectory in directory.EnumerateDirectories())
            {
                CleanDirectory(childDirectory);
                childDirectory.Delete();
            }
        }
    }
}