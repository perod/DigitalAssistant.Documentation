using System;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace PackageAnalyzer.Cleaner.WebJob.Services
{
    internal class ApplicationConfiguration : IApplicationConfiguration
    {
        private string _prefix = "packageanalyzer";

        public string StorageConnectionString => Get("storage-connection-string", "UseDevelopmentStorage=true");

        public string StorageContainerName => Get("storage-container-name", "packageanalyzer");


        private T Get<T>(string name, T defaultValue = default(T))
        {
            var key = $"{_prefix}:{name}";

            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                var value = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return defaultValue;
        }
    }
}
