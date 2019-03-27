
# Identity Services

The Unit4 Identity Services is used to secure all communications over HTTPS in the Wanda ecosystem. As explained in the architectural guides, the environment depends of two variants of the IDS.
The communication between the global services in the infrastructure is secured by a dedicated instance of the IDS. This IDS is only used for machine to machine communication, meaning that no user is involved.
Examples of this is the communication between the Assistant Service and the Unit4 Identity Mapper, and between the Digital Assistant Push Service and the Assistant Service (notifications).

The second variant is refered to as the regional IDS. In the Unit4 cloud environment multiple instances will exist, most likely mapped to geopolitical regions.
These instances are where users authenticate, through their tenant's Identity Provider.

## Unit4 People Platform IDS

The global IDS instance is used to secure the communication between global services, or regional service communication with a global service.

### Unit4 Identity Mapper

The assistant service communicates with the Identity Mapper over HTTPS. This communication uses the global IDS to obtain a token using the client credentials flow.
This communication requiers the following:

1. A credentials client in the IDS, with a corresponding resource scope.
2. The Assistant service must be configured (app setting) - the IDS authority, client ID, client secret, and required scope.
3. The Unit4 Identity Mapper must be configured with the same IDS authority and required scope. This will be used when validating the incoming token in the 's authentication middleware.

> These setting will be read from parameter files during deployment. The secret value is read from keyvault during deployment, and should never be visible to developers.

### Digital Assistant Push service

This service is a bridge between a regional infrastucture and the global Wanda system. The push service must be registered as a client in the global IDS instance.

1. Register resource scope **u4da**, 
2. Register client **u4da-push** as a client using client credentials, connect **u4da** scope
3. Obtain client secret
4. Configure the push service with required IDS settings

The AssistantService must be configured with the global IDS as authority and have **u4da** defined as the required scope to call notifications endpoint.

> The client secret for the **u4da-push** client can be shared across all instances of the service. Alternatively, you can use one client secret per service instance.

## Regional IDS

The use of regional IDS is essential in the ecosystem and affects how the ecosystem is constructed, since the ecosystem supports multiple IDS instances.
This must be reflected throughout all the services. Keep in mind that the ecosystem is global and does not correspond to the regional (geopolitical) existence of the IDS.

### Clients and Scopes
With knowledge of the ecosystem architecture, we can map the services to clients in the regional IDS.

Whenever a user authenticates with Wanda he/she does so through the regional IDS using a dedicated client.
This is the **u4da** client. This client uses **hybrid** authentication flow, this means that there will be a client secret involved.
Since the ecosystem will need to support all production IDS instances, the prerequisites are that the client is registered in all IDS instances and that the client secret is configured in the ecosystem.
For Wanda, this secret is stored in a dedicated KeyVault instance in the central resource group.

This pattern is then repeated for most of the required IDS clients required by the ecosystem. The communication between chatbots and domain APIs relies on the user impersonation custom flow. This flow also involves the usage of secrets obtained from the IDS instance.
These secrets are also stored in the central Wanda KeyVault, and is read at runtime by the services (chatbots and assistant service).

Domain APIs do not read secrets from KeyVault at runtime, but are deployed with all secrets in the application setting of the service. The secrets themselves are read from a specific KeyVault during deployment, and are not visible to developers.

The other aspect of IDS configuration is the use of scopes. The **u4da** client is defined with a set of scopes, that are used to request consent from user (IDS consent screen) and the use of refresh tokens.
Other clients will request specific resource scopes, that are used as validation in downstream API calls. For instance the expenses chatbot will need to obtain a token that includes a specific resource scope. The travel and expenses API will perform standard token validation and will also require that scope to be included in the token.

All domain APIs are registered as user impersonation clients with the standard **u4bw** scope. The domain APIs will obtain tokens and use them to call the U4BW ERP source system public API.

> All IDS clients must be configured to use **Jwt** token type. 

The use of Jwt tokens has two aspects. Domain APIs accept requests with bearer tokens from multiple issuers. In order to validate tokens properly, the service must be able to read the *issuer* claim from the token, then use the issuer in the token validation process.
If the bearer token was of type **reference** this would not be possible, as the api would need to go to the IDS to unpack the token via introspection.
For the Domain APIs the reasoning is different, if the system is to use **reference** token, a set of scope secret must be maintained in the regional IDS and be reflected in all U4BW installations. Either you have to share the scope secret, or you have to have a scope secret for each U4BW on that IDS instance.

### Process

A Powershell script that creates all required client and scopes in a regional IDS is provided. The outcome of this will be a set of client secrets that is either uploaded to the Wanda KeyVault of the KeyVault used for deployment to the environment.
A initial version of a script is included in the DigitalAssistant.DevOps git repository.

The scripts are found here: https://unit4-peopleplatform.visualstudio.com/People%20Platform/People%20Platform%20Team/_git/DigitalAssistant.DevOps?path=%2FSetup%2FIDS&version=GBmaster

### Client secrets and KeyVault

All client secrets for regional IDS are read from a KeyVault at runtime. The architecture guide describes the required setup for the system to be able to read secrets from keyvault.
The secrets obtained from regional IDSs must be uploaded to KeyVault, and given a name according to a set pattern.

The name must follow this pattern: 

**secretname-url token encoded issuer/IDS authority**

The secret name will in most cases refer to the client id name assigned to a service in IDS. Example: **u4-expenses-chatbot-aHR0cHM6Ly91NGlkcy1zYW5kYm94LnU0cHAuY29tL2lkZW50aXR50**, where the last part is a url token encoded value for *https://u4ids-sandbox.u4pp.com/identity*.

> No automatic powershell is currently provided for this step. Secrets must be uploaded manually.

### Domain APIs

All domain APIs are also configurable to operate on multiple IDS client secrets. But for those API, the secrets are set in the app settings section of the web application resource.
They are read from a central KeyVault (not to be confused with Digital Assistant KeyVault) during deployment. The domain APIs are required to be configured with the configured issuers as well.

The following app settings are used:

 - ids:authorities
 - ids:domain-api-client-id
 - ids:domain-api-client-secrets

> The authorities and secret settings are comma-separated values, where the position of the authority has to match the secret.

In the release pipeline the lists of authorities should be specified in a variable group, and used across deployment of the APIs.

> The Unit4 DIMPS service must be configured with the same authorities as the standard domain/feature APIs.





 





 








