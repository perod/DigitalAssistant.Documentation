using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.IO;
using PackageAnalyzer.Cleaner.WebJob.Services;
using System;

namespace PackageAnalyzer.Cleaner.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        private const int _numItemsToKeep = 5;

        static void Main()
        {
            var host = new JobHost();
            var task = host.CallAsync(typeof(Program).GetMethod("RunCleanerEngine"));
            task.Wait();
        }

        [NoAutomaticTrigger]
        public static async Task RunCleanerEngine(TextWriter log)
        {
            var logger = Ioc.Container.GetInstance<IApplicationLogger>();
            
            try
            {
                logger.Information("PackageAnalyzer - removing old data, keeping max {numItemsToKeep} instances of each result.", _numItemsToKeep);
                var engine = Ioc.Container.GetInstance<ICleanerEngine>();
                await engine.Cleanup(_numItemsToKeep);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error ocured while running the cleanup process");
            }
            finally
            {
                logger.Information("PackageAnalyzer - done");
            }
        }
    }
}
