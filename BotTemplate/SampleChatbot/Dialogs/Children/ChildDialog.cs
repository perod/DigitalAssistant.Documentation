using Microsoft.Bot.Builder.Dialogs;
using SampleChatbot.Models;
using SampleChatbot.Services.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U4.Bot.Builder.Extensions;

namespace SampleChatbot.Dialogs.Children
{
    [Serializable]
    public class ChildDialog : IDialog<ChildDialogModel>
    {
        private bool _isChild;

        [NonSerialized]
        private IBusinessLogicService _businessLogicService;
        public IBusinessLogicService BusinessLogicService
        {
            get { return _businessLogicService; }
            set { _businessLogicService = value; }
        }
        public ChildDialog(IBusinessLogicService businessLogicService, bool isChild = true)
        {
            _businessLogicService = businessLogicService;
            _isChild = isChild;
        }

        public virtual Task StartAsync(IDialogContext context)
        {
            PromptForNumber(context);
            return Task.CompletedTask;
        }

        private void PromptForNumber(IDialogContext context)
        {
            var promptDialog = new PromptDialog.PromptInt64(
                promptOptions: new PromptOptions<long>(
                    prompt: "Please pick a number between one and ten (hint: UnitX).",
                    retry: "Sorry, I didn't get that. Please enter a number between one and ten.",
                    tooManyAttempts: "Too many attempts. I'm ending this conversation.",
                    promptStyler: new PromptStyler(PromptStyle.None)
                ),
            min: 1,
            max: 10);

            context.Call(promptDialog, ResumeAfterNumberEntered);
        }

        private async Task ResumeAfterNumberEntered(IDialogContext context, IAwaitable<long> result)
        {
            try
            {
                var number = await result;
                var isLucky = BusinessLogicService.IsLuckyNumber((int)number);

                if (isLucky)
                {
                    await Done(context, "Congratulations. You picked the correct number.");
                }
                else
                {
                    PromptDialog.Confirm(
                        context,
                        ResumeAfterAskedToEnterNewNumber,
                        promptOptions: new PromptOptions<string>(
                            prompt: "Sorry, you picked the wrong number. Do you want to try again?",
                            retry: "Sorry, I didn't get that. Do you want to enter a new number?",
                            tooManyAttempts: "Too many attempts. I'm ending this conversation.",
                            promptStyler: new PromptStyler(PromptStyle.None)),
                        patterns: new string[][]
                            {
                                new List<string>{ "Yes", "y", "sure", "ok", "yep", "1", "true"}.ToArray(),
                                new List<string>{ "No", "n", "nope", "2", "false" }.ToArray(),
                            });
                }
            }
            catch (TooManyAttemptsException)
            {
                await Done(context);
            }
        }

        private async Task ResumeAfterAskedToEnterNewNumber(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                var pickNewNumber = await result;

                if (pickNewNumber)
                {
                    PromptForNumber(context);
                }
                else
                {
                    var msg = context.MakeMessage();
                    await Done(context, "Ok, goodbye!");
                }
            }
            catch (TooManyAttemptsException)
            {
                await Done(context);
            }
        }

        private async Task Done(IDialogContext context, string message = null)
        {
            message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;

            if (!_isChild)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    await context.Complete(message);
                }
                else
                {
                    await context.Complete();
                }
            }
            else
            {
                context.Done(new ChildDialogModel
                {
                    Success = !string.IsNullOrWhiteSpace(message),
                    Message = message
                });
            }
        }
    }
}
