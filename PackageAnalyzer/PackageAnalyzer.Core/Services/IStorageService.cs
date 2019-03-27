using Microsoft.WindowsAzure.Storage.Blob;
using PackageAnalyzer.Core.Model;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public interface IStorageService
    {
        Task<bool> SetSolutionList(SolutionConfiguration solutionConfiguration, SolutionList solutionList);
        Task<SolutionList> GetSolutionList(string identifier);
        Task<StorageIdentifiersSegment> GetSolutionListIdentifersSegmented(BlobContinuationToken token = null, int segmentSize = 100);
        Task<bool> DeleteSolutionList(string identifier);

        void Initialize(string buildId, string storageConnectionString, string storageName);
    }
}
