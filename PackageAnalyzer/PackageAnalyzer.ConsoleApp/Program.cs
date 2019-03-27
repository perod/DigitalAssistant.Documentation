using PackageAnalyzer.Core.Ioc;
using PackageAnalyzer.Core.Model;
using PackageAnalyzer.Core.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageAnalyzer.ConsoleApp
{
    class Program
    {
        private static Container _container;

        static Program()
        {
            _container = ContainerBuilder.CreateContainer();
        }

        static void Main(string[] args)
        {
            try
            {
                var argumentSet = ParseArguments(args);
                if (argumentSet != null)
                {
                    var readSolutionConfigurationsTask = ReadSolutionConfigurations(argumentSet.SolutionConfigurations);
                    readSolutionConfigurationsTask.Wait();
                    if (readSolutionConfigurationsTask.Status == TaskStatus.RanToCompletion)
                    {
                        var solutionConfigurations = readSolutionConfigurationsTask.Result;

                        OverrideRootFolderAndOutputLocation(argumentSet, solutionConfigurations);
                        
                        var htmlRenderer = _container.GetInstance<IEngine>();
                        htmlRenderer.InitializeStorage(argumentSet.BuildId, argumentSet.StorageConnectionString, argumentSet.StorageName);

                        var tasks = new List<Task>();
                        foreach (var solutionConfiguration in solutionConfigurations)
                        {
                            tasks.Add(htmlRenderer.Run(solutionConfiguration));
                        }

                        Task.WaitAll(tasks.ToArray());

                        Console.WriteLine("Operation complete. See output folder(s) for results.");
                    }
                    else
                    {
                        Console.WriteLine("Unable to read solution configurations. Aborting further operations...");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }

        private static void OverrideRootFolderAndOutputLocation(ArgumentSet argumentSet, List<SolutionConfiguration> solutionConfigurations)
        {
            if (!string.IsNullOrWhiteSpace(argumentSet.OutputPath))
            {
                solutionConfigurations.ForEach(s =>
                {
                    s.OutputPath = argumentSet.OutputPath;
                });
            }

            if(argumentSet.RootFolders?.Any() == true)
            {
                var count = argumentSet.RootFolders.Count;
                for (var i = 0; i < solutionConfigurations.Count; i++)
                {
                    var solutionConfiguration = solutionConfigurations[i];

                    if (count > i)
                    {
                        solutionConfiguration.RootFolder = argumentSet.RootFolders[i];
                    }
                }
            }
        }

        private static ArgumentSet ParseArguments(string[] args)
        {
            var argumentSet = ArgumentSet.ParseArguments(args);
            if (!argumentSet.IsValid())
            {
                Console.WriteLine(ArgumentSet.GetArgumentMessage());
                return null;
            }
            return argumentSet;
        }

        private static async Task<List<SolutionConfiguration>> ReadSolutionConfigurations(string solutionConfigurations)
        {
            var solutionConfigurationReader = _container.GetInstance<ISolutionConfigurationReader>();
            return await solutionConfigurationReader.GetSolutionConfigurations(solutionConfigurations);
        }
    }
}
