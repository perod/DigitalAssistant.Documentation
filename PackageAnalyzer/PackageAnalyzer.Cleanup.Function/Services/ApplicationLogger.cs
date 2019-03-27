using System;
using Microsoft.Azure.WebJobs.Host;

namespace PackageAnalyzer.Cleanup.Function.Services
{
    internal class ApplicationLogger : IApplicationLogger
    {
        private TraceWriter _log;

        public void Information(string message, string source = null)
        {
            _log.Info(message, source);
        }

        public void Error(string messageTemplate, Exception ex = null, string source = null)
        {
            _log.Error(messageTemplate, ex, source);
        }

        public void Initialize(TraceWriter log)
        {
            _log = log;
        }
    }
}
