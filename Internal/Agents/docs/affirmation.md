
# Affirmation

In a conversational flow you might have situations where you need to ask the user yes/no questions. 
Users will answer such questions in several ways and to handle these types of questions and responses in a uniform way, the U4.Bot.Builder framework provides a set of dialogs and services.
We refer to this as the **Affirmation** concept and this concept is backed by a central LUIS application that can classify user utterances, and determine if a user replies positively or negatively. 

> The LUIS model for affirmation will be managed by the Unit4 People Platform and receive continuous training.
  
In addition to simple yes/no scenarios the AffirmationService is also able to handle more natural conversation cases. 
For example, the bot could ask the question _"Do you want to provide a cost estimate?"_ and the user responds with _"Yes 500 euros"_ instead of just _"Yes"_ and then waiting for the question for the actual amount.

## Affirmation service

See [description of service](bot-builder-services.md#affirmationservice)

The above scenario can be handled using the typed version of the AffirmationService.GetAffirmation method with a custom parse method that will be used with all parts of the user utterance that are not classified as Affirmation or Negation.

``` csharp
    private async Task AskForCostEstimate(IDialogContext context)
    {
        await context.PostAsync("Do you want to provide a cost estimate?").ConfigureAwait(false);
        context.Wait(ResumeAfterCostEstimateQuestion);
    }

    private async Task ResumeAfterCostEstimateQuestion(IDialogContext context, IAwaitable<IMessageActivity> result)
    {
        var message = await result;

        var affirmationService = context.Resolve<IAffirmationService>();
        var affirmationResult = await affirmationService.GetAffirmation<double>(context, message.Text, TryParse).ConfigureAwait(false);
        if (AffirmationType.Negative == affirmationResult.Result)
        {   // user said no, doesn't matter if a value was specified
            AskForSomethingElse(context);	// continue with dialog
            return;
        }

        if (affirmationResult.ValueParsed)
        {   // user has given a value
            Save(affirmationResult.Value);	// save value and continue with dialog
            AskForSomethingElse(context);	// continue with dialog
            return;
        }

        if (AffirmationType.Positive == affirmationResult.Result)
        {   // user said yes, but no value specified
            AskForCostEstimateValue(context);   // ask the user for the actual value
            return;
        }
        // ...
    }

    private Task<ParseResult<double>> TryParse(IDialogContext context, string text)
    {
        double value;
        return Task.FromResult(
            double.TryParse(text, out value)
                ? new ParseResult<double>(value)
                : new ParseResult<double>());
    }
```


## Affirmation dialogs

In most scenarios the AffirmationService is not used directly but through a set of dialogs that the U4.Bot.Builder provides.
These dialogs combine the AffirmationService with the retry logic of Microsofts PromptDialogs.

* Affirmation - pure positive / negative detection, no value parsing
* AffirmationBool - parsing of boolean values
* AffirmationDouble - parsing of double values
* AffirmationGeneric - parsing of generic values by using a custom parse method
* AffirmationInt64 - parsing of integer values
* AffirmationMoney - parsing of money values (amount and currency combinations)

The above scenario could be solved by using the AffirmationDouble dialog and would provide a retry mechanism for free.

``` csharp
    private async Task AskForCostEstimate(IDialogContext context)
    {
        AffirmationDouble.Dialog(context, ResumeAfterCostEstimateQuestion, "Do you want to provide a cost estimate?");
    }

    private async Task ResumeAfterCostEstimateQuestion(IDialogContext context, IAwaitable<AffirmationResult<double>> result)
    {
        AffirmationResult<double> affirmationResult;
        try
        {
            affirmationResult = await result;
        }
        catch (TooManyAttemptsException)
        {
            context.Done("too many attempts to tell me a valid amount");
            return;
        }

        if (AffirmationType.Negative == affirmationResult.Result)
        {   // user said no, doesn't matter if a value was specified
            AskForSomethingElse(context);   // continue with dialog
            return;
        }

        if (affirmationResult.ValueParsed)
        {   // user has given a value
            Save(affirmationResult.Value);  // save value and continue with dialog
            AskForSomethingElse(context);   // continue with dialog
            return;
        }

        if (AffirmationType.Positive == affirmationResult.Result)
        {   // user said yes, but no value specified
            AskForCostEstimateValue(context);   // ask the user for the actual value
            return;
        }
        // ...
    }
```

> Affirmation dialogs and Microsoft PromptDialogs throw TooManyAttemptsExceptions if the user exceeds the specified number of attempts (default 3).

An example how to use the generic Affirmation dialog:
``` csharp
    private async Task AskForFavouriteArtist(IDialogContext context)
    {
        AffirmationGeneric<string>.Dialog(context, ResumeAfterArtistQuestion, "Would you like to tell me your favourite artist?", TryParseArtists);
    }

    private async Task<ParseResult<string>> TryParseArtists(IDialogContext context, string artist)
    {	// check if that artist is actually famous
        var artistDatabase = context.Resolve<IArtistDatabase>();
        return await artistDatabase.IsFamous(artist).ConfigureAwait(false)
            ? new ParseResult<string>(artist)
            : new ParseResult<string>();
    }

    private async Task ResumeAfterArtistQuestion(IDialogContext context, IAwaitable<AffirmationResult<string>> result)
    {
        AffirmationResult<string> affirmationResult;
        try
        {
            affirmationResult = await result;
        }
        catch (TooManyAttemptsException)
        {
            context.Done("Sorry, I didn't know any of them :(");
            return;
        }

        if (AffirmationType.Negative == affirmationResult.Result)
        {   // user said no
            context.Done("Too bad, then I will not talk to you anymore!");
            return;
        }

        if (affirmationResult.ValueParsed)
        {   // user has given an artist
            context.Done($"Wow! I love {affirmationResult.Value} too!");
            return;
        }

        if (AffirmationType.Positive == affirmationResult.Result)
        {   // user said yes, but no artist specified
            PromptDialog.Text(context, ResumeAfterArtist, "Who is it?");   // ask the user for the artist via Microsofts PromptDialog
            return;
        }
        // ...
    }
```


## Mocking affirmation in fluent tests

If you write [fluent tests](writing-tests.md) for a chatbot that uses any of the supported affirmation dialogs, you need to inject an `IAffirmationService` mock in your test harness.








