using Autofac;
using $ext_safeprojectname$.Services.BusinessLogic;
using $ext_safeprojectname$.Services.Health;

namespace $ext_safeprojectname$.Ioc.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BusinessLogicService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ChatbotHealthChecker>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
