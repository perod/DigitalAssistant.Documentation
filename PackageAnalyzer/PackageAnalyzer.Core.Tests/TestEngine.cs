using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer.Core.Model;
using PackageAnalyzer.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Core.Tests
{
    [TestClass]
    public class TestEngine
    {
        [TestMethod]
        public async Task Engine_renderTo()
        {
            var fileUtilities = new FileUtilities();
            var renderer = new Engine(new SolutionAnalyzer(new ProjectAnalyzer(fileUtilities), fileUtilities), new StorageService(), new HtmlRenderer(fileUtilities, new LicenseMapper(fileUtilities))); //In real life by ioc :D
            await renderer.Run(new SolutionConfiguration
            {
                StorageIdentifier = "TravelAndExpense",
                AreaTags =  
                    SolutionAreaTag.FunctionalAggregator | 
                    SolutionAreaTag.Chatbots | 
                    SolutionAreaTag.TravelAndExpense | 
                    SolutionAreaTag.DigitalAssistant,               
                PageTitle = "Travel & Expense Functional Aggreagor and Chatbots",
                HeaderText = "Third party assemblies used by the Travel & Exense Functional Aggreagator with connected Chatbots",
                OutputPath = @"c:\temp\PackageAnalyzer",
                IndexFileName = "Travel.html",
                RootFolder = @"C:\VS2015\pp-git\TravelDomain\",
                RenderProperties = RenderProperties.Default,
                Solutions = new List<SolutionInformation>
                {
                    new SolutionInformation
                    {
                        SolutionFile = @"U4.TravelDomain\U4.TravelDomain.sln",
                        ProjectReferencesToIgnore = new List<string> { "U4" },
                        ProjectsToIgnore = new List<string> { "Test" }
                    },
                    new SolutionInformation
                    {
                        SolutionFile = @"U4.TravelRequest\U4.TravelRequest.sln",
                        ProjectReferencesToIgnore = new List<string> { "U4" },
                        ProjectsToIgnore = new List<string> { "Test", "Emulator" }
                    },
                    new SolutionInformation
                    {
                        SolutionFile = @"U4.Expenses.Agent\U4.Expenses.Agent.sln",
                        ProjectReferencesToIgnore = new List<string> { "U4" },
                        ProjectsToIgnore = new List<string> { "Test", "Receipt", "Mock", "Console", "Luis", "Emulator" }
                    }
                }
            });
        }
    }
}
