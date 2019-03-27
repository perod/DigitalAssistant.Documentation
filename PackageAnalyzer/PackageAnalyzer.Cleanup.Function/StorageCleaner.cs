using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using PackageAnalyzer.Cleanup.Function.Services;

namespace PackageAnalyzer.Cleanup.Function
{
    public static class StorageCleaner
    {
        //"0 0 8 * * 1" Every monday at 08am
        //"*/20 * * * * *" Every 20 sec
        private const int _numItemsToKeep = 5;
        private const string _logSource = "StorageCleaner";

        [FunctionName("StorageCleaner")]
        public static async Task Run([TimerTrigger("0 0 8 * * 1")]TimerInfo myTimer, TraceWriter log)
        {
            var logger = Ioc.Container.GetInstance<IApplicationLogger>();
            logger.Initialize(log);

            try
            {
                logger.Information($"PackageAnalyzer - removing old data, keeping max { _numItemsToKeep} instances of each result.", _logSource);
                var engine = Ioc.Container.GetInstance<ICleanerEngine>();
                await engine.Cleanup(_numItemsToKeep);
            }
            catch (Exception ex)
            {
                logger.Error("An error ocured while running the cleanup process", ex, _logSource);
            }
            finally
            {
                logger.Information("PackageAnalyzer - done", _logSource);
            }
        }
    }
}
