#  Guidelines for creating bot conversation texts

## This topic

This topic provides a set of writing guidelines and considerations to be used when creating conversation texts for your chatbot.

## Wanda's general style and tone
Wanda represents Unit4 and must cater for a wide range of users who are working under a wide range of circumstances. Therefore, Wanda must present a professional, business-like and neutral persona. Wanda should:

- be friendly and informal
- use simple, colloquial and natural human language
- show the user respect at all times
- not risk offending or annoying the user by trying to be funny, telling jokes or engaging in sarcasm
- focus on helping the user accomplish her/her tasks as quickly and easily as possible
- avoid cultural references
- understand that English might not be the user's first language and uses a simple and direct language style


## Use of contractions

A contraction is a combination of two words linked with an apostrophe into one, shortened word. Contractions are normally a noun with a verb, and are simple, easier to pronounce and say in normal conversation.

People are used to seeing contractions in web and social media texts and expect to see them in a chat environment. Avoiding them will make your bot seem too formal or too stiff. Therefore use contractions in your bot conversations as these help to convey a less formal and relaxed tone.

Contractions such as I'm and I'll allow the conversation to flow in a more natural way and make for an easy and conversational tone. 

**List of common contractions to use**

The following contractions are commonly used: 

* I am = I’m
* You are = You’re
* Here is = Here's
* That is = That's
* They are = They’re (not to be confused with there or their)
* Do not = Don’t
* Will not = Won’t
* Cannot = Can’t
* Should not = Shouldn’t
* It is = It’s (not to be confused with its, the possessive)
* Is not = Isn’t
* You have = You've

**List of contractions to avoid**

The following contractions are awkward sounding and shouldn't be used:

* She would = She’d
* He would = He’d
* Would have = Would’ve
* There have = There've
* They have = They've
* That would = That'd
 
## Conversational versus grammatically correct
The focus should always be on the conversation flow and how things are said in a real conversation between real people. In real conversations the formal and strict rules of grammar are not always strictly followed and you should aim to keep Wanda's replies as conversational as possible, even if this sometimes means bending the formal rules of grammar to make the conversation flow better. But having said that, you should observe correct punctuation, spelling and capitalization. 

## Message texts
**Active voice**

Use the active voice as much as possible as this provides a more direct, less wordy and natural style of conversation. 

In an active sentence, the subject of the sentence is doing something whereas in a passive sentence, something is being done to the subject (making the subject passive).

Do write:

*"I submitted your timesheet for week 42."*

Don't write:

*"Your timesheet for week 42 has been submitted."*

**Message length**

Avoid lengthy texts and keep all dialogs short and to the point as users are unlikely to read lots of text. 

Don't ask the user to do more than one thing in the same text.

Focus your texts around intents; that is, what actions you want the user to take and what you want the user to accomplish. This also makes it easier to set expectations, provide context and steer the conversation in the right direction.

**First and second person**

Use "you" when referring to the user and "I" when referring to Wanda. 

**Avoid open-ended questions**

Don’t use open-ended standalone questions such as *"How are you?"*. This could imply free form interaction and encourage people to respond in ways you don’t anticipate. If you do ask questions, include a hint as to the expected response and format, or consider adding buttons listing the available options.

**Questions that invite a yes/no response**

When asking for data, avoid phrasing the request as a question that could potentially be answered with a yes or no as you will need to then cater for these the conversation. For example in stead of *"Can you give me the amount?"*, it would be better so say *"Please tell me the amount."*

**Use of tentative words such as can, should and may**

Try to avoid using tentative words such as should and could as these are often imprecise and can imply that a required user action is optional when it is required.  For example:

*"You should type the week number to see which projects you worked on that week."* is less direct than *"Type the week number to see which projects you worked on that week."*

Use "must" in cases where the user must take an action.

**Exclamation marks**

Wanda should never shout at the user therefore don't use exclamation marks in your Wanda replies.

## Translation
Keep translation in mind when crafting your conversations. Consider how well your texts will stand up to being translated and avoid slang, ambiguities, cultural references and metaphors that risk not translating well.

## Make your Wanda sentences English language neutral
UK English and US English contain differences in spelling and certain words have slightly different or even completely different meanings. Try to structure or write your Wanda responses in a way that make them neutral of English language differences. For example, in a travel request scenario, instead of having Wanda ask:

*When are you travelling?*

You would rewrite this to:

*When would you like to travel?* or *When do you want to travel?*

to avoid the spelling differences of *travelling* versus *traveling* between UK and US English.

For an overview of the differences between UK and US English, see for example <a href="https://en.wikipedia.org/wiki/American_and_British_English_spelling_differences" target="_blank">American and British English spelling differences</a> and <a href="https://en.wikipedia.org/wiki/Comparison_of_American_and_British_English" target="_blank">Comparison of American and British English</a>.


## Use of alternative texts to provide message variety
Write several versions of each message as this will help provide a more varied and engaging interaction for the user than if you always use the same phrases to handle the same situations.

## Avoid compound sentences and title reuse

Avoid using compound sentences wherever possible. Sentences consisting of fragments joined together, or sentences consisting of two or more smaller sentences joined together, don’t translate well and reduce flexibility when designing  the conversation. They also create dependencies as changing individual sentence fragments becomes It’s better to create separate titles for each scenario, rather than reuse titles in different situations.

Reusing titles in multiple contexts reduces flexibility as it means any changes to a title need to take into account how it appears in the various contexts, and as a result compromises are usually needed which means the sentence is not 100% ideal in any context. It's therefore better to create separate titles for each scenario as this allows you to fine tune each title to exactly fit its context, and create good alternative texts for a better conversational experience.

## Buttons
Where the user has a number of fixed choices, display these choices as buttons to guide the conversation and keep it on track.

## Currency and dates
Place the currency code after the number like this 125 GBP, 125 NOK etc.

Present dates to the user in Wanda's replies in the format *Monday 12 June* as this is easier to read.

Use ISO 8601 format (yyyy-mm-dd) when presenting dates in forms.


## Use of emoji, smileys and emoticons

Don't use emoji, smileys or emoticons as they risk being misinterpreted, are not supported consistently across all platforms and can appear differently on different platforms (for example, emoji symbols can look different on Android and on iOS).  




