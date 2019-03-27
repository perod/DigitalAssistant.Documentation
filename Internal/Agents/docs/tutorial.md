
# Tutorial for feature developers

This tutorial is meant for developers who want to put their ideas for Wanda features into real life samples without digging into the details of the bot framework more than necessary.

An agent is a standalone piece of the digital assistant eco-system. It listens for relevant messages coming in, it handles the dialog with the user and your agent should provide information that's helpful for the user. This tutorial covers what you need to setup and how you can test you agent.

You can start building an agent without connecting it to the digital assistant by just writing tests. Let's do that first. Then we will see how to use the Bot Framework Emulator and console app. Deploying your agent to the People Platform is not covered here since it's still under development.

## Agent - Dialog - Tests
Create a new class library project on .Net4.6.1 and name it U4.Bot.Agent.youragentname. Then create a second class library and call it U4.Bot.Agent.youragentname.Tests


Get U4.Bot.Builder from U4PP nuget and add it to both projects. Then add U4.Bot.Builder.Test from NuGet on your test project.
These packages are hosted at :
[https://packages.u4pp.com/nuget-dev/nuget](https://packages.u4pp.com/nuget-dev)

## Agent and dialogs
Your agent basically consists of an agent class and a dialog class.

### Agent class

This uses the magic of U4.Bot.Builder to connect your class library to the bot framework by implementing the IAgent interface

```csharp
public class CleverbotAgent : IAgent
{
    [MessageService()]
    public async Task<IList<IMessageActivity>> Message(IMessageActivity request, IConversationContext context)
    {
        var res = await Builder.Conversation.SendAsync<CleverbotDialog>(request, context);
        return res;
    }
}
```

### Dialog class
Your dialog needs a constructor and methods decorated with `LuisIntent` to direct different intents to different functions. `Context.Complete()` sends message back to the user with your reply. Complete also closes the conversation telling the system that you are not waiting for a reply. If you ask a question back to the user that your are waiting for you would make a `PostAsync`. 

```csharp
[Serializable]
[LuisModel("4a120a18-170b-4396-94cf-259cda0602b7", "e8cc19d84513485ca90155201672df62")]
public class CleverbotDialog : LuisDialog<IMessageActivity>
{
    public const string INTENT_FLIRTY = "Flirty";
    public const string INTENT_NONE = "None";
    public CleverbotDialog(params ILuisService[] services)
    : base(services)
    {
    }

    [LuisIntent(INTENT_FLIRTY)]
    public async Task CleverbotFlirty(IDialogContext context, LuisResult result)
    {
        var reply = await GetReplyFromCleverbot(result.Query); //fetch from cleverbot
        await context.Complete(reply);
    }
    [LuisIntent(INTENT_NONE)]
    public async Task CleverbotNone(IDialogContext context, LuisResult result)
    {
        var reply = await GetReplyFromCleverbot(result.Query); //fetch from cleverbot
        await context.Complete(reply);
    }
}
```

> **Note**: You have the option to add the LUIS `ModelId` and `SubscriptionKey` to the `appSettings`. The settings key have to be prefixed with the dialog class name. By convention `U4.Bot.Builder` will read these settings if the class is not decorated with the `LuisModel` attribute.
>
>		<appSettings>
>			... 
>			<add key="CleverbotDialog-ModelId" value="<guid>" />
>			<add key="CleverbotDialog-SubscriptionKey" value="<key>" />
>			... 
>		</appSettings>

If you are not using LUIS for your agent you can make a simple dialog just implementing the [IDialog](https://docs.botframework.com/en-us/csharp/builder/sdkreference/dialogs.html).

```csharp
[Serializable]
public class CleverbotSimpleDialog : IDialog<IMessageActivity>
{
    public async Task StartAsync(IDialogContext context)
    {
        context.Wait(GetMessageBack);
    }
    private async Task GetMessageBack(IDialogContext context,IAwaitable<IMessageActivity> aresult)
    {
        var result = await aresult;
        string reply = await Cleverbot.GetReplyFromCleverbot(result.Text);
        await context.Complete(reply);
    }
}
```

### Tests
Use [this guide](/recipes/#write-a-unit-test-for-a-dialog) to write unit tests for your app.

# LUIS
LUIS is available for you to use as a developer to be able to understand the content of text from the user. LUIS will both create an intent for the message and connect it with entities such as number, dates, location. So when the user says. "we can plan this event in Barcelona next week." You will receive both Intent for planning, and entities for Barcelona and next week for you to handle in your response. The intents are your responsibility to create by training the LUIS in different ways you can say things when you mean that it is for e.g. planning.

## Training your own LUIS
Training Luis is done by creating a new intent. You can create your own subscription on LUIS to test it out, but when deploying you agent as a part of the Digital assistant we want all LUIS apps to be hosted on the u4peopleplatform luis account. Here is how to [create a new LUIS app](/luis/#using-luis).

If you have a simple agent and don't need special intents or entities such as dates and location you don't need you own app on LUIS. To be able to test your agent through the digitial assistant you do how ever to create an intent on the "main" LUIS that is used by the [arbiter](/glossary). 

## The Main LUIS used by the arbiter
This LUIS app is called Agent on the u4peopleplatform subscription, and you need to open it and enter a new intent that will forward messages coming in to your agent through this intent.

![LUIS](images/mainluis.jpg "Agent bot as the main LUIS")

Make sure that your name your intent to something that is unique to your agent, and train it so that the utterances can not be mistaken for other intents on the main LUIS.

# Chat with your agent
There is a couple of options to test the chat with your bot without deploying your agent to the digital assistant eco system.

## Bot Framework emulator
You can use an emulator to test and debug your agent which means that the messages will go directly to your agent without being routed to the digital assistant and arbiter. To use the Micorsoft Bot Emulator to test your agent and dialogs, you can use the U4 Agent Emulator is shipped as a part of the **U4.Bot.Builder.Test** package. To learn how to use the agent emulator read this [guide](emulator.md).

### Install the emulator
You can find the emulator here: [Bot FrameWork Emulator](https://download.botframework.com/botconnector/tools/emulator/publish.htm)

### Setup the emulator
You only need to setup the address to you agent API to make the emulator work.
![Emulator](images/BotFrameworkEmulator.jpg "Bot Framework Emulator")

>**Note**: By convention the agent will be hosted at **http://localhost:43200/api/messages** if url is not specified.

## Console app
You can test you agent against one of the digital assistants setup on slack or facebook by calling your agent in a console app and listening to the messages coming in. Create a new console application project in your solution, make sure to install the u4.Bot.Builder nuget package. This approach requires that your main LUIS intents has been registered with Wanda.

```csharp
static void Main(string[] args)
{
    var keyWords = ConfigurationManager.AppSettings["MessageService.Names"];
    var prefix = ConfigurationManager.AppSettings["MessageService.Prefix"];

    MessageService.Setup<CleverbotAgent>();

    Console.WriteLine("Cleverbot agent started. Listening at \"{0}{1}\" queue.", prefix, keyWords);
    Console.ReadLine();        
}
```
Use this setup in your app.config

The `MicrosoftAppId` is the actual digital assistant, like Wanda or Wendy that is registered at the Microsoft Bot Framework by the People Platform team (the AppId in the example is for Wendy that you can talk to on slack)

The servicebus connectionstring is using the azure infrastructure that the digital assistant is forwarding messages to and is hosted on the people platform development subscription in Azure. 

The `MessageService.Names` and Prefix is to the servicebus message queue that your app is listening to. This is taken from your intent name on the main luis. The prefix is used to make it easier to test when something is already deployed on the same intent. The prefix name you need to listen to is setup on the assistant web app appsettings on azure.

```xml
<add key="MicrosoftAppID" value="99e04b8d-2c35-4a41-af08-af47b294db3e" />
<add key="MicrosoftAppPassword" value="bnhsZsCUaSOnQcPa9zbSOsf" />

<add key="MessageService.ConnectionString" value="Microsoft.ServiceBus.ConnectionString" />
<add key="MessageService.Names" value="Salutation" />
<add key="MessageService.Prefix" value="" />
<add key="Microsoft.ServiceBus.ConnectionString" value="Endpoint=sb://wandatest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=rCyWGDrNuhunsRsvUCUiO5+jI9K5qZ77YUU+PiLDfjM=" />
```

When you run your console you can use the the rest of the Digital Assistant infrastructure to and it will capture your intents in the console and you can debug/test. 

