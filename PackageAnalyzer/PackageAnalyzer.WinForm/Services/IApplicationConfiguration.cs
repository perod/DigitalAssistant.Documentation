namespace PackageAnalyzer.WinForm.Services
{
    public interface IApplicationConfiguration
    {
        string StorageConnectionString { get; }
        string StorageContainerName { get; }
    }
}
