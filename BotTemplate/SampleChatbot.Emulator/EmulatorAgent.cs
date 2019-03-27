using Autofac;
using SampleChatbot.Ioc;
using U4.Bot.Builder.Test.Extensions;

namespace SampleChatbot.Emulator
{
    public class EmulatorAgent : Agent
    {
        private static readonly IContainer _container;

        protected override IContainer Container => _container;

        static EmulatorAgent()
        {
            var builder = IocConfiguration.GetContainerBuilder();

            //Ensure that core Ioc services can run with the emulator.
            builder.UseEmulator();

            _container = builder.Build();
        }
    }
}
