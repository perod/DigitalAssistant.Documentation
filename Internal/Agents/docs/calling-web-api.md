# Calling a web API from a chatbot

## Chatbot architecture
As a chatbot developer you can decide your architecture and infrastructure, but we recommend that you keep your chatbots lean. The main concern of the chatbot should be to:

* handle incoming requests by using the components provided by the Unit4 Bot framework
* implement the conversational design for your bot skill

In most cases the chatbot will rely on web APIs, which we recommend are hosted on a separate API server. The chatbot will use HTTP to communicate with the API server.

## Service discovery

Your chatbot will in most cases rely on calling other APIs to execute business logic. If that service is considered to be a multitenant or global service, you can configure the URL using standard app settings in your chatbot.
However, if the service s considered to be tenant specific, then the service URL can be registered in the Unit4 People Platform Discovery Service. The chatbot will have access to the tenant ID for the incoming request via the `IConversationContext.Indentity`, and this can be used to resolve the service configuration for the API at runtime.

The **DiscoveryService.SDK** is available via the Unit4 People Platform dev NuGet: [http://packages.u4pp.com/nuget-dev/nuget](http://packages.u4pp.com/nuget-dev/nuget). To register your service please contact the Unit4 People Platform.

## <a href="authentication"></a>Authentication
All public web API servers must be secure which implies that they must rely on an authorization server to provide security. In the Unit4 ecosystem it's recommended that the [Unit4 Identity Services (U4IDS)](https://thehub.unit4.com/docs/identity-services/Latest/.%2Fdocs%2Findex.md) is used.

Any users connecting to Wanda must be authenticated. This authentication is an integral part of the Wanda ecosystem and is implemented centrally, with no chatbots being required to prompt the user for authentication. Wanda manages this authentication and is able to connect social media identities (Microsft Teams, Slack, Skype, Facebook etc.) to Unit4 corporate identities. This functionality relies on a central installation of U4IDS.

The access token obtained during authentication is passed on to the chatbots via the `IConversationContex.Identity`. Chatbots can exchange this token with a new access token that grants access to the web APIs used by the chatbot. The access token obtained by Wanda includes the **unit4_id** claim for the user. To retain this information in the new access token, chatbots must use the [User impersonation authentication flow](https://docs.u4pp.com/identity-services/flows/#user-impersonation-custom-flow). This flow is suitable for systems that need to call another API on behalf of the user. For more information on how to implement and register the chatbot with U4IDS see [Client registration](https://docs.u4pp.com/identity-services/register-client/#act-as-clients-custom). 

> If the target API does not require specific scopes, but only valid tokens from the IDS, the access token available on the `IConversationContext.Identity.AccessToken` can be used.

All chatbots and web APIs must rely on the same authority (authorization server). In a development scenario all chatbots and web APIs can rely on the [U4IDS sandbox](https://u4ids-sandbox.u4pp.com/api/swagger/ui/index). 

The authority (URL) used to obtain the access token in Wanda is available via the `IConversationContext.Identity.Authority` property.

To simplify the process to obtain tokens on behalf of the users from an IDS, the Unit4 Bot Framework provides an interface and implementation that can be used. The `IIdsTokenClient` implements the user impersonation custom flow via the `RequestUserImpersonationGrantAsync(..)` method. The `IIdsTokenClient` is registered in the central IOC and can be injected in the class that needs to obtain a token to call an Web API.

## <a name="httpclient"></a>  HttpClient

> This topic applies to v3 of the U4.Bot.Builder. 

A good practice in any application is to reuse the `HttpClient` instance, this is due to overhead with creating the instance. To handle this the Unit4 Bot Framework provides a central HttpClient factory, that can be injected into any class that will use a `HttpClient` to communicate with a service.
To use the central factory inject the `IHttpClientFactory` in the class that will use HTTP to call an API.

You can store the HttpInstance instance in a static variable in your class: 

`private static Lazy<HttpClient> _client;`

And in the constructor set it up:

`Interlocked.CompareExchange(ref _client, new Lazy<HttpClient>(httpClientFactory.Create), null);`

To prepare your class for unit tests it's recommended to have an additional constructor parameter `useSharedHttpClient`, that can be set to `false` in a unit test.

```csharp
if (useSharedHttpClient)
{
    Interlocked.CompareExchange(ref _client, new Lazy<HttpClient>(httpClientFactory.Create), null);
}
else
{
    _client = new Lazy<HttpClient>(httpClientFactory.Create);
}
```

To use the `HttpClient` in your code access it via the `Value` property of your lazy class member.

```csharp
using (HttpResponseMessage response = await _client.Value.SendAsync(request).ConfigureAwait(false))
{
    ...
}
```

## Complete example 

> Coming soon




