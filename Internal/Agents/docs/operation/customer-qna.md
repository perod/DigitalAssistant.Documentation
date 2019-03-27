
# Customer QnA

Customers adopted into the Wanda ecosystem have the ability to connect their own QnA Maker service into the system. This gives them the ability to use Wanda to field questions from users in their organization.
To run a tenant's QnA service in the Wanda pipeline, the customer must provide the following:

- QnA host url
- QnA endpoint Key
- QnA Knowledgebase ID

These values have to be uploaded as secrets into the core KeyVault in the AssistantService resource group.
The following patterns must be used:

- **tenant-id**-company-qna-host-url
- **tenant-id**-company-qna-endpoint-key
- **tenant-id**-company-qna-knowledgebase-key

A script to to upload the required settings to KeyVault is found [here](https://unit4-peopleplatform.visualstudio.com/People%20Platform/_git/DigitalAssistant.DevOps?path=%2FSetup%2FCustomerQnA%2FCreateKeyVaultEntriesForQAbot.ps1&version=GBmaster).






