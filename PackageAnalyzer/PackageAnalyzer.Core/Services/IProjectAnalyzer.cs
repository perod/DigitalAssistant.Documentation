using PackageAnalyzer.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Services
{
    public interface IProjectAnalyzer
    {
        Task<Project> AnalyzeProject(string csProjPath, List<string> projectReferenceNamesToIgnore = null);
    }
}
