using System;

namespace PackageAnalyzer.Cleanup.Function.Services
{
    internal class ApplicationConfiguration : IApplicationConfiguration
    {
        public string AzureWebJobsStorage => Get("AzureWebJobsStorage", "UseDevelopmentStorage=true");

        public string StorageContainerName => Get("StorageContainerName", "packageanalyzer");

        private string Get(string name, string defaultValue = null)
        {
            var retVal = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            return !string.IsNullOrWhiteSpace(retVal) ? retVal : defaultValue;
        }
    }
}
