using PackageAnalyzer.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Cleanup.Function.Services
{
    public interface IApplicationStorageService
    {
        Task<List<StorageIdentifier>> GetSolutionListStorageIdentifiers();
        Task<bool> Delete(string blobName);
    }
}
