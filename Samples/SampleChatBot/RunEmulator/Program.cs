using Autofac;
using SampleChatBot;
using U4.Bot.Builder.Test.Extensions;
using U4.Bot.Builder.Models;
namespace RunEmulator
{
    class Program
    {
        private class EmulatorChatBotAgent : ChatBotAgent
        {
            private static readonly IContainer _container;

             protected override IContainer Container => _container;

            static EmulatorChatBotAgent()
            {
                ContainerBuilder builder = IoCConfig.GetContainerBuilder();

                builder.UseEmulator();

                _container = builder.Build();
            }
        }
        static void Main(string[] args)
        {
            var conversationContext = new ConversationContext
            {
                Identity = new ConversationIdentity
                {
                    Authority = "https://u4ids-sandbox.u4pp.com/identity",
                    FullName = "Testy McTesty",
                    UserId = "Testy McTesty",
                    AccessToken = "ABC",
                    TenantId = "TestTenant"
                },
                LuisConfiguration = new LuisConfiguration
                {
                   
                    SubscriptionKey = "<add your own Luis subscription key>"
                }
            };
            U4.Bot.Builder.Test.Emulation.Emulator.Start<EmulatorChatBotAgent>(conversationContext);
        }
    }
}
