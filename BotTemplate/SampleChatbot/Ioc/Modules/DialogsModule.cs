using Autofac;
using SampleChatbot.Dialogs;
using SampleChatbot.Dialogs.Children;

namespace SampleChatbot.Ioc.Modules
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
