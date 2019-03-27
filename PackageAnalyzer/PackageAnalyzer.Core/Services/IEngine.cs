using PackageAnalyzer.Core.Model;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public interface IEngine
    {
        Task Run(SolutionConfiguration solutionConfiguration);
        void InitializeStorage(string buildId, string storageConnectionString, string storageName);
    }
}
