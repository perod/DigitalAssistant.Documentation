using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U4.Bot.Builder;

namespace SampleChatBot
{
    public static class IoCConfig
    {
        public static IContainer Container { get; }

        static IoCConfig()
        {
            ContainerBuilder builder = GetContainerBuilder();

            Container = builder.Build();
        }

        public static ContainerBuilder GetContainerBuilder()
        {
            ContainerBuilder builder = Conversation.GetContainerBuilder();

            builder.RegisterType<ChatBotDialog>();
            builder.RegisterType<WeatherService>().As<IWeatherService>();

            return builder;
        }
}
}
