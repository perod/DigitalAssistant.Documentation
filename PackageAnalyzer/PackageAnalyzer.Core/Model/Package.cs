namespace PackageAnalyzer.Core.Model
{
    public class Package
    {
        public Package()
        {
            Id = string.Empty;
            Version = string.Empty;
            TargetFramework = string.Empty;
        }

        public string Id { get; set; }
        public string Version { get; set; }
        public string TargetFramework { get; set; }
    }
}
