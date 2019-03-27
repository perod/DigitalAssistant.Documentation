#  Ten usability heuristics for evaluating your bot

## This topic

This topic describes 10 usability heuristics applied to bot interaction which can be used to evaluate the general usability of your bot.

### 1. Visibility of system status

The system should always keep users informed about what is happening using appropriate and timely feedback.

Your bot should be responsive and a certain task is taking a long time, then the user shouldn't be left hanging and waiting for a response. Provide a typing indicator, like a human does and indicate how long a request will take. For example:

*“I'm working on this now…give me a few minutes.”*

provides feedback that the system is still alive and working on the given task.

### 2. Match between system and the real world 

The system should speak the users’ language using words, phrases and concepts familiar to the user, rather than system terms and jargon wherever possible. Follow real-world conventions, making information appear in a natural and logical order.

Speak the user's language and use the terminology the user expects and is comfortable with. Build your bot with a solid understanding of the use case and the domain audience who will be using it. Write for the lowest common denominator.


### 3. User control and freedom 

Users often choose system functions by mistake and will need a clearly marked “emergency exit” to leave the unwanted state without having to go through an extended dialog. Provide undo and redo functions. For example if the user gets lost a message could appear informing of the available options.

*"I'm sorry I didn't understand that. You can say 'help' if you need help, or 'start over' if you want to start again."*

Allow users to cancel or undo input and to switch context seamlessly.

### 4. Consistency and standards

Users shouldn't have to wonder whether different words, situations, or actions mean the same thing. Follow platform conventions. Bots should be internally consistent and adopt the same communication tone and style across the application and "Unit4 bot library".

### 5. Error prevention

 Better than good error messages is a robust design which prevents problems from occurring in the first place. To help eliminate error-prone conditions:

- avoid open-ended questions
- ask for confirmation from the user for any critical step in the interaction
- present users with a confirmation option before they commit to an action where they have made choices, or where committing to the action by mistake would cause inconvenience to the user (for example submitting a timesheet with the wrong project codes without asking)
- provide summaries after complex steps where several choices have been made
- account for typing errors -- try and anticipate and account for common typing errors to keywords that your bot needs to respond to
- use synonyms for keywords that your bot should respond to

### 6. Recognition rather than recall

 Minimize the user’s memory load by making objects, actions, and options visible. Don't make the user remember unreasonable things such as time codes, project codes or order numbers etc. The user should not have to remember information from one part of the dialog to another. Instructions for use of the bot should be visible or easily retrievable whenever appropriate. Also build in assistance to your bot, for example if a response is needed in a specific format, then give an example of the format required etc.

### 7. Flexibility and efficiency of use 

Accelerators are used to speed up the interaction for the expert user such that the system can cater successfully for both inexperienced and experienced users (efficiency of use versus ease of learning). For example, an experienced user could type a word like "buy" to trigger the purchasing bot without going through a longer dialog first. A set of advanced commands could also be provided using, for example hashtags to prefix the commands etc.

### 8. Aesthetic and minimalist design 

Dialogs shouldn't contain any extra information which is irrelevant or rarely needed. Every extra piece of information in a dialog competes with the relevant information and dilutes its relative visibility, making life more difficult for the user.

### 9. Help users recognize, diagnose, and recover from errors

 Any error messages should be expressed in plain language (no codes), precisely indicate the problem, and constructively suggest a solution. In cases where user input is not understood due to typos, misspellings or various errors then ask the user to reenter the text.  As a minimum, in cases of simple words and phrases, your bot should be able to understand user's utterances without asking to reformulate the entered sentence.


### 10. Help 

Ideally the system is able to be used without any help but it might be necessary to provide some basic help options which, for example, tell the user what the bot can do and how to proceed, and how to use any accelerators if these are available. Any help should be focused on the user’s tasks, list concrete steps to be done and be compact. If the bot detects that the user is having problems, then the user should be prompted to ask for help, for example:

*"It looks like you're having some problems. You can always ask me for help at anytime by just typing 'help'."*

An example of help from the Poncho weather bot:

![](/images/ponchoHelpExample.png)



