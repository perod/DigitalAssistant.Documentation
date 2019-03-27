
# Adaptive Cards

Microsoft presented the Adaptive Card concept during Build 2017 as a unified way to present data to the user on various communication channels. Sadly, even today (June 2018) almost no channels (not even Microsoft's own) are able to natively display Adaptive Cards.
The only exceptions are the BotFramework emulator, BotFramework Web chat and Skype web client. However, native Adaptive Card support will come "soon" to Skype apps and Microsoft Teams. See [Channel Status](https://docs.microsoft.com/en-us/adaptive-cards/resources/partners) for more details.

As a fallback until then (and for channels that do not / will not support Adaptive Cards) Adaptive Cards can be rendered to images and sent as attachments to the user.

## Adaptive Card concept

Adaptive Cards are an open card exchange format enabling developers to exchange UI content in a common and consistent way. Card content is described as a simple JSON object and then natively rendered on the client side.
The .NET AdaptiveCard NuGet package provides classes to make card authoring easier.

Check out the [samples](http://adaptivecards.io/samples/) for inspiration and what you can do with Adaptive Cards.
For more information see [Microsoft's documentation](https://docs.microsoft.com/en-us/adaptive-cards/).

## AdaptiveCardRenderingService

The `IAdaptiveCardServiceClient` provides an interface to a Unit4 web service that renders Adaptive Cards with predefined dimensions depending on the target channel and returns the image as a base64 encoded string.
Those dimensions are different for each channel so make sure the Adaptive Card image is displayed properly. If the image has different dimensions the channel might only show the top left corner or the center of the image etc.

### Limitations

- The dimensions are fixed. Regardless of the content, the rendering service will always produce an image 400px wide and 209px high for Facebook, and 400px x 225px for Skype, Slack and Microsoft Teams.
This means that until a channel natively supports Adaptive Cards we can't show huge amounts of data to the user. Depending on font size, spacing etc. the limit is around 10 lines.
- It's not possible to use input fields for channels that do not natively support Adaptive Cards.
- Buttons will not be rendered by the Rendering service (since they would be useless anyway) and have to be added to the message as HeroCard buttons.

## Sending AdaptiveCards to the user

> This topic applies to v3 of the U4.Bot.Builder. 

The U4.Bot.Builder package provides an IMessageActivity extension method to make sending Adaptive Cards to the user easier.
`IMessageActivity.AttachAdaptiveCard` checks if the target channel natively supports AdaptiveCards and either adds the card to the message as a JSON attachment or sends the card to the Unit4 Rendering service and, depending on the channel, adds the rendered image as an image attachment (Facebook and Slack) or as a HeroCard attachment. 
All Adaptive Card Actions (buttons etc.) are automatically turned into HeroCard Buttons if the channel does not natively support Adaptive Cards.

This example shows how to use AttachAdaptiveCard:
```csharp
private async Task DisplayCard(IDialogContext context, ResumeAfter<IMessageActivity> resumeMethod)
{
	var adaptiveCard = new AdaptiveCard
	{
		Body = new List<AdaptiveElement> { ... }
		FallbackText = " ... ",  // fall back text in case the rendering of the Adaptive Card fails.
		Actions = new List<AdaptiveAction> { ... }
	};

	var adaptiveCardServiceClient = context.Resolve<IAdaptiveCardServiceClient>();
	var serviceUrl = "https://u4-da-dev-adaptive-cards.azurewebsites.net";
	var message = context.MakeMessage();
	await message.AttachAdaptiveCard(
		adaptiveCard: adaptiveCard,     // [required] AdaptiveCard object to attach to the message.
		title: "Hello World message",   // [optional] Used as title of the attachment. Some channels display this. 
		leadText: "This is my message to the world",    // [optional] Text shown before the Adaptive Card.
		serviceClient: adaptiveCardServiceClient, // [required] if the channel does not natively support Adaptive Cards. [optional] otherwise.
		serviceUrl: serviceUrl,     // [optional] If not provided the sercice url is read from the AppSettings key "ac:service-uri".
		forceImage: false,          // [default: false] Force rendered image even if the channel natively supports Adaptive Cards. Useful for use with the emulator.
		speak: "Hello world")       // [optional] Will be set as the Speak property of the message.
		.ConfigureAwait(false);

	await context.PostAsync(message).ConfigureAwait(false);
	context.Wait(resumeMethod);
}
```

## PromptAdaptiveCard dialog

> This topic applies to v3 of the U4.Bot.Builder. 

In a lot of scenarios an Adaptive Card is used to present data and let the user choose what to do with that data ("Send for approval", "Save as draft", "Change", "Cancel" etc.)
The `PromptAdaptiveCard` dialog can be used for these cases to simplify recognition of user input and retries in case of invalid user responses. 
The dialog is based on Microsoft's PromptChoice dialog, but expects a configuration object of type PromptOptionsAdaptiveCard.

This example shows how to use the PromptAdaptiveCard dialog:
```csharp
private void DisplayCard(IDialogContext context, ResumeAfter<string> resumeMethod)
{
    var adaptiveCard = new AdaptiveCard
    {
        Body = new List<AdaptiveElement> { ... }
        FallbackText = " ... ",  // fall back text in case the rendering of the Adaptive Card failed.
        Actions = new List<AdaptiveAction> { ... }
    };

    var serviceUrl = "https://u4-da-dev-adaptive-cards.azurewebsites.net";
    PromptAdaptiveCard.Dialog(context, resumeMethod,
        new PromptOptionsAdaptiveCard(new AdaptiveCardSettings(adaptiveCard, "Hello World message", "This is my message to the world", serviceUrl)
        { ForceAdaptiveCardAsImage = true },
        attempts: 1, tooManyAttempts: "Please use the buttons next time!", retry: "I didn't get that."));
}
```
