namespace PackageAnalyzer.Core.Model
{
    public class RenderProperties
    {
        public static RenderProperties Default
        {
            get
            {
                return new RenderProperties
                {
                    Count = true,
                    IncludeSolutionInformation = true,
                    NugetProperties = new NugetProperties
                    {
                        Id = true,
                        Version = true,
                        Description = true,
                        Owners = true,
                        ProjectUrl = true,
                        LicenseUrl = true,
                        LicenseType = true
                    }
                };
            }
        }

        public bool Count { get; set; }
        public bool IncludeDuplicates { get; set; }
        public bool IncludePackageDependencies { get; set; }
        public bool PageAutomaticRefresh { get; set; }
        public bool IncludeSolutionInformation { get; set; }
        public NugetProperties NugetProperties { get; set; }
        public ProjectReferenceProperties ProjectReferenceProperties { get; set; }
        public PackageProperties PackageProperties { get; set; }
    }
    
    public class ProjectReferenceProperties
    {
        public bool Name { get; set; }
        public bool Version { get; set; }
        public bool Culture { get; set; }
        public bool PublicKeyToken { get; set; }
        public bool ProcessorArchitecture { get; set; }
        public bool Location { get; set; }
        public bool Private { get; set; }
        public bool ProjectGuid { get; set; }
        public bool LicenseFiles { get; set; }
        public bool ParentProjectName { get; set; }
        public bool ParentProjectPath { get; set; }
    }
    
    public class PackageProperties
    {
        public bool Id { get; set; }
        public bool Version { get; set; }
        public bool TargetFramework { get; set; }
    }

    public class NugetProperties
    {
        public bool Id { get; set; }
        public bool Version { get; set; }
        public bool Description { get; set; }
        public bool LicenseUrl { get; set; }
        public bool LicenseType { get; set; }
        public bool Authors { get; set; }
        public bool Owners { get; set; }
        public bool ProjectUrl { get; set; }
    }
}