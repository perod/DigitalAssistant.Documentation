using PackageAnalyzer.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.WinForm.Services
{
    public interface IApplicationStorageService
    {
        Task<List<StorageIdentifier>> GetSolutionListStorageIdentifiers();

        Task<SolutionList> GetSolutionList(string blobName);
    }
}
