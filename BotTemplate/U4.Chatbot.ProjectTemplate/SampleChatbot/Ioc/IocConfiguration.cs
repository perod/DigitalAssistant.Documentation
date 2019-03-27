using Autofac;
using $ext_safeprojectname$.Ioc.Modules;
using U4.Bot.Builder;

namespace $ext_safeprojectname$.Ioc
{
    public static class IocConfiguration
    {
        public static IContainer Container { get; }

        static IocConfiguration()
        {
            ContainerBuilder builder = GetContainerBuilder();
            Container = builder.Build();
        }

        public static ContainerBuilder GetContainerBuilder()
        {
            //Initiate a new builder with services registered in U4.Bot.Builder
            //and in the core Microsoft Bot Framework.
            var builder = Conversation.GetContainerBuilder();

            //Register components and services used by the $ext_safeprojectname$.
            builder.RegisterModule<DialogsModule>();
            builder.RegisterModule<ServicesModule>();

            return builder;
        }
    }
}
