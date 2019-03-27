using Autofac;
using $ext_safeprojectname$.Dialogs;
using $ext_safeprojectname$.Dialogs.Children;

namespace $ext_safeprojectname$.Ioc.Modules
{
    public class DialogsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainDialog>();
            builder.RegisterType<ChildDialog>();
        }
    }
}
