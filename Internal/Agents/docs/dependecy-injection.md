
# Dependency injection

> This topic applies to v3 of the U4.Bot.Builder. 

When implementing a chatbot the proposed pattern is to have a clear separation of what is considered to be conversational code and what is considered to be business logic.
Business logic could be calling Web APIs, implementing certain business rules, or other types of logic that drives the chatbot.

To have the ability to write a proper test harness for your chatbot, you should use dependency injection. If you are not familiar with the dependency injection concept, there are many sources on the internet that explain the pattern.
We propose using constructor injection, rather then service discovery pattern. 

## Autofac

The U4.Bot.Builder framework is based on the Microsoft Bot Builder framework. At its core the Microsoft Bot Builder is using [Autofac](http://autofac.readthedocs.io/en/latest/index.html). The U4.Bot.Builder provides a bridge so that you can register your services in the central Autofac IOC container.
Understanding Autofac is not trivial, but we will walk you through the steps to understand how the IOC is working in a chatbot.

The IOC Container is sent in as a parameter to the `U4.Bot.Builder.Conversation.SendAsync<Dialog>(...)`. The proposed pattern is to isolate the construction of you rchatbot IOC Container.

``` csharp
public class Chatbot : IAgent
{
    protected virtual IContainer Container => IocConfiguration.Container;

    [MessageService]
    public Task Message(IMessageActivity message, IConversationContext conversationContext, CancellationToken cancellationToken)
    {
        return Conversation.SendAsync<Dialog>(message, conversationContext, Container, null, cancellationToken);
    }
}
```

In this example the `IocConfiguration.Container` is pointing at a static class, that will build the IOC container. 
The crucial part is to get hold of the core container before any custom registrations are added -- see `Conversation.GetContainerBuilder()` call. 

``` csharp
public static class IocConfiguration
{
    public static IContainer Container { get; }

    static IocConfiguration()
    {
        ContainerBuilder builder = GetContainerBuilder();
        Container = builder.Build();
    }

    public static ContainerBuilder GetContainerBuilder()
    {
        //Initiate a new builder with services registered in U4.Bot.Builder
        //and in the core Microsoft Bot Framework.
        var builder = Conversation.GetContainerBuilder();

        //Register components and services used by the SampleChatbot.
        builder.RegisterModule<DialogsModule>();
        builder.RegisterModule<ServicesModule>();

        return builder;
    }
}
```

The example above uses Autofac modules to group service registrations.

Due to the nature of Dialog classes you need special handling of dependencies injected into your dialogs. 
Dialogs are serializable objects, so you cannot store the resolved instances as normal properties on your dialog class.
To bypass this restriction, you can mark the backing fields for the injected instance as `[NonSerialized]`.

``` csharp
[Serializable]
public class Dialog : IDialog<IMessageActivity>
{
    [NonSerialized]
    private IBusinessLogicService _businessLogicService;
    public IBusinessLogicService BusinessLogicService
    {
        get { return _businessLogicService; }
        set { _businessLogicService = value; }
    }

    public Dialog(IBusinessLogicService businessLogicService)
    {
        BusinessLogicService = businessLogicService;
    }
}
```











 
