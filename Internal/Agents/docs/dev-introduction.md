
# Overview

## What is a chatbot?

A chatbot is an application that provides a specific bot skill in the Wanda ecosystem and which users interact with in a conversational way using text, graphics (cards), or speech. It could be a simple question and answer dialog, or a more sophisticated chatbot that allows users to interact with services in a more intelligent way using pattern matching, state tracking and artificial intelligence techniques that are well integrated with existing business services.

## Unit4 Bot Framework

Chatbots within the Wanda ecosystem are developed using the Unit4 Bot Framework which provides:

* Tools to build and test your chatbot
* Guidelines on how to build your chatbot and manage your language models

## Initial planning

Before writing code, it's recommended that you design an outline of the conversation -- see the [conversation guidelines](writing-guidelines.md) for details. You should also be familiar with the Microsoft Language Intelligent Service [LUIS](luis.md).
You can plan your conversational design using a whiteboard, storyboards or more sophisticated tools. Several chatbot conversation prototyping tools are available, but a tool like Visio can be used to map out the basic flow of the conversation. During planning you should identify if your chatbot should support [proactive conversations](proactive-conversations.md).

## Building a chatbot

To build a chatbot you will use the Unit4 Bot Builder SDK. The Unit4 Bot Builder is for .NET and leverages C# to provide a familiar way for .NET developers to create chatbots using Visual Studio.

The Unit4 Bot Builder is built on top of Microsoft Bot Builder SDK for .NET (v3).

Your chatbot is a service that runs inside the Wanda ecosystem. The chatbot implements the conversational interface that communicates with the Microsoft Bot Connector and services within the Unit4 ecosystem.

A [Visual Studio template](chatbot-template.md) has been made available to help you get started.


## Testing your chatbot

Chatbots are complex applications that include a conversational layer backed by a set of services. It is good and recommended practice to build a test harness for your chatbot. The Unit4 Bot Builder provides guidelines and tools for testing your bot.

* [Test chatbot with the emulator](emulator.md)
* [Writing tests](writing-tests.md)

## Publishing your chatbot to the Wanda ecosystem

A chatbot must be registered and deployed to the Wanda ecosystem to be made available for users. The deployment process consists of several steps and these are highlighted in the [Chatbot deployment overview](deploy-chatbot.md). 





