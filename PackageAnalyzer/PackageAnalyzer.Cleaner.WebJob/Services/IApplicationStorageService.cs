using PackageAnalyzer.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Cleaner.WebJob.Services
{
    public interface IApplicationStorageService
    {
        Task<List<StorageIdentifier>> GetSolutionListStorageIdentifiers();
        Task<bool> Delete(string blobName);
    }
}
