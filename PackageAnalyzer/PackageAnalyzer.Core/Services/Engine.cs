using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace PackageAnalyzer.Core.Services
{
    public class Engine : IEngine
    {
        private readonly ISolutionAnalyzer _solutionAnalyzer;
        private readonly IStorageService _storageService;
        private readonly IHtmlRenderer _htmlRenderer;

        public Engine(ISolutionAnalyzer solutionAnalyzer, IStorageService storageService, IHtmlRenderer htmlRenderer)
        {
            _solutionAnalyzer = solutionAnalyzer;
            _storageService = storageService;
            _htmlRenderer = htmlRenderer;
        }

        public void InitializeStorage(string buildId, string storageConnectionString, string storageName)
        {
            _storageService.Initialize(buildId, storageConnectionString, storageName);
        }

        public async Task Run(SolutionConfiguration solutionConfiguration)
        {
            var solutionResult = await AnalyzeSolutions(solutionConfiguration);
            await StoreResults(solutionConfiguration, solutionResult);
            await Render(solutionConfiguration, solutionResult);
        }


        private async Task StoreResults(SolutionConfiguration solutionConfiguration, SolutionList solutionList)
        {
            try
            {
                await _storageService.SetSolutionList(solutionConfiguration, solutionList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<SolutionList> AnalyzeSolutions(SolutionConfiguration solutionConfiguration)
        {
            var tasks = new List<Task<Solution>>();

            foreach (var solutionInformation in solutionConfiguration.Solutions)
            {
                var solutionFile = Path.Combine(solutionConfiguration.RootFolder, solutionInformation.SolutionFile);
                tasks.Add(_solutionAnalyzer.AnalyzeSolution(solutionFile, solutionInformation.ProjectReferencesToIgnore, solutionInformation.ProjectsToIgnore));
            }

            var results = await Task.WhenAll(tasks);
            var retVal = new SolutionList();
            retVal.AddRange(results);
            return retVal;
        }
        
        private async Task Render(SolutionConfiguration solutionConfiguration, SolutionList solutionList)
        {
            await _htmlRenderer.Render(solutionConfiguration, solutionList);
        }
    }
}
