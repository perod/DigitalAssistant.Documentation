namespace PackageAnalyzer.Cleanup.Function.Services
{
    public interface IApplicationConfiguration
    {
        string AzureWebJobsStorage { get; }
        string StorageContainerName { get; }
    }
}
