using PackageAnalyzer.Core.Services;
using SimpleInjector;
using System;

namespace PackageAnalyzer.Core.Ioc
{
    public class ContainerBuilder
    {
        public static Container CreateContainer(Action<Container> registrationAction = null)
        {
            var container = new Container();

            container.Register<IProjectAnalyzer, ProjectAnalyzer>(Lifestyle.Singleton);
            container.Register<ISolutionAnalyzer, SolutionAnalyzer>(Lifestyle.Singleton);
            container.Register<IEngine, Engine>(Lifestyle.Singleton);
            container.Register<IFileUtilities, FileUtilities>(Lifestyle.Singleton);
            container.Register<ISolutionConfigurationReader, SolutionConfigurationReader>(Lifestyle.Singleton);
            container.Register<ILicenseMapper, LicenseMapper>(Lifestyle.Singleton);
            container.Register<IStorageService, StorageService>(Lifestyle.Singleton);
            container.Register<IHtmlRenderer, HtmlRenderer>(Lifestyle.Singleton);
            
            registrationAction?.Invoke(container);

            container.Verify();
            return container;
        }
    }
}
