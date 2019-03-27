using PackageAnalyzer.Cleanup.Function.Services;
using PackageAnalyzer.Core.Ioc;
using SimpleInjector;

namespace PackageAnalyzer.Cleanup.Function
{
    public static class Ioc
    {
        public static Container Container;

        static Ioc()
        {
            Container = CreateIocContainer();
        }

        private static Container CreateIocContainer()
        {
            return ContainerBuilder.CreateContainer((container) =>
            {
                container.Register<ICleanerEngine, CleanerEngine>(Lifestyle.Singleton);
                container.Register<IApplicationConfiguration, ApplicationConfiguration>(Lifestyle.Singleton);
                container.Register<IApplicationStorageService, ApplicationStorageService>(Lifestyle.Singleton);
                container.Register<IApplicationLogger, ApplicationLogger>(Lifestyle.Singleton);
            });
        }
    }
}
