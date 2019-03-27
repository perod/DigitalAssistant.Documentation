using PackageAnalyzer.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public interface ISolutionConfigurationReader
    {
        Task<List<SolutionConfiguration>> GetSolutionConfigurations(string solutionConfigurationFile);
    }
}
