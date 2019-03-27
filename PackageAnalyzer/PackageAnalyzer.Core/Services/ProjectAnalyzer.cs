using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace PackageAnalyzer.Core.Services
{
    public class ProjectAnalyzer : IProjectAnalyzer
    {
        private readonly IFileUtilities _fileUtilities;

        public ProjectAnalyzer(IFileUtilities fileUtilities)
        {
            _fileUtilities = fileUtilities;
        }

        public async Task<Project> AnalyzeProject(string csProjPath, List<string> projectReferenceNamesToIgnore = null)
        {
            var doc = new XmlDocument();

            var fileInfo = new FileInfo(csProjPath);

            doc = await _fileUtilities.ReadXmlDocument(fileInfo);

            if (doc != null)
            {
                projectReferenceNamesToIgnore = projectReferenceNamesToIgnore == null ? new List<string>() : projectReferenceNamesToIgnore;

                var project = new Project();
                project.ProjectReferences = new ProjectReferenceList();

                project.Name = fileInfo.Name;
                project.Path = fileInfo.FullName;

                var packageList = await GetPackageFile(doc, fileInfo.DirectoryName);
                project.Packages = packageList;
                project.ProjectReferences.AddRange(await GetReferences(doc, projectReferenceNamesToIgnore, project, packageList, fileInfo.DirectoryName));
                project.ProjectReferences.AddRange(await GetPackageReferences(doc, projectReferenceNamesToIgnore, project, packageList, fileInfo.DirectoryName));
                project.ProjectReferences.AddRange(GetProjectReferences(doc, projectReferenceNamesToIgnore, project));

                return project;
            }

            return null;
        }

        private async Task<PackageList> GetPackageFile(XmlDocument doc, string csProjPath)
        {
            var packageList = new PackageList();

            var packageName = GetPackageName(doc, "None", "Content");
            if (!string.IsNullOrWhiteSpace(packageName))
            {
                var path = Path.Combine(csProjPath, packageName);
                var packageFileInfo = new FileInfo(path);

                var packageDocument = await _fileUtilities.ReadXmlDocument(packageFileInfo);

                var packageNodes = packageDocument.GetElementsByTagName("package");

                foreach (XmlNode packageNode in packageNodes)
                {
                    if (packageNode != null && packageNode.Attributes.Count > 0)
                    {
                        var package = new Package();

                        foreach (XmlAttribute attribute in packageNode.Attributes)
                        {
                            var attributeName = attribute.Name.ToLowerInvariant();

                            if (attributeName.CompareTo("id") == 0)
                            {
                                package.Id = attribute.Value;
                            }
                            else if (attributeName.CompareTo("version") == 0)
                            {
                                package.Version = attribute.Value;
                            }
                            else if (attributeName.CompareTo("targetframework") == 0)
                            {
                                package.TargetFramework = attribute.Value;
                            }
                        }

                        packageList.Add(package);
                    }
                }
            }

            return packageList;
        }


        private string GetPackageName(XmlDocument doc, params string[] nodesToSearch)
        {
            foreach (var nodeToSearch in nodesToSearch)
            {
                var nodes = doc.GetElementsByTagName(nodeToSearch);

                foreach (XmlNode node in nodes)
                {
                    if (node != null)
                    {
                        var include = node.Attributes["Include"];

                        if (include != null && include.Value.ToLowerInvariant().IndexOf("packages.config") >= 0)
                        {
                            return include.Value;
                        }
                    }
                }
            }

            return null;
        }


        /*
        Understands this format:

            <ProjectReference Include="..\U4PP.Location.Core\U4PP.Location.Core.csproj">
                <Project>{72A15398-3F20-47B7-BFA1-40470F95D167}</Project>
                <Name>U4PP.Location.Core</Name>
            </ProjectReference> 
             
        */
        private List<ProjectReference> GetProjectReferences(XmlDocument doc, List<string> projectReferenceNamesToIgnore, Project parentProject)
        {
            var projectReferences = new List<ProjectReference>();

            var projectReferenceNodes = doc.GetElementsByTagName("ProjectReference");

            foreach (XmlNode node in projectReferenceNodes)
            {
                if (node != null && node.ChildNodes != null)
                {
                    var projectReference = new ProjectReference();

                    projectReference.ParentProjectName = parentProject.Name;
                    projectReference.ParentProjectPath = parentProject.Path;

                    var include = node.Attributes["Include"];

                    if (include != null)
                    {
                        var value = include.Value;
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            projectReference.Location = value;
                        }
                    }

                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode != null)
                        {
                            if (childNode.Name.CompareTo("Project") == 0)
                            {
                                projectReference.ProjectGuid = childNode.InnerText;
                            }
                            else if (childNode.Name.CompareTo("Name") == 0)
                            {
                                projectReference.Name = childNode.InnerText;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(projectReference.Name) && projectReferenceNamesToIgnore?.Any(p => projectReference.Name.IndexOf(p) >= 0) == false)
                    {
                        projectReferences.Add(projectReference);
                    }
                }
            }
            return projectReferences;
        }


        private async Task<List<ProjectReference>> GetPackageReferences(XmlDocument doc, List<string> projectReferenceNamesToIgnore, Project parentProject, PackageList packageList, string csProjPath)
        {
            var references = new List<ProjectReference>();

            var referenceNodes = doc.GetElementsByTagName("PackageReference");

            foreach (XmlNode node in referenceNodes)
            {
                if (node != null)
                {
                    var projectReference = new ProjectReference();
                    projectReference.ReferenceType = ReferenceType.PackageReference;

                    projectReference.ParentProjectName = parentProject.Name;
                    projectReference.ParentProjectPath = parentProject.Path;

                    var includeAttr = node.Attributes["Include"];
                    
                    if (includeAttr != null)
                    {
                        projectReference.Name = includeAttr.Value;
                    }

                    var versionAttr = node.Attributes["Version"];
                    if(versionAttr != null)
                    {
                        projectReference.Version = versionAttr.Value;
                    }

                    if (!string.IsNullOrWhiteSpace(projectReference.Name) && projectReferenceNamesToIgnore?.Any(p => projectReference.Name.IndexOf(p) >= 0) == false)
                    {
                        await GetPackageReferenceFromGlobalPackageFolder(projectReference);
                        references.Add(projectReference);
                    }
                }
            }
            return references;
        }


        private async Task GetPackageReferenceFromGlobalPackageFolder(ProjectReference projectReference)
        {
            if(!string.IsNullOrWhiteSpace(projectReference.Name) && !string.IsNullOrWhiteSpace(projectReference.Version))
            {
                var globalNugetPath = @"%UserProfile%\.nuget\packages\" + $"{projectReference.Name}\\{projectReference.Version}";
                var path = Environment.ExpandEnvironmentVariables(globalNugetPath);

                var directory = new DirectoryInfo(path);

                if(!directory.Exists)
                {
                    globalNugetPath = @"%LocalAppData%\NuGet\Cache\" + $"{projectReference.Name}\\{projectReference.Version}";
                    path = Environment.ExpandEnvironmentVariables(globalNugetPath);
                    directory = new DirectoryInfo(path);
                }

                if(directory.Exists)
                {
                    var licenseFiles = directory.GetFiles("*license*");
                    var nupkgFiles = directory.GetFiles("*.nupkg");

                    if (nupkgFiles?.Any() == true)
                    {
                        projectReference.NugetPackage = await GetNugetPackageInformation(nupkgFiles[0]);
                    }

                    if (licenseFiles?.Any() == true)
                    {
                        projectReference.LicenseFiles = await GetLicenses(licenseFiles);
                    }

                }
            }
        }

        /*
        Understands these formats:

            <Reference Include="Serilog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10, processorArchitecture=MSIL">
                <HintPath>..\packages\Serilog.2.6.0\lib\net46\Serilog.dll</HintPath>
                <Private>True</Private>
            </Reference>

            <Reference Include="Microsoft.VisualStudio.QualityTools.CodedUITestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
                <Private>False</Private>
            </Reference>

            <Reference Include="System.Configuration" />  
        */
        private async Task<List<ProjectReference>> GetReferences(XmlDocument doc, List<string> projectReferenceNamesToIgnore, Project parentProject, PackageList packageList, string csProjPath)
        {
            var references = new List<ProjectReference>();

            var referenceNodes = doc.GetElementsByTagName("Reference");

            foreach (XmlNode node in referenceNodes)
            {
                if (node != null)
                {
                    var projectReference = new ProjectReference();

                    projectReference.ParentProjectName = parentProject.Name;
                    projectReference.ParentProjectPath = parentProject.Path;

                    var include = node.Attributes["Include"];

                    if (include != null)
                    {
                        var value = include.Value;
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            var sValue = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var attributeValue in sValue)
                            {
                                var tAttributeValue = attributeValue.Trim();
                                if (tAttributeValue.IndexOf("Version=") == 0)
                                {
                                    projectReference.Version = tAttributeValue.Replace("Version=", "");
                                }
                                else if (tAttributeValue.IndexOf("Culture=") == 0)
                                {
                                    projectReference.Culture = tAttributeValue.Replace("Culture=", "");
                                }
                                else if (tAttributeValue.IndexOf("PublicKeyToken=") == 0)
                                {
                                    projectReference.PublicKeyToken = tAttributeValue.Replace("PublicKeyToken=", "");
                                }
                                else if (tAttributeValue.IndexOf("processorArchitecture=") == 0)
                                {
                                    projectReference.ProcessorArchitecture = tAttributeValue.Replace("processorArchitecture=", "");
                                }
                                else
                                {
                                    projectReference.Name = tAttributeValue;
                                }
                            }
                        }
                    }

                    if (node.ChildNodes != null)
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode != null)
                            {
                                if (childNode.Name.CompareTo("HintPath") == 0)
                                {
                                    projectReference.Location = childNode.InnerText;

                                    if (childNode.InnerText.IndexOf(@"\packages\") > 0)
                                    {
                                        await ApplyPackageInformationToProjectReference(projectReference, packageList, csProjPath);
                                    }
                                }
                                else if (childNode.Name.CompareTo("Private") == 0)
                                {
                                    var isPrivateText = childNode.InnerText;
                                    bool isPrivate;
                                    bool.TryParse(isPrivateText, out isPrivate);
                                    projectReference.Private = isPrivate;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(projectReference.Name) && projectReferenceNamesToIgnore?.Any(p => projectReference.Name.IndexOf(p) >= 0) == false)
                    {
                        references.Add(projectReference);
                    }
                }
            }
            return references;
        }

        private async Task ApplyPackageInformationToProjectReference(ProjectReference projectReference, PackageList packageList, string csProjPath)
        {
            projectReference.ReferenceType = ReferenceType.PackageReference;
            
            var fileInfo = new FileInfo(Path.Combine(csProjPath, projectReference.Location));

            if (fileInfo.Exists)
            {
                var directory = fileInfo.Directory;
                int position = 0;
                while (position < 3 && directory != null)
                {
                    var licenseFiles = directory.GetFiles("*license*");
                    var nupkgFiles = directory.GetFiles("*.nupkg");

                    if (nupkgFiles?.Any() == true)
                    {
                        projectReference.NugetPackage = await GetNugetPackageInformation(nupkgFiles[0]);
                    }

                    if (licenseFiles?.Any() == true)
                    {
                        projectReference.LicenseFiles = await GetLicenses(licenseFiles);
                    }

                    directory = directory.Parent;
                    position++;
                }

                if (projectReference.NugetPackage != null)
                {
                    projectReference.Package = packageList.GetPackageByName(projectReference.NugetPackage.Id);
                }
            }
        }


        private async Task<NugetPackage> GetNugetPackageInformation(FileInfo nuGetFileInfo)
        {
            var tempPath = Path.GetTempPath();
            var archivePath = Path.Combine(tempPath, Guid.NewGuid().ToString());
            System.IO.Compression.ZipFile.ExtractToDirectory(nuGetFileInfo.FullName, archivePath);

            var directory = new DirectoryInfo(archivePath);

            var nuspecs = directory.GetFiles("*.nuspec");

            if (nuspecs?.Any() == true)
            {   
                var nuspec = nuspecs.First();
                var xml = await _fileUtilities.ReadXmlDocument(nuspec);
                var metadataNodes = xml.GetElementsByTagName("metadata");
                if (metadataNodes?.Count == 1)
                {

                    var retVal = new NugetPackage();
                    foreach(XmlNode child in metadataNodes[0].ChildNodes)
                    {
                        if(child != null)
                        {
                            var value = child.InnerText;
                            switch(child.Name.ToLowerInvariant())
                            {
                                case "id":
                                    retVal.Id = value;
                                    break;
                                case "version":
                                    retVal.Version = value;
                                    break;
                                case "authors":
                                    retVal.Authors = value;
                                    break;
                                case "licenseurl":
                                    retVal.LicenseUrl = value;
                                    break;
                                case "projecturl":
                                    retVal.ProjectUrl = value;
                                    break;
                                case "owners":
                                    retVal.Owners = value;
                                    break;
                                case "description":
                                    retVal.Description = value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    return retVal;

                }
            }

            return null;
        }


        private async Task<List<string>> GetLicenseUrls(FileInfo[] nupkgFiles)
        {
            var retVal = new List<string>();
            foreach(var nupkgFile in nupkgFiles)
            {
                var tempPath = Path.GetTempPath();
                var archivePath = Path.Combine(tempPath, Guid.NewGuid().ToString());
                System.IO.Compression.ZipFile.ExtractToDirectory(nupkgFile.FullName, archivePath);

                var directory = new DirectoryInfo(archivePath);

                var nuspecs = directory.GetFiles("*.nuspec");

                if(nuspecs?.Any() == true)
                {
                    foreach(var nuspec in nuspecs)
                    {
                        var xml = await _fileUtilities.ReadXmlDocument(nuspec);
                        var licenseElements = xml.GetElementsByTagName("licenseUrl");

                        if(licenseElements.Count > 0)
                        {
                            foreach(XmlNode licenseElement in licenseElements)
                            {
                                if(licenseElement != null)
                                {
                                    retVal.Add(licenseElement.InnerText);
                                }
                            }
                        }
                    }
                }

                _fileUtilities.DeleteDirectory(directory);
            }

            return retVal;
        }

        private async Task<List<LicenseFile>> GetLicenses(FileInfo[] licenseFiles)
        {
            var licenses = new List<LicenseFile>();

            foreach (var file in licenseFiles)
            {
                var license = new LicenseFile
                {
                    FileName = file.Name,
                    FullName = file.FullName
                };

                licenses.Add(license);
                license.Content= await _fileUtilities.ReadFile(file);
            }

            return licenses;
        }
    }
}
