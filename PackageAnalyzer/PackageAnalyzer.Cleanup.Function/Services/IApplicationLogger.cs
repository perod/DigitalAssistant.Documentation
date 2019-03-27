using System;
using Microsoft.Azure.WebJobs.Host;

namespace PackageAnalyzer.Cleanup.Function.Services
{
    public interface IApplicationLogger
    {
        void Initialize(TraceWriter log);

        void Information(string message, string source = null);
        void Error(string messageTemplate, Exception ex = null, string source = null);
        
    }
}
