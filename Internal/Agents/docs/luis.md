# LUIS

> This topic applies to v3 of the U4.Bot.Builder. 

[LUIS (Language Understanding Intelligent Service)](https://www.luis.ai) offers a fast and effective way of adding language understanding to applications. With LUIS, you can use pre-existing, world-class, pre-built models. And when you need specialized models, LUIS guides you through the process of quickly building them.

In LUIS you define a [LUIS Application](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/home) that is given *Utterances* which you group into predefined *Intents*. LUIS then uses machine learning to train the LUIS application, so that correct *Intent* is returned when a new *Utterance* is passed to the LUIS application. When properly trained, the LUIS application is published, and will be available through a LUIS endpoint.

To learn more about LUIS in general see the [official Microsoft LUIS documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/home). 

## Using LUIS in a chatbot

You can integrate LUIS in your chatbot by either using any of the `LuisDialog` classes, or by using the `ILuisRequestService` provided by the U4.Bot.Builder. 

### ILuisRequestService
The `ILuisRequestService` instance is registered in the IoC and can be injected into any class that is registered in the central IoC container.

```csharp

//Get the luis result containing the best matched 
//LUIS intent with connected LUIS entites etc.
//
//Note: There are overloads to this method where you can
//specify LUIS application id and subscription key
//instead of luisApplicationName.
var luisResult = await _luisRequestService
	.Request(
		new LuisRequest(userInput),
		
		//Wanda main LUIS configuration can be fetched from the
		//current conversation context, or it can be created
		//from scratch. Contains Luis subscription and endpoint
		//information ++.
		luisConfiguration,

		//The name of luis application to call.
		luisApplicationName)
	.ConfigureAwait(false);
```
The return value from this method is a `Microsoft.Bot.Builder.Luis.Models.LuisResult` containing intents and entities resolved by the LUIS application. This information can be further analyzed to present the best fitted user response.

### LuisDialog/CachingLuisDialog/ResolvingLuisDialog
A chatbot communicates with LUIS by routing messages to a [`LuisDialog`](https://docs.botframework.com/en-us/csharp/builder/sdkreference/dialogs.html). Internally, the message text will be passed as an *utterance* to a LUIS endpoint before reaching the `LuisDialog`.

```csharp
internal class ConversationAgent 
{
    [MessageService]
    public async Task Message(IMessageActivity request, IConversationContext conversationContext)
    {
        // Using the convention to determine the model-id and subscription-key for the LuisDialog.
        var res = await Conversation.SendAsync<TestLuisDialog>(request, conversationContext);
        return res;
    }
}
```

> The `IConversationContext` is accessible within your dialog through the `IDialogContext.GetConversationContext()` extension method.  

When the LUIS application returns scored intents, the returned intent names are paired with methods tagged with a `LuisIntent` attribute. The method with the best matching intent will be called. `U4.Bot.Builder` delivers a custom `LuisDialog` named [ResolvingLuisDialog](dialogs.md) for communicating with the LUIS subscription provided with the current conversation context. The following code block shows an example of a `ResolvingLuisDialog`:

```csharp
[LuisApplicationName("MyChatbot")]
[Serializable]
internal class TestLuisDialog : ResolvingLuisDialog<IMessageActivity>
{
	public TestLuisDialog(LuisServiceFactory luisServiceFactory, ILuisSubscriptionResolutionService luisSubscriptionResolutionService, ILuisServiceCreator luisServiceCreator)
		: base(typeof(SalutationDialog), luisServiceFactory, luisSubscriptionResolutionService, luisServiceCreator)
	{}

	public TestLuisDialog(params ILuisService[] services)
		: base(services)
	{}

	[LuisIntent("Intent_DoSomething")]
	public async Task DoSomethingHandlerAsync(IDialogContext context, LuisResult result)
	{
		var conversationContext = context.GetConversationContext();
		...
		await context.PostAsync("I did something");
		context.Wait(DoSomethingElseAsync);
	}

	 //Required for the None intent (catch all)
	[LuisIntent("")]
	public async Task CatchAllAsync(IDialogContext context, LuisResult result)
	{
		...
		await context.Complete($"You said: {result.Query}");
	}

	private async Task DoSomethingElseAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
	{
		...
		await context.Complete(result);
	}
	...
}
```

**Remarks:**

* A [`IDialog<T>`](https://docs.botframework.com/en-us/csharp/builder/sdkreference/dd/d5e/interface_microsoft_1_1_bot_1_1_builder_1_1_dialogs_1_1_i_dialog.html) is a suspendable conversational process that produces a result of type *T*. A `LuisDialog` will typically return a `IMessageActivity` to the user, which again means that the preferred way of declearing a custom LuisDialog is to inherit `LuisDialog<IMessageActivity>`.

* Dialogs must be marked `Serializable` as the complete dialog state of a conversation is serialized, stacked and persisted through the Messages sent to and from the underlying Bot connector.

