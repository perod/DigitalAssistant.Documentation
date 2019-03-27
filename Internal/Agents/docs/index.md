# Introduction

## About this site

> Some of the topics found in this documentation applies to SDK v3 release of U4.Bot.Builder, this version is no longer supported. Version v4 of the Unit4 Bot Framework is in development, and is currently avaliable in prerelease.
> This version supports the the latest version of the Microsoft Bot Framework SDK for .NET (v4). Any new development should use v4 of the Unit4 Bot Framework SDK for .NET.

### Audience
This site is aimed at:

* Unit4 developers tasked with developing a Wanda chatbot for the Wanda chatbot ecosystem 
* Unit4 Cloud Operations staff tasked with configuring, running, maintaining and updatiing the Wanda service and required Unit4 People Platform services for customers running Wanda in their Business World installation

### Information provided
Information is provided on Wanda, chatbots, dialogs, LUIS etc. and how to:

* Build chatbots using the Unit4 Bot Framework
* Register and integrate bot skills into the Wanda ecosystem
* Perform day-to-day operational tasks such as monitoring, maintenance and updating the Wanda service  

## To get started
To get started and learn how to build chatbots using the Unit4 Bot Builder, see the [chatbot tutorial](chatbot-tutorial.md).

To get an introduction to chatbot concepts and processes, see the [chatbot developer introduction](dev-introduction.md).

## Wanda Digital Assistant

Wanda is the public facing Unit4 Digital Assistant which Unit4's users will communicate with using chat messages. Wanda consists of multiple bot skills that are implemented as chatbots, with the chatbots being deployed into the Wanda ecosystem.
The chatbots are built using the **Unit4 Bot Builder** SDK for .NET.

Wanda is registered with the Microsoft Bot Framework and receives messages from users originating from various social network channels, such as Microsoft Teams, Skype and Slack. When Wanda receives a message from a user, she tries to resolve the intent and route it to a chatbot that can handle the user's request. In this process she uses a specialized [LUIS](luis.md) application to identify and classify the intent.

From the intent, Wanda is able to resolve the appropriate chatbot (skill) that can handle the user's message. When the **main** intent has been classified, a conversation is established between the user and the specific chatbot, and  any subsequent messages from the user to Wanda are sent to that chatbot. Wanda manages the active conversations a user has with the various chatbots in the ecosystem. 

Wanda is also responsible for authenticating and maintaining the connection/mapping between user identities in the supported channels and the corporate accounts. Wanda is true multitenant and allows users to sign on with their corporate accounts on their tenant's Unit4 Identity Services (U4IDS). For more details about user authentication, identity and tenants see [User identity](user-identity.md).

## Wanda ecosystem

The Wanda ecosystem is a set of services that works together to provide the conversational AI experience. 
The main parts of Wanda are the core digital assistant and the chatbots. The core digital assistant is responsible for receiving messages from users through the Microsoft Bot Framework, managing user identities and the conversations, and directing traffic to the available chatbots.
The chatbots provide the various skills, such as the ability to register absences, timesheets, expenses, as well as submit simple purchase requisitions etc.

The core digital assistant is a Web API that's registered with the Microsoft Bot Framework. Any messages from users on the supported social media channels will go to the digital assistant API.
This API relays messages to the chatbots using a message-based system. The chatbots are long running WebJobs that listen to messages on the message bus infrastructure.

The Wanda ecosystem relies on these other Unit4 People Platform services:

* [Unit4 Indentity Services](https://thehub.unit4.com/docs/identity-services/Latest/.%2Fdocs%2Findex.md)
* [Unit4 Discovery Service](https://thehub.unit4.com/docs/discovery-service/Latest/docs%2Findex.md)
* [Unit4 Identity Mapper](https://thehub.unit4.com/docs/identity-mapper/Latest/docs%2Findex.md)

The chatbots rely on other business APIs to drive the conversational UI.






