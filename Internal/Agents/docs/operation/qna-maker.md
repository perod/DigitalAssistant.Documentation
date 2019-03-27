
# QnA Maker

To provide an extensible conversational experience, Wanda relies on the QnA Maker Service to generate question and answer knowledge bases and provide the logic to answer typical user questions.
As with LUIS, QnA Maker relies on a cognitive service resource deployed in Azure. 
This resource consists of several services that are used by Microsoft to classify user questions and return matching answers. 

The knowledge bases are created and maintained using [https://www.qnamaker.ai/](https://www.qnamaker.ai/).

## Standard knowledge bases

Wanda is dependant on two knowledgbases in the conversation pipeline:

- **Help QnA** trained with QnA pairs to provide basic answers to standard Wanda related help questions
- **Fallback QnA** trained with a set of answers to the type of offbeat questions users typically ask chatbots when encountering them for the first time

## Endpoint keys

Endpoint keys for the knowledge bases are obtained via the qnamaker.ai portal. The subscription key is obtained via the cognitive resource in Azure.

For Wanda the following subscription key and endpoint keys are required:

- Standard Help QnA
- Fallback QnA

These configurations must be configured in the Assistant Service and the common Help chatbot. The proposed way of handling this is to keep them in a KeyVault used for deployment, so that they are read as part of the release pipeline.

## Language support

The QnA Maker service used by Wanda is created in English. To be able to provide answers in the other supported languages, some initial steps are required.

* All QnA pairs in the two knowledge bases must be assigned with a **translation** identifier. This must be added as a metadata entry to the QnA pair.
* At runtime Wanda translates all user requests to English and sends them to the QnA Maker service.
* If the service returns an answer, the translation metadata is used to get the translated answer for the QnA pair. The translated answers are stored in the storage account in the QnA Maker resource in Azure, and 

> A custom tool to manage the translations is available and can be downloaded from Azure Devops artifacts. Additional material on how to use the tool can be provided upon request.

## Question to answer flow

When the users as a question in his or her native language, it will be translated to English by Wanda before it is sent to the English QnA Service.
The returned Eglish QnA pair is then checkd for a **translation** metadata key. If found, the translation ID value combined with the user's language is used to find the trabslated answer in the table store.
The translated answer is returned to the user.