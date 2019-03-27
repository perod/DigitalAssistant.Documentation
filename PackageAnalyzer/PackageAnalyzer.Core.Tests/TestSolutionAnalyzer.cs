using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Tests
{
    [TestClass]
    public class TestSolutionAnalyzer
    {
        [Ignore]
        [TestMethod]
        public async Task SolutionAnalyzer_test()
        {
            var fileUtils = new FileUtilities();
            var solutionAnalyzer = new SolutionAnalyzer(new ProjectAnalyzer(fileUtils), fileUtils);
            var projectList = await solutionAnalyzer.AnalyzeSolution(@"C:\VS2015\pp-git\LocationService\LocationService.sln", projectNamesToIgnore: new List<string> { "Test" });
        }
    }
}
