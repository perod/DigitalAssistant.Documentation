# IConversationContext

The ConversationContext object is established by the `AssistantService` and passed on to the chatbots/dialogs in each call. It contains information about the current conversation, the current user, which chatbots theuser has access to, as well as the credentials needed to communicate with the Microsoft Bot Connector and settings for calls to the LUIS applications.

The `IConversationContext` is accessible within dialogs from the `IDialogContext.GetConversationContext()` extension method.

## Properties
### ConversationIdentity
`ConversationIdentity` contains information about the current user, such as  full name, `Unit4Id` and `TenantId` as well as access token and authority needed for calls to the other systems.

### BotCredentials
`BotCredentials` contains information about the current bot, such as name (for example Wanda, Wendy, etc.) and credentials. This information is usually not needed by the chatbot developer, but is used by the U4.Bot.Builder framework to communicate with Microsoft's bot framework.

### Agents
`Agents` returns a list of all chatbots that are registered in the current bot and the current user has access to.

### LUIS Configuration
`LuisConfiguration` contains the settings for the communication with the LUIS applications, such as `CacheExpiration`, `MinimumIntentScore`, `Environment` name and a list of `SubscriptionKeys`, and is used by the U4.Bot.Builder framework for communication with LUIS applications.
* `CacheExpiration` controls how long a LuisResult is cached without being hit again ("SlidingExpiration"). Setting this value to "0" disables caching. 
* `MinimumIntentScore` controls how confident the "Master LUIS app" used by the CanHandle service has to be about which agent can handle a spedific user utterance. This setting does not affect the results of any other LUIS applications.
* The name of the LUIS `Environment` (for eample "Wanda004") is used to determine the endpoint of a specific version of a LUIS application.
* `SubscriptionKeys` contains a list of [luis.ai](https://www.luis.ai) subscriptions that are used to resolve the endpoint of a specific LUIS application and version.