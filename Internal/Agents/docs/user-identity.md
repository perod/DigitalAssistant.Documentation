# User identity

The Wanda ecosystem supports chatbots that require authentication, and chatbots that run in anonymous mode. If your chatbot requires authentication (establish a user identity), this will be managed by the core Wanda bot, you do not have to do anything specific in the chatbot.
A prerequisite for customers running Wanda is that the user can authenticate via the Unit4 Identity Services (U4IDS). 

A user's tenant identifier and unique user identifier is available through the [conversation context](conversation-context) in a chatbot.

For details on how to call a Web API from a chatbot, see [Calling a web API from a chatbot](calling-web-api#authentication).

## Authentication

The user authentication process implemented in Wanda relies on these Unit4 cloud services:

* [Unit4 Discovery Services](https://thehub.unit4.com/docs/discovery-service/Latest/docs%2Findex.md)
* [Unit4 Identity Services](https://thehub.unit4.com/docs/identity-services/Latest/.%2Fdocs%2Findex.md)
* [Unit4 Identity Mapper](https://thehub.unit4.com/docs/identity-mapper/Latest/docs%2Findex.md)

## Unit4 Discovery Services

Whenever a customer is adopted into the Wanda ecosystem, the customer is assigned a unique identifier (tenant ID). 
The tenant is then made discoverable by additional configuration in the [Unit4 Discovery Service](https://thehub.unit4.com/docs/discovery-service/Latest/docs%2Findex.md) (U4DS). The login dialog implemented in core Wanda, uses the U4DS to find the user's company and from that the Unit4 Identity Services instance (URL) where users can authenticate.
 
## Unit4 Identity Services

The [Unit4 Identity Services](https://thehub.unit4.com/docs/identity-services/Latest/.%2Fdocs%2Findex.md) acts as the gateway where users can authenticate with Wanda. 
Wanda implements the logic to exchange access tokens for a user. These tokens are available through the `IConversationContext` in the chatbot service.

## Unit4 Identity Mapper

After a successful authentication, the mapping between the user's social account and enterprise account is maintained in the [Unit4 Identity Mapper](https://thehub.unit4.com/docs/identity-mapper/Latest/docs%2Findex.md).

## Identity

A user within the Wanda ecosystem has a unique identifier that is based on the tenant's IDS setup. A user is unique via the **tenant** and **unit4_id** claims, and these claims are provided and managed by the U4IDS.




