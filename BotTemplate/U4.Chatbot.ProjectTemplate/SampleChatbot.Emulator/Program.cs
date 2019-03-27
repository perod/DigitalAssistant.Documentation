using U4.Bot.Builder.Models;

namespace $ext_safeprojectname$.Emulator
{
    class Program
    {
        static void Main()
        {
            var conversationContext = new ConversationContext
            {
                //The identity of the user running the chatbot
                Identity = new ConversationIdentity
                {
                    FullName = "John Doe",
                    UserId = "Average Joe",

                    AccessToken = "", //Ids Access token identifying the user.
                    Authority = "", //Ids Authority used to authorize and grant access.
                    TenantId = "" //The current tenant
                },

                //Setup the core Luis configuration used by services defined
                //in U4.Bot.Builder and this chatbot.
                LuisConfiguration = new LuisConfiguration
                {
                    CacheExpiration = 0,
                    SubscriptionKey = "",
                    EndpointKey = ""
                },
                
                //Azure keyvault configuration. Can be used to access 
                //chatbot keyvault secrets through U4.Bot.Builder.Interfaces.ISecretService.
                VaultConfig = new KeyVaultConfig
                {
                    ClientId = "",
                    ClientSecret = "",
                    Url = "" //Azure keyvault url

                }
            };

            U4.Bot.Builder.Test.Emulation.Emulator.Start<EmulatorAgent>(conversationContext);
        }
    }
}
