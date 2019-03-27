using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using U4.Bot.Builder.Interfaces;
using U4.Bot.Builder;
using Autofac;

namespace SampleChatBot
{
    public class ChatBotAgent : IAgent
    {
        protected virtual IContainer Container => IoCConfig.Container;
        public Task HealthCheckReceived()
        {
            throw new NotImplementedException();
        }

        [MessageService]
        public Task Message(IMessageActivity message, IConversationContext conversationContext, CancellationToken token = default(CancellationToken))
        {
            return Conversation.SendAsync<ChatBotDialog>(message, conversationContext, Container, null, token);
        }
    }

}
