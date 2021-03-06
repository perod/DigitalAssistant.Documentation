﻿using Autofac;
using Microsoft.Bot.Connector;
using SampleChatbot.Dialogs;
using SampleChatbot.Ioc;
using SampleChatbot.Services.Health;
using System.Threading;
using System.Threading.Tasks;
using U4.Bot.Builder;
using U4.Bot.Builder.Interfaces;

namespace SampleChatbot
{
    public class Agent : IAgent
    {
        protected virtual IContainer Container => IocConfiguration.Container;

        [MessageService]
        public Task Message(IMessageActivity message, IConversationContext conversationContext, CancellationToken cancellationToken)
        {
            return Conversation.SendAsync<MainDialog>(message, conversationContext, Container, null, cancellationToken);
        }

        [MessageServiceMethod(MessageMethods.HealthCheck)]
        public async Task HealthCheckReceived()
        {
            var healthChecker = Container.Resolve<IChatbotHealthChecker>();
            var healthResult = await healthChecker.CheckHealth().ConfigureAwait(false);

            var healthCheck = Container.Resolve<IHealthCheck>();
            await healthCheck.SendHealthStatus(Constants.ChatbotId, healthResult.Status.ToString(), healthResult.Message).ConfigureAwait(false);
        }

        public static void Start(CancellationToken cancellationToken)
        {
            MessageService.SetupAsync<Agent>(cancellationToken).Wait();
        }
    }
}
