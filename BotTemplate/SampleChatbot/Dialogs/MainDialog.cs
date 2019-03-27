using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SampleChatbot.Dialogs.Children;
using SampleChatbot.Models;
using System;
using System.Threading.Tasks;
using U4.Bot.Builder.Extensions;

namespace SampleChatbot.Dialogs
{
    [Serializable]
    public class MainDialog : IDialog<IMessageActivity>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceived);
            return Task.CompletedTask;
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var msg = await result;
            await context.PostAsync("Hello, you have now reached the SampleChatbot.").ConfigureAwait(false);
            var childDialog = context.Resolve<ChildDialog>();
            context.Call(childDialog, ResumeAferChildDialogDone);
        }

        private async Task ResumeAferChildDialogDone(IDialogContext context, IAwaitable<ChildDialogModel> result)
        {
            var childDialogModel = await result;
            if (childDialogModel.Success)
            {
                await context.Complete(childDialogModel.Message).ConfigureAwait(false);
            }
            else 
            {
                await context.Complete().ConfigureAwait(false);
            }
        }
    }
}
