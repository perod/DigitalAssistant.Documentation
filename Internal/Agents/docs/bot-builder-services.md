
# U4.Bot.Builder services

The U4.Bot.Builder NuGet package comes with a set of utility services that can be used in a chatbot. The services are registered in the central IOC container, and can be injected into any class that is registered in the same container. The proposed pattern is to use constructor injection.

The following utility services are registered in the container:

* IBotLogger
* IIdsTokenClient
* ILuisRequestService
* IAffirmationService
* IHttpClientFactory
* ISecretService
* IAdaptiveCardServiceClient

## IBotLogger

This service can be used to omit log events to the central logging service in the Wanda ecosystem. See [Logging topic](logging.md) for more details.

## IIdsTokenClient

This service can be used to obtain access tokens from the Unit4 Identity Services (U4IDS) for a user. It facilitates performing user impersonation custom authorization flow.

## ILuisRequestService

Service to perform LUIS requests. See [LUIS guide](luis.md) for more details on LUIS in general.

## <a name="affirmationservice"></a> IAffirmationService

The affirmation service can be used to classify if a statement from a user is positive or negative. In most cases you would use the Affirmation dialogs.
See [affirmation](affirmation.md) for more details on the affirmation concept.

## IHttpClientFactory

It's good practice in any application to reuse the `HttpClient` instance due to overhead with creating the instance. To handle this the Unit4 Bot Framework provides a central HttpClient factory, that can be injected into any class that will use a `HttpClient` to communicate with a service.
See [this example](calling-web-api.md#httpclient) for details on how to use the factory.

## ISecretService

The `ISecretService` provides another way to handle secrets in your chatbot. If you don't want to have secrets and keys in your application config, then this service can be used. The secret service can be used to obtain secrets used in your chatbot and this could be API keys, IDS secrets etc. The implementation of the service is backed by a KeyVault hosted in the Wanda ecosystem. So to use this service from you chatbot, you first need to upload the secrets to that KeyVault.
Please contact Unit4 People Platform team for assistance. 

## IAdaptiveCardServiceClient

Microsoft presented the AdaptiveCard concept during Build 2017 as a unified way to present data to the user on various communication channels. Sadly, even today (June 2018) almost no channels (not even Microsofts own) are able to natively display AdaptiveCards.
The only exception being the BotFramework emulator and Skypes web client. But native AdaptiveCard support will come "soon" to Skype apps and Microsoft Teams.
As a fallback until then (and for channels that do/will not support AdaptiveCards) AdaptiveCards can be rendered to images and sent as attachments to the user.

The IAdaptiveCardServiceClient provides an interface to a Unit4 web service that renders AdaptiveCards with predefined dimensions depending on the target channel and returns the image.
Those dimensions are different for each channel and make sure the AdaptiveCard image is displayed properly.

See [Adaptive Cards](adaptive-cards.md) for more detailed information about Adaptive Cards and how to present them to the user.
