
# LUIS service

## This topic
Wanda uses the Microsoft Language Understanding Intelligent Service (LUIS) for language understanding to classify intents and extract entities. This guide describes the operational tasks related to running the various LUIS language apps/models used in the Wanda ecosystem and does not describe specific LUIS language apps in detail.

> LUIS concepts are not covered in this topic. See the [Microsoft LUIS documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/) for details.

## Region
All LUIS applications are created and maintained in the European region (https://eu.luis.ai/home).

## Language
Wanda uses English LUIS applications only. The ecosystem supports multiple languages but all user input is translated to English before being sent to LUIS for classification.

## LUIS subscriptions

## Azure LUIS Cognitive Service
To operate LUIS applications a LUIS Cognitive Service is created in the Azure subscription. Via this resource you obtain the LUIS endpoint key. The resource is also used for billing.
The LUIS subscription key is obtained from the LUIS portal. Additional access control for users is setup in the LUIS portal.

### Production subscription

The production Wanda ecosystem uses a dedicated LUIS subscription to manage all LUIS applications used in the ecosystem.
A general task is to monitor those applications and train them based on suggested utterances registered by LUIS. This process, called active learning, allows LUIS to improve its intent and entity classification, and thus respond better to user utterances and get smarter over time.

> See the [Microsoft LUIS documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/) for details.

### Development subscription
The production subscription is not used for development and a specific development subscription is used that contains the same LUIS applications. This allows development work to be done without affecting production.

 > A key aspect of operating the LUIS applications is to synchronize the LUIS applications in the production and development subscriptions. This is a manual step and will be affected if the LUIS application is changed as a result of new developments.












 