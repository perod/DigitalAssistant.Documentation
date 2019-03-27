using System.Threading.Tasks;

namespace PackageAnalyzer.Cleaner.WebJob.Services
{
    public interface ICleanerEngine
    {
        Task Cleanup(int numItemsToKeep = 5);
    }
}
