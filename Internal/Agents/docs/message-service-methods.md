# Message Service Methods

Message Service methods mark methods in your handler/agent class which will receive messages from the service. The first method `SimpleMessage` in the `MyAgent` class is marked with the [MessageServiceMethod("message")] attribute. This function will be called whenever the application receives a message looking for a function marked with "message" and has no parameters. 

The second method `Message` has a `MessageService` attribute applied. The `MessageService` attribute is a sub-class of the `MessageServiceMethod` which automatically sets name to "message". 

There are two methods in this agent class that are marked with the same MessageServiceMethod ("message"). When a message is received the parameters will determine which method is called. If a message doesn't match name and parameters correctly nothing will be called.

To receive messages from the AssistantService the agent has to implement the IAgent interface and set the `MessageService` attribute on the Message method that implements the interface.

The `ConversationStarted` method is required to implement the IProactiveAgent interface which enables the agent to receive StartConversation callbacks from the AssistantService. That method has to be marked with the [MessageServiceMethod(MessageMethods.ConversationStarted)] attribute.

```csharp
public class MyAgent : IAgent, IProactiveAgent
{
	protected virtual IContainer Container => MyConfig.Container;
	
    [MessageServiceMethod("message")]
    public Task SimpleMessage()
    {
        return Task.CompletedTask;
    }

    [MessageService]
	public Task Message(IMessageActivity message, IConversationContext conversationContext)
	{
		return Conversation.SendAsync<MyDialog>(message, conversationContext, Container);
	}

	[MessageServiceMethod(MessageMethods.ConversationStarted)]
	public async Task ConversationStarted(IMessageActivity startMessage, IConversationContext conversationContext, JObject data)
	{
		if (null == startMessage)
		{   // could not open conversation with given user
			return;
		}

		try
		{
			startMessage.ChannelData = data.ToObject<MyDto>();
			await Conversation.SendAsync<MyDialog>(startMessage, conversationContext, Container).ConfigureAwait(false);
		}
		catch (Exception)
		{
			// TODO: some logging
		}
	}
}
```