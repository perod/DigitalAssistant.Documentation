# LUIS apps for chatbots, repos, environments and deployment

The following documentation only applies to chatbots and teams that use the PeoplePlatform Luis subscriptions.

## Use https://eu.luis.ai when doing developement

- When working with Wendy, our Wanda development enviornment, use eu.luis.ai directly for adding and ammending apps for your chatbots.
- For now, we share one account. Log into https://eu.luis.ai in incognito-mode with user wendy@u4pp.com with password Wa*****0101
- The apps can be trained and published at any time. 
- If you work on LUIS apps outside your teams area, please coordinate with the owner of the LUIS apps before doing changes.

## Use chatbot repos for storing LUIS apps jsons
- Whenever you are happy with a version of a LUIS app in the Wendy LUIS.ai subscription, export the app.
- Use the repository for you chatbot/agent to store exports of your LUIS apps. 
- LUIS app resolver is still in use, so make sure that you keep the luis.json file updated.
- The luis apps jsons will follow the branches when branching out demo and release branches.

## Main-LUIS is fully manual from now until release
 - To allow for manual tweaking and updating of main-LUIS without too much overhead, we have decided to keep this process totally manual, which means that utterances have to be added manually and model trained manually for Wendy.
 - Whenever changes to main-luis is done (and you want to keep it), update the luis application json stored in the DigitalAssistant repository.

## Wendy LUIS
 - As mentioned above, the Wendy LUIS apps will be updated whenever people change and publish them using eu.luis.ai

## Wanda demo LUIS
 - The Wanda demo environment will use the Wanda production Luis subscription.
 - In case the demo environment falls behind or otherwise deviates from the Production environment we will create a special demo Luis subscription.

## Wanda (prod) LUIS
 - The Wanda production environment is built using the release branches in the repositories.
 - The LUIS applications will be automatically deployed (a new release-step) together with the agents/chatbots and digital assistant service.
 - The user/password for Wanda preview luis.ai subscription will not be shared outside the Wanda core team.

