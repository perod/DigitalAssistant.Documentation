namespace PackageAnalyzer.Cleaner.WebJob.Services
{
    public interface IApplicationConfiguration
    {
        string StorageConnectionString { get; }
        string StorageContainerName { get; }
    }
}
