
# Logging

> This topic applies to v3 of the U4.Bot.Builder. 

To understand how your chatbot behaves and to identify failure situations, it's good practice to let your chatbot write log entries.
The U4.Bot.Builder framework provides a couple of ways to write log entries in your chatbot. 

## IDialogContext
To write log entries from dialog classes, you can use the log extension methods on `IDialogContext`. The extension methods are available though the `U4.Bot.Builder.Log.Extensions` namespace.

## IBotLogger
If you want to write log entries from a custom class that's used by your chatbot, you can inject a `IBotLogger` instance in your class constructor.

## Writing log entries

The log system in chatbots supports these levels:

* Debug
* Information
* Warning
* Error
* Fatal

You should use the levels in accordance with how important the log entries are. To get insights into how your chatbot operates, the **Information** level is appropriate. **Warning, Error, Fatal** levels are used in failure situations.

The log methods on `IBotLogger` provide overloads where you can pass in `IConversationContext` or `ConversationIdentity`. Using these methods, the log system will add identifiers to log entries so they are easier to track.
This includes tenant ID, a unique identifier for the identity, and an identifier for the chatbot.

The proposed way to write the log entries is to use message templates. You will find some examples [in this article](https://github.com/serilog/serilog/wiki/Writing-Log-Events). 

## Log sink

When a chatbot is deployed into the Wanda ecosystem the log entries are written to a central log sink. This sink will be managed by the organization hosting the Digital Assistant.

When running the chatbot in the emulator, log entries will be written to the console.


