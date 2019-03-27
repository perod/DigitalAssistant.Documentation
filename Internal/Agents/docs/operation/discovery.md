
# Discovery

The Unit4 Discovery service maintains a list of known tenants and their connected source systems in the ecosystem.

All customer/tenants in the ecosystem must be registered in the global Unit4 Discovery service. 

## Required source systems
Wanda requires the following source systems to be defined:

- u4da
- u4ids
- u4bw

## Tenant registration
The tenant must be registered, assigned the unique identifier and named.

## IDS Source System
The discovery service should register all available instance of IDS as separate source systems. This should be done as a preliminary step setting up the discovery service.
This source system must contain service type **authority** pointing to the IDS base url.

```
{
  "id": "",
  "name": "Unit4 Identity Services - EU 1",
  "services": [
    {
      "capabilities": [ ],
      "sourceSystemId": "",
      "name": "?",
      "type": "openid-connect-configuration",
      "url": "https://s-eu-ids1.unit4cloud.com/identity/.well-known/openid-configuration"
    },
    {
      "capabilities": [ "authentication" ],
      "sourceSystemId": "",
      "name": "Unit4 Identity Services Authority",
      "type": "authority",
      "url": "https://s-eu-ids1.unit4cloud.com/identity"
    }
  ],
  "type": "u4ids"
}
```

## U4DA source system
The discovery service must configure at least one instance of the **u4da** source system.
The source system needs one service **webapi** pointing to the Assistant service url.

## U4BW source system
The **u4bw** source system is tenant-specific amd must contain the following service descriptions:

- webapi
- soap
- webapp

pointing to urls of the services of a specific u4bw instance.

## Connect tenant to source systems
When the source system has been created, it must be connected to the tenant. This involves assigning the correct geopolitical IDS instance, the created u4bw source system and the u4da source system.
Connecting the u4da source system involves configuring the relevant chatbot skills that are available for that tenant. This is done via the dedicated **chatbots** property in the connection between the general **u4da** source system and the tenant.

Example: **chatbots=U4.Expenses.Agent,U4.Tasks.Agent,U4.Absence.Agent,U4.Help.Agent**

The available chatbot skills identifiers are:

- U4.TravelRequest.Agent
- U4.Expenses.Agent
- U4BW.Timesheet
- U4.Purchasing.Agent
- U4.Balance.Agent
- U4.Payslip.Agent
- U4.Tasks.Agent
- U4.Absence.Agent

The customer will choose which skill(s) they will use and this must be defined in the discovery configuration.

## Automated process

PowerShell script [DiscoRegisterTenant](https://unit4-peopleplatform.visualstudio.com/People%20Platform/_git/DigitalAssistant.DevOps?path=%2FSetup%2FDisco%2FDiscoRegisterTenant.ps1&version=GBmaster) can be used to setup a known customer/tenant in the Unit4 Discovery Service. It relies upon the existing *Unit4 Discovery Service PowerShell module*. [Install](https://thehub.unit4.com/docs/discovery-service/Latest/docs:powershell_installation.md) this module before running the script. The script will register both the tenant and the new **u4bw** source system. It will also bind the new **u4bw** source system and the existing **u4ids** and **u4da** source systems to the  tenant registration.

### Example of usage
Create a JSON file containing an array of tenants to register, such as:

	[
        {
            "TenantId": "e2f4f189-e545-4f28-b6c2-07092cf76553",
            "TenantName": "tenant-name",
            "TenantDisplayName": "Tenant Display Name",
            "U4BwSoapUrl": "https://someurl.com/BusinessWorld-webservices/service.svc",
            "U4BwWebApiUrl": "https://someurl.com/BusinessWorld-web-api",
            "U4BwWebUrl": "https://someurl.com/BusinessWorld/",
            "U4IdsSourceSystemId": "uniqueId",
            "U4DaSourceSystemId": "uniqueId",
            "ChatbotSkills": [ "Expense", "Timesheet", "Travel", "Purchase", "Task", "Balance", "Payslip" ]
        }
    ]

Connect to the discovery service, and pipe the JSON file to the script:

	$json = ".\Tenants.json"
	Connect-U4DS -NoAuthentication

	#Pipe the JSON to the script
	(ConvertFrom-Json (Get-Content -Path $json -Raw)).Tenants | .\DiscoRegisterTenant.ps1 -Verbose

	Disconnect-U4DS

#### Script input parameters

* **TenantId** - The id of a known Unit4 customer/tenant.

* **TenantName** - The name of the tenant expressed in lowercase with hyphens. The *id* of the **u4bw** source system created by the script will be defined as "u4bw-\[TenantName\]". It is therefore important that the tenant name is unique. To be consistent with existing tenants it should also be registered in lowercase using hyphens. Examples: 'upride-norway-gde' or 'upride-belgium-gde'.

* **TenantDisplayName** - The TenantDisplayName should be a short display friendly description of the tenant. The *name* of the **u4bw** source system created by the script will be defined as "Unit 4 Business World - \[TenantDisplayName\]".

* **U4BwXXUrl** - URLs to the different Unit4 services required by Wanda. Each URL will be registered as a service in the new **u4bw** source system created by the script.

* **U4IdsSourceSystemId** - The *id* of an existing **u4ids** source system already registered within the connected Unit4 Discovery Service.

* **U4DaSourceSystemId** - The *id* of an existing **u4da** source system already registered within the connected Unit4 Discovery Service.

* **ChatbotSkills** - Defines which chatbots that are supported by the tenant. Internally, the script will convert the values expressed here with the corresponding chatbot names required by the binding between the tenant and the **u4da** source system (*Expenses* to *U4.Expenses.Agent* etc).