
# Customer adoption

Any customer running Unit4 Business World has the ability to use Wanda. The prerequisites and the process to adopt customers into Wanda are explained in this guide.

## Prerequisites

* Customers must use [Unit4 Indentity Services (U4IDS)](https://thehub.unit4.com/docs/identity-services/Latest/.%2Fdocs%2Findex.md) as this is required by Wanda for authentication.
* Wanda requires Unit4 Business World Milestone 6 Update 3 or later plus the latest Experience packs.
* The Unit4 Business World Public Web API and SOAP services must be exposed to the internet and secured with a cloud U4IDS instance.
* The customer must be registerd as tenant in the [Unit4 Discovery Service](https://thehub.unit4.com/docs/discovery-service/Latest/docs%2Findex.md). The customer must be assigned a unique tenant ID (GUID), this ID is normally obtained when the customer is registered in the Unit4 Identity Services.

## Process to integrate customer into Wanda ecosystem

### Ordering

Ordering and registration of the Unit4 Wanda digital assistant with the Unit4 Cloud for the customer's Business World installation. Ordering of Wanda and the selected digital assistant skills is done by the customer via their Unit account manager, and Wanda registration and setup in the Unit Cloud is done by Unit4 Cloud Operations.

### Setup steps

1. Register customer as tenant in Unit4 Identity Services.
3. Register customer as tenant in Unit4 Discovery Service.
3. Configure Unit4 Business World to be ready for Wanda.

#### Unit4 Discovery Service configuration
The required configuration in the Unit4 Discovery Service consists of the following source system:

* **U4IDS** - pointing to the IDS instance setup for the tenant
* **Digital Assistant (U4DA)** - describing what chatbot skills the tenant will use.
* **U4BW**- service urls for U4BW Web Application, SOAP services and Public REST API

For further details on this step consult this [guide](discovery.md).

#### Unit4 Business World

Additional configuration is required in the Unit4 Business World installation to make it work with Wanda. 
This is explained in more detail in the [Wanda implementation guide](https://wanda-implementation-guide.u4pp.com/).

#### Available skills

As of October 2018:

* U4BW Timesheet
* U4bW Purchasing
* U4BW Balances
* U4BW Payslip
* U4BW Travel Request
* U4BW Expenses
* U4BW Tasks
* U4BW Absence Request

To learn more about the available skills, see this [overview](https://wanda-implementation-guide.u4pp.com/digitalassistants/).

>The following guides explain in detail how to configure the required settings.






