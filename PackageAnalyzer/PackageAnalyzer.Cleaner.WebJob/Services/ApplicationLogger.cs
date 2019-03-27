using Serilog;
using System;
using Serilog.Core;

namespace PackageAnalyzer.Cleaner.WebJob.Services
{
    internal class ApplicationLogger : IApplicationLogger
    {
        private readonly Logger _logger;

        public ApplicationLogger()
        {
            _logger = new LoggerConfiguration()
              .ReadFrom.AppSettings()
              .CreateLogger();
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(messageTemplate, propertyValues);
        }

        public void Error(Exception ex, string messageTemplate)
        {
            _logger.Error(ex, messageTemplate);
        }
    }
}
