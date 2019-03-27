using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer.Core.Model;
using PackageAnalyzer.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Tests
{
    [TestClass]
    public class TestStorageService
    {
        [TestMethod]
        public async Task StorageService_SetSolutionList()
        {
            var fileUtils = new FileUtilities();
            var solutionAnalyzer = new SolutionAnalyzer(new ProjectAnalyzer(fileUtils), fileUtils);
            var solution = await solutionAnalyzer.AnalyzeSolution(@"C:\VS2015\pp-git\LocationService\LocationService.sln", projectNamesToIgnore: new List<string> { "Test" });
            var solutionList = new SolutionList { solution };
            var storage = new StorageService();
            storage.Initialize("1000.01", "UseDevelopmentStorage=true", "solutioncontainer");

            for (var i = 0; i < 20; i++)
            {
                await storage.SetSolutionList(new SolutionConfiguration
                {
                    StorageIdentifier = "LocationService",
                    AreaTags = SolutionAreaTag.Location | SolutionAreaTag.TimeCapture | SolutionAreaTag.DigitalAssistant
                }, 
                solutionList);
            }
        }

        [TestMethod]
        public async Task StorageService_GetSolutionList()
        {
            var storage = new StorageService();
            storage.Initialize("1000.01", "UseDevelopmentStorage=true", "solutioncontainer");
            var solutionList = await storage.GetSolutionList("LocationService");
        }

        [TestMethod]
        public async Task StorageService_GetSolutionListIdentifiersSegmented()
        {
            var storage = new StorageService();
            storage.Initialize("1000.01", "UseDevelopmentStorage=true", "solutioncontainer");

            var segment = await storage.GetSolutionListIdentifersSegmented(segmentSize: 2);

            if (segment?.StorageIdentifiers.Any() == true)
            {
                var storageIdentifiers = new List<StorageIdentifier>();
                storageIdentifiers.AddRange(segment.StorageIdentifiers);
                
                do
                {
                    segment = await storage.GetSolutionListIdentifersSegmented(
                        token: segment.ContinuationToken,
                        segmentSize: 2);

                    if (segment?.StorageIdentifiers.Any() == true)
                    {
                        storageIdentifiers.AddRange(segment.StorageIdentifiers);
                    }

                } while (segment?.ContinuationToken != null);

                var ordered = storageIdentifiers
                    .OrderBy(s => s.Name)
                    .ThenByDescending(s => s.Date)
                    .ToList();
                ;
            }   
        }
    }
}
