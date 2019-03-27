using System;

namespace PackageAnalyzer.Cleaner.WebJob.Services
{
    public interface IApplicationLogger
    {
        void Information(string messageTemplate, params object[] propertyValues);
        void Error(Exception ex, string messageTemplate);
    }
}
