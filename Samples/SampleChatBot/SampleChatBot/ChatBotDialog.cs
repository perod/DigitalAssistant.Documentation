using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using U4.Bot.Builder.Dialogs;
using U4.Bot.Builder.Extensions;
using U4.Bot.Builder.Interfaces;
using U4.Bot.Builder.Luis;

namespace SampleChatBot
{
    public enum DialogType { nice, rain, weather }

    [LuisApplicationName("SampleChat")]
    [Serializable]
    public class ChatBotDialog : LuisDialogBase<IMessageActivity>
    {
        #region Constructors
        public DialogType dialogType { get; set; }
        private readonly IWeatherService _weatherService;
        public ChatBotDialog(params ILuisService[] services)
            : base(services)
        {
            dialogType = DialogType.weather;
           
        }

        public ChatBotDialog(LuisServiceFactory luisServiceFactory, ILuisSubscriptionResolutionService luisSubscriptionResolutionService,
            ILuisServiceCreator luisServiceCreator,IWeatherService weatherService)
            : base(typeof(ChatBotDialog), luisServiceFactory, luisSubscriptionResolutionService, luisServiceCreator)
        {
            dialogType = DialogType.weather;
            _weatherService = weatherService;
        }
        #endregion
        #region ContextualIntents

        [LuisIntent("nice")]
        public async Task StartConversationAboutNiceWeather(IDialogContext context, LuisResult result)
        {
            dialogType = DialogType.nice;
            await StartConversationAboutWeather(context, result);
        }
        [LuisIntent("rain")]
        public async Task StartConversationAboutRain(IDialogContext context, LuisResult result)
        {
            dialogType = DialogType.rain;
            await StartConversationAboutWeather(context, result);
        }
        #endregion

        [LuisIntent("weather")]
        public async Task StartConversationAboutWeather(IDialogContext context,LuisResult result)
        {
 
            var entity = (result.Entities.Count == 0 ? null: result.Entities.First(x => x.Type.StartsWith("builtin.geography")));
            if (entity != null)
            {
                await context.PostAsync("Let me check for you");

                var forecast = await _weatherService.GetForecastFromService(entity.Entity);
                await AnswerBasedOnContext(context, forecast);
            }
            else
                PromptDialog.Text(context, FindWeatherForPlace, "Where are you?");
        }
        #region FallBackConversation
        [LuisIntent("")]
        [LuisIntent("None")]
        public Task None(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult luisResult)
        {
            return context.Complete("Don't know what you are talking about");
        }
        #endregion
        public async Task FindWeatherForPlace(IDialogContext context, IAwaitable<string> place)
        {
            string placeName = await place;
            var forecast = await _weatherService.GetForecastFromService(placeName);
            await AnswerBasedOnContext(context, forecast);
        }
        #region CentextualChat
        public async Task AnswerBasedOnContext(IDialogContext context,string forecast)
        {
            if (dialogType == DialogType.rain && forecast.Contains("rain"))
                await context.Complete("Yes, " + forecast);
            else if (dialogType == DialogType.rain && !forecast.Contains("rain"))
                await context.Complete("No, " + forecast);
            else if (dialogType == DialogType.nice && forecast.Contains("clear"))
                await context.Complete("Yes, " + forecast);
            else if (dialogType == DialogType.nice && !forecast.Contains("clear"))
                await context.Complete("No, " + forecast);
            else
                await context.Complete("It looks like it is " + forecast);
        }
        #endregion
    }
}
