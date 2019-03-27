using Autofac;
using SampleChatbot.Services.BusinessLogic;
using SampleChatbot.Services.Health;

namespace SampleChatbot.Ioc.Modules
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
