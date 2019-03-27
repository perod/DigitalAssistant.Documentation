using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer.Core.Services;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Tests
{
    [TestClass]
    public class TestProjectAnalyzer
    {
        
        [TestMethod]
        public async Task ProjectAnalyzer_ReadProjectFile()
        {
            var analyzer = new ProjectAnalyzer(new FileUtilities());
            var projectReferenceList = await analyzer.AnalyzeProject(@"C:\VS2015\pp-git\LocationService\U4PP.Service.Location.API\U4PP.Service.Location.API.csproj");
            ;
        }
    }
}
