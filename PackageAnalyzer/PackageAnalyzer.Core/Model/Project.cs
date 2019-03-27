namespace PackageAnalyzer.Core.Model
{
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ProjectReferenceList ProjectReferences { get; set; }
        public PackageList Packages { get; set; }
    }
}
