using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateBuilder
{
    public class Generator
    {
        public async Task Generate(Template template)
        {
            var sourceFiles = template.SourceProjects.Select(s => new FileInfo(s));
            var destinationFile = new FileInfo(template.DestinationProject);

            if (!sourceFiles.Any(f => !f.Exists) && destinationFile.Exists)
            {
                var noneContentFilesForDestinationProject = string.Empty;

                var projectNamespacesForRootVsTemplate = new List<string>();

                foreach (var sourceFile in sourceFiles)
                {
                    var projectNamespace = await GetRootNamespace(sourceFile).ConfigureAwait(false);
                    var projectGuid = Guid.NewGuid();

                    projectNamespacesForRootVsTemplate.Add(projectNamespace);

                    var destinationDirectory = destinationFile.Directory.FullName + $"\\{projectNamespace}";

                    if (Directory.Exists(destinationDirectory))
                    {
                        try
                        {
                            TemplateFile.CleanDirectory(new DirectoryInfo(destinationDirectory));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unable to clean directory {destinationDirectory}. Reason: {ex.Message}. Continuing operation. Files in directory might be old...");
                        }
                    }

                    var sourceDirectoryInfo = sourceFile.Directory;
                    var allFiles = GetFiles(sourceDirectoryInfo);
                    
                    var allTemplateFileTasks = allFiles.Select(f => TemplateFile.CreateTemplateFile(f, sourceDirectoryInfo, destinationFile.Directory, projectNamespace, template.RootNamespace, projectGuid.ToString()));
                    var result = await Task.WhenAll(allTemplateFileTasks).ConfigureAwait(false);
                    var processedTemplateFiles = await CreateDestinationFiles(destinationFile, result.ToList()).ConfigureAwait(false);
                    
                    var vsTemplateFile = await UpdateVsTemplateProject(destinationFile, processedTemplateFiles, projectNamespace, template.RootNamespace).ConfigureAwait(false);
                    noneContentFilesForDestinationProject += $"    <None Include=\"{vsTemplateFile}\" />\r\n";

                    noneContentFilesForDestinationProject += GetFilesToIncludeInProjectfile(processedTemplateFiles);
                }

                if (!string.IsNullOrWhiteSpace(noneContentFilesForDestinationProject))
                {
                    await UpdateDestinationProjectFile(destinationFile, noneContentFilesForDestinationProject).ConfigureAwait(false);
                }

                if (projectNamespacesForRootVsTemplate.Any())
                {
                   await UpdateRootVsTemplateProject(template, projectNamespacesForRootVsTemplate, destinationFile).ConfigureAwait(false);
                }
            }
        }

        private string GetFilesToIncludeInProjectfile(List<TemplateFile> processedTemplateFiles)
        {
            var createdTemplateFiles = processedTemplateFiles.Where(t => t.Created);

            if (createdTemplateFiles?.Any() == true)
            {
                return $"{string.Join("\r\n", createdTemplateFiles.Select(t => $"    {t.ItemGroupEntry}"))}\r\n";
            }

            return string.Empty;
        }

        private async Task UpdateDestinationProjectFile(FileInfo destinationFile, string noneContentFilesForDestinationProject)
        {
            var icon = destinationFile.Name.Replace(".csproj", ".ico");
            noneContentFilesForDestinationProject +=
                $"    <None Include=\"{icon}\" />\r\n";

            var txt = string.Empty;
            var skip = false;

            using (var streamReader = destinationFile.OpenText())
            {
                var line = await streamReader.ReadLineAsync().ConfigureAwait(false);


                while (line != null)
                {
                    if (line.IndexOf("<Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />") >= 0 && !string.IsNullOrWhiteSpace(noneContentFilesForDestinationProject))
                    {
                        txt += "  <ItemGroup>\r\n" + noneContentFilesForDestinationProject + "</ItemGroup>\r\n";
                    }

                    if (line.IndexOf("<None Include=") >= 0)
                    {
                        if (line.IndexOf("/>") < 0)
                        {
                            skip = true;
                        }
                        txt += noneContentFilesForDestinationProject;
                        noneContentFilesForDestinationProject = "";
                    }
                    else if (line.IndexOf("<None>") >= 0)
                    {
                        skip = true;
                    }
                    else if (line.IndexOf("</None>") >= 0)
                    {
                        skip = false;
                    }
                    else
                    {
                        txt += skip ? string.Empty : (line + "\r\n");
                    }

                    line = await streamReader.ReadLineAsync().ConfigureAwait(false);
                }
            }

            destinationFile.Delete();

            using (var fileStream = destinationFile.OpenWrite())
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    await streamWriter.WriteAsync(txt).ConfigureAwait(false);
                }
            }
        }

        private async Task UpdateRootVsTemplateProject(Template template, IEnumerable<string> projectNamespaces, FileInfo destinationFile)
        {
            var vsTemplateFileInfo = new FileInfo(destinationFile.FullName.Replace(".csproj", ".vstemplate"));

            if (!vsTemplateFileInfo.Exists)
            {
                var vsTemplates = destinationFile.Directory.GetFiles("*.vstemplate");
                if (vsTemplates?.Count() == 1)
                {
                    vsTemplateFileInfo = vsTemplates[0];
                }
                else
                {
                    Console.WriteLine("Unable to determine which vs template file to update.");
                    return;
                }
            }

            var icon = destinationFile.Name.Replace(".csproj", ".ico");

            var templateText = string.Empty;

            templateText  += "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
            "<VSTemplate Version=\"3.0.0\" Type=\"ProjectGroup\" xmlns=\"http://schemas.microsoft.com/developer/vstemplate/2005\" xmlns:sdk=\"http://schemas.microsoft.com/developer/vstemplate-sdkextension/2010\">\r\n" +
            "  <TemplateData>\r\n" +
            "    <Name>Unit4 Wanda Chatbot</Name>\r\n" +
            "    <Description>Unit4 Wanda Chatbot Template containing a default template for creating a Unit4 Chatbot.</Description>\r\n" +
            $"    <Icon>{icon}</Icon>\r\n" +
            "    <ProjectType>CSharp</ProjectType>\r\n" +
            "    <RequiredFrameworkVersion>4.6.1</RequiredFrameworkVersion>\r\n" +
            "    <SortOrder>1000</SortOrder>\r\n" +
            "    <TemplateID>74f1f6dd-7858-4ef8-907c-f43724265abe</TemplateID>\r\n" +
            "    <CreateNewFolder>false</CreateNewFolder>\r\n" +
            "    <DefaultName>U4.Area.Chatbot</DefaultName>\r\n" +
            "    <ProvideDefaultName>true</ProvideDefaultName>\r\n" +
            "    <CreateInPlace>true</CreateInPlace>\r\n" +
            "  </TemplateData>\r\n" +
            "  <TemplateContent>\r\n" +
            "    <ProjectCollection>\r\n";

            foreach (var projectNamespace in projectNamespaces)
            {
                templateText += $"      <ProjectTemplateLink ProjectName=\"{ projectNamespace.Replace(template.RootNamespace, "$safeprojectname$")}\" CopyParameters=\"true\">{projectNamespace}\\{projectNamespace}.vstemplate</ProjectTemplateLink>\r\n";
            }

            templateText += "    </ProjectCollection>\r\n" +
            "    <CustomParameters>\r\n" +
            "      <CustomParameter Name=\"$company$\" Value=\"Unit4\"/>\r\n" +
            "      <CustomParameter Name=\"$year$\" Value=\"2018\"/>\r\n" +
            "    </CustomParameters>\r\n" +
            "  </TemplateContent>\r\n" +
            "</VSTemplate>";

            vsTemplateFileInfo.Delete();

            using (var fileStream = vsTemplateFileInfo.OpenWrite())
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    await streamWriter.WriteAsync(templateText).ConfigureAwait(false);
                }
            }
        }


        private async Task<string> UpdateVsTemplateProject(FileInfo destinationFile, List<TemplateFile> processedTemplateFiles, string projectNamespace, string rootNamespace)
        {
            var createdTemplateFiles = processedTemplateFiles.Where(t => t.Created);

            if (createdTemplateFiles?.Any() == true)
            {
                var vsTemplateProjectEntry =
                     $"\t\t<Project File=\"{projectNamespace}.csproj\" ReplaceParameters=\"true\">\r\n{string.Join("\r\n", createdTemplateFiles.Select(t => $"\t\t\t{t.VsTemplateEntry}"))}\r\n" +
                     "\t\t</Project>";

                var templateText = string.Empty;

                var projectName =projectNamespace.Replace(rootNamespace, "$safeprojectname$");

                templateText += "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<VSTemplate Version=\"3.0.0\" Type=\"Project\" xmlns=\"http://schemas.microsoft.com/developer/vstemplate/2005\" xmlns:sdk=\"http://schemas.microsoft.com/developer/vstemplate-sdkextension/2010\">\r\n" +
                    "  <TemplateData>\r\n" +
                    $"    <Name>{projectNamespace}</Name>\r\n" +
                    $"    <Description>{projectNamespace} template definition</Description>\r\n" +
                    "    <ProjectType>CSharp</ProjectType>\r\n" +
                    "    <RequiredFrameworkVersion>2.0</RequiredFrameworkVersion>\r\n" +
                    "    <SortOrder>1000</SortOrder>\r\n" +
                    $"    <TemplateID>{Guid.NewGuid()}</TemplateID>\r\n" +
                    $"    <DefaultName>{projectNamespace}</DefaultName>\r\n" +
                    "  </TemplateData>\r\n" +
                    "  <TemplateContent>\r\n" +
                    vsTemplateProjectEntry + "\r\n" +
                    "  </TemplateContent>\r\n" +
                    "</VSTemplate>\r\n";

                var fileName = $"{projectNamespace}\\{projectNamespace}.vstemplate";
                var vsTemplateFile = destinationFile.Directory.FullName + "\\" + fileName;

                var fileInfo = new FileInfo(vsTemplateFile);

                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                using (var fileStream = fileInfo.OpenWrite())
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        await streamWriter.WriteAsync(templateText).ConfigureAwait(false);
                    }
                }

                return fileName;
            }
            return string.Empty;
        }

        private async Task<List<TemplateFile>> CreateDestinationFiles(FileInfo destinationFile, List<TemplateFile> templateFiles)
        {
            var tasks = templateFiles.Select(t => t.CreateFile());

            var processedTemplateFiles = await Task.WhenAll(tasks).ConfigureAwait(false);
            return processedTemplateFiles.ToList();
        }

        private async Task<string> GetRootNamespace(FileInfo sourceFile)
        {
            var xmlDoc = await TemplateFile.ReadXmlDocument(sourceFile).ConfigureAwait(false);
            var rootNamespaceElements = xmlDoc.GetElementsByTagName("RootNamespace");
            if (rootNamespaceElements?.Count == 1)
            {
                return rootNamespaceElements[0].InnerText;
            }

            return null;
        }

        private List<FileInfo> GetFiles(DirectoryInfo currentDirectory)
        {
            var files = new List<FileInfo>();

            foreach (var file in currentDirectory.EnumerateFiles())
            {
                files.Add(file);
            }

            foreach (var directory in currentDirectory.EnumerateDirectories())
            {
                if (directory.Name == "bin" || directory.Name == "obj")
                {
                    continue;
                }

                var childFiles = GetFiles(directory);
                if (childFiles?.Any() == true)
                {
                    files.AddRange(childFiles);
                }
            }

            return files;
        }
    }
}
