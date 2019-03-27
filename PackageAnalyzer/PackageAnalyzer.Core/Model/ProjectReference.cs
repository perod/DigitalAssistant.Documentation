using System.Collections.Generic;

namespace PackageAnalyzer.Core.Model
{
    public class ProjectReference
    {
        public ProjectReference()
        {
            Name = string.Empty;
            Version = string.Empty;
            Culture = string.Empty;
            PublicKeyToken = string.Empty;
            ProcessorArchitecture = string.Empty;
            Location = string.Empty;
            ProjectGuid = string.Empty;
            Package = new Package();
            LicenseUrls = new List<string>();
            LicenseFiles = new List<LicenseFile>();
            ParentProjectName = string.Empty;
            ParentProjectPath = string.Empty;
            NugetPackage = new NugetPackage();
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string Culture { get; set; }
        public string PublicKeyToken { get; set; }
        public string ProcessorArchitecture { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public string Location { get; set; }
        public bool Private { get; set; }
        public string ProjectGuid { get; set; }
        public Package Package { get; set; }
        public List<string> LicenseUrls { get; set; }
        public List<LicenseFile> LicenseFiles { get; set; }
        public string ParentProjectName { get; set; }
        public string ParentProjectPath { get; set; }
        public NugetPackage NugetPackage { get; set; }
    }
}
