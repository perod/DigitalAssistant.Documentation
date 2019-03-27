using PackageAnalyzer.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public interface ISolutionAnalyzer
    {
        Task<Solution> AnalyzeSolution(string solutionFile, List<string> projectReferenceNamesToIgnore = null, List<string> projectNamesToIgnore = null);
    }
}
