using System.Threading.Tasks;

namespace PackageAnalyzer.Cleanup.Function.Services
{
    public interface ICleanerEngine
    {
        Task Cleanup(int numItemsToKeep = 5);
    }
}
