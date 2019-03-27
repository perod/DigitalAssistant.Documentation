# Proactive conversations


A chatbot that only responds to user input is nice but the real power of bots lies in their ability to anticipate what the user wants to do, and initiate conversation with the user.

A proactive chatbot could recognize patterns in the user's communication or use external triggers (calendar, task systems etc.) to be more agentive and make the user's life easier.
 
The Wanda ecosystem supports two types of proactive conversation:

- One-shot notifications
- Chatbot proactive conversations

## Architecture

The proactive conversations concept in the Wanda ecosystem requires that the user who should receive the proactive message is known to the system. 

A prerequisite is that the [identity](user-identity) of a user is established in the system. 
This identity makes it possible to map an internal user identifier to a user in the supported bot channels. The ecosystem will also keep track of what social channel the user last used, and this will be considered as the primary channel for proactive conversations.

The entry point for sending proactive messages to users exists in the central Wanda bot. The Digital Assistant has a dedicated Web API endpoint that's used to initiate any proactive conversations with the user. 
This endpoint accepts a `Notification` DTO. Systems that post to this endpoint need a valid IDS token obtained from a central IDS instance.

The trigger of any proactive conversations in the ecosystem would in most cases be a self-contained service that lives outside of the receiving chatbot service.

Proactive conversation requests sent to the Digital Assistant will be handled centrally and delegated to the chatbot in accordance with a set of rules. If the target user is currently engaged in a conversation, then the message will be put in a conversation queue and sent to the user when the current conversation ends.
It's also possible to interrupt an ongoing conversations by setting the `NotificationConfig.NotificationType` to Interrupt. The central system will pause the users current conversation and start a new one. The paused conversation will be continued when the proactive conversation ends.

### Notification DTO

The notification DTO sent to the Wanda Digital Assistant must specify the [identity](user-identity) (`NotificationIdentity.TenantId` and `NotificationIdentity.UserId`) and either the `Message` or `Data`. 
Setting the `Message` results in a one-shot notification, otherwise the notification will initiate a proactive conversation with the specified chatbot (`AgentId`).
`Priority` and `NotificationType` default to NotificationPriority.Low and NotificationType.Wait.

This code snippet describes the Notification DTO:

``` csharp
    public class Notification
    {
        public string SourceId { get; set; }
        public string AgentId { get; set; }
        public NotificationIdentity Identity { get; set; }
        public NotificationMessage Message { get; set; }
        public JObject Data { get; set; }
        public NotificationConfig Config { get; set; }
    }

    public class NotificationIdentity
    {
        public string FullName { get; set; }
        public string UserId { get; set; }
        public string TenantId { get; set; }
    }

    public class NotificationMessage
    {
        public string Text { get; set; }
    }

    public class NotificationConfig
    {
        public DateTime? ExpiresAt { get; set; }
        public NotificationPriority Priority { get; set; }
        public NotificationType Type { get; set; }
    }

    public enum NotificationPriority
    {
        High = 0,
        Moderate = 1,
        Low = 2
    }

    public enum NotificationType
    {
        Interupt = 0,
        Wait = 1
    }

```

`SourceId` specifies what source system the proactive messages request originates from. 
The `NotificationIdentity` specifies which user should get the proactive message. If that identity is unknown to the ecosystem, the Web API will respond with HttpCode 422.
For chatbot proactive conversations the `Data` property will carry the contextual data that will be sent to the chatbot.


## One-shot notifications

A one-shot notification is a single message sent to a user in Wanda and will not initiate a conversation between the user and a chatbot.
This can be useful in proactive scenarios where you want to send a single notification that does not require any feedback from the user.

## Chatbot proactive conversations

Chatbot proactive conversations are notifications that are handed off to chatbots in the ecosystem to handle the proactive request. 
In this scenario the notification sent to the central Digital Assistant will carry some custom data that is used by the chatbot to initiate a conversation with the user.
Chatbot proactive conversations could be single notifications, but also initiate a conversation that requires interaction from the user.

The same rules as for one-shot notifications apply to chatbot proactive conversations. The only difference is that the chatbot proactive conversations are put in the central conversation stack maintained for the user.

## Add support for proactive conversations in chatbots

> This topic applies to v3 of the U4.Bot.Builder. 

To add support for chatbot proactive conversations your chatbot must implement the `IProactiveAgent` interface and decorate the interface method with `[MessageServiceMethod(MessageMethods.ConversationStarted)]`.

``` csharp
public class ProactiveChatbot : IAgent, IProactiveAgent
{
    protected virtual IContainer Container => TasksIocConfig.Container;

    [MessageService]
    public async Task Message(IMessageActivity message, IConversationContext context, CancellationToken cancellationToken)
    {
        await Conversation.SendAsync<StandardDialog>(message, context, Container, null, cancellationToken).ConfigureAwait(false);
    }

    [MessageServiceMethod(MessageMethods.HealthCheck)]
    public async Task HealthCheckReceived()
    {
        ...
    }

    [MessageServiceMethod(MessageMethods.ConversationStarted)]
    public async Task ConversationStarted(IMessageActivity startMessage, IConversationContext conversationContext, JObject data)
    {
        startMessage.ChannelData = data;
        await Conversation.SendAsync<ProactiveDialog>(startMessage, conversationContext, Container).ConfigureAwait(false);
    }
}
```

The `ConversationStarted` method takes a `JObject` parameter that is the contextual data that originates from the notification request sent to the central digital assistant.
In this example it's passed to the dialog in the `ChannelData` property of the `IActivityMessage`. 

> The way `ChannelData` is used in this example has some potential drawbacks. It's safer to use the `Entities` property instead to transport the custom data to your dialog. 
> Another way is to use the `scopedServices` parameter of the `SendAsync` to pass in a service that provides the custom data to your proactive dialog. 
> This service will be registered in Autofac and can be resolved inside the dialog.

In the dialog that handles the proactive conversation request you can serialize the JObject to a strongly typed class for further processing.
This infers that there is a **contract** between the proactive conversation trigger and the chatbot with regards to the shape of the custom data object.

``` csharp
[Serializable]
public class ProactiveDialog : IDialog<IMessageActivity>
{
    public virtual Task StartAsync(IDialogContext context)
    {
        context.Wait(MessageReceived);
        return Task.CompletedTask;
    }

    protected async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
    {
        var message = await item;
       
        ...  

        JObject response = message.ChannelData;
        MyDto proactiveConversationData = response.ToObject<MyDto>();

        ...
    }
}
```