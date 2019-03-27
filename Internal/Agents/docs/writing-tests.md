# Writing unit tests for a dialog

> This topic applies to v3 of the U4.Bot.Builder. 

The `U4.Bot.Builder.Test` nuget package provides an API to write fluent dialog tests that mimic an actual chatbot conversation. A fluent test can be used to verify both dialog conversation flow and that expected services are called etc. The nuget package is delivered with mocking framework [Moq](https://github.com/moq/moq4) making it easy to setup mocks for services used by the dialog.

## ChatbotDialog - The dialog to test
The following code example shows a simple dialog. When constructed, it is given an instance of a IBusinesLogicService, and is then just waiting for user input. When user input is received, it queries the business logic service instance for a response to the user input, and then completes the dialog with this response.

```csharp
[Serializable]
public class ChatbotDialog : IDialog<IMessageActivity>
{
	[NonSerialized] 
	IBusinessLogicService _businessLogicService;
	public IBusinessLogicService BusinessLogicService
	{
		get { return _businessLogicService; }
		set { _businessLogicService = value; }
	}

	public ChatbotDialog(IBusinessLogicService businessLogicService)
	{
		BusinessLogicService = businessLogicService;
	}

	public Task StartAsync(IDialogContext context)
	{
		//Wait for user input
		context.Wait(MessageReceived);
		return Task.CompletedTask;
	}

	private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
	{
		var message = await result;
		var response = await BusinessLogicService.GetFeatures(message.Text);
		await context.Complete(response);
	}
}
```
## ChatBotDialogTest - Fluent test setup
To test the aforementioned dialog in a fluent way, a `FluentTests` instance must be initialized with all services required by the dialog. Typically, these services are mocked for testing purposes, and are assigned to the IoC container builder of the test instance when the fluent test is created.

```csharp
private FluentTests<IMessageActivity, ChatbotDialog> InitializeChatbotDialogFluentTest()
{
	//Private instance - so that other tests can assign different responses dependant on what to test
	BusinessLogicServiceMock = new Mock<IBusinessLogicService>();
	BusinessLogicServiceMock.Setup(b => b.GetFeatures(It.IsAny<string>())).ReturnsAsync("You have to be very specific.");
	BusinessLogicServiceMock.Setup(b => b.GetFeatures("What can you do for me?")).ReturnsAsync("I can predict the future.");

	//FluentTest<IMessageActivity, ChatBotDialog> means that 
	//the ChatBotDialog is of type IDialog<IMessageActivity>.
	return new FluentTests<IMessageActivity, ChatbotDialog>()
		.SetContainerMocks(b =>
		{
			//Register service mocks with the IoC container builder (Autofac)
			b.RegisterInstance(BusinessLogicServiceMock.Object)
				.As<IBusinessLogicService>()
				.SingleInstance();
		})

		//Mehtod Interactive kicks off the test, passing
		//along initial dialog constructor parameters etc. The method holds
		//overloads for setting up other dialog features, such as the
		//dialog conversation context if that is required.
		.Interactive(BusinessLogicServiceMock.Object);
}
```
## ChatBotDialogTest - Fluent dialog test
After initializing the fluent test instance, the actual test can be written as a regular unit test in a fluent way, utlizing the `Message`method of the `FluentTests` instance. The following example illustrates this:
```csharp
//A standard MSTest unit test
[TestMethod]
public void ChatbotDialog_get_features()
{
	var fluentTest = InitializeChatbotDialogFluentTest();

	fluentTest
		.Message("What can you do for me?") //Post question to dialog
		.Message(messages => {
			//Wait for dialog response
			Assert.AreEqual(1, messages.Count);
			Assert.AreEqual("I can predict the future.", messages[0].Text);
		});
}
```
If the dialog under test has a more complex conversational flow, a fluent test can handle this by responding to a message received from the dialog:

```csharp
[TestMethod]
public void Dialog_flow_test()
{
	var fluentTest = SetupFluentTest();

	fluentTest
		.Message("I want something to eat.")
		.Message(messages =>
		{
			var message = messages.First();
			Assert.AreEqual("What do you want to eat?", message.Text);
			return message.MakeReply("I want fish and chips"); //respond 
		})
		.Message(messages =>
		{
			var message = messages.First();
			Assert.AreEqual("Sorry, we only serve pizza.", message.Text);
			return message.MakeReply("Ok, give me a pizza then."); //respond
		})
		.Message(messages =>
		{
			var message = messages.First();
			Assert.AreEqual("What type of pizza do you want?", message.Text);
			return message.MakeReply("A big pepperoni pizza"); //respond
		})
		.Message(messages =>
		{
			var message = messages.First();
			Assert.AreEqual("Ok, one big pepperoni pizza coming up.", message.Text);
		});
}
```

