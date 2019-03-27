using PackageAnalyzer.Core.Ioc;
using PackageAnalyzer.WinForm.Services;
using SimpleInjector;

namespace PackageAnalyzer.WinForm
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
                container.Register<IApplicationConfiguration, ApplicationConfiguration>(Lifestyle.Singleton);
                container.Register<ISolutionListTreeViewService, SolutionListTreeViewService>(Lifestyle.Singleton);
                container.Register<IAreaTagCheckedListBoxService, AreaTagCheckedListBoxService>(Lifestyle.Singleton);
                container.Register<IActionService, ActionService>(Lifestyle.Singleton);
                container.Register<IViewerService, ViewerService>(Lifestyle.Singleton);
                container.Register<IRenderingOptionsTreeViewService, RenderingOptionsTreeViewService>(Lifestyle.Singleton);
                container.Register<IMainFormService, MainFormService>(Lifestyle.Singleton);

                container.Register<IApplicationStorageService, ApplicationStorageService>(Lifestyle.Singleton);
            });
        }
    }
}
