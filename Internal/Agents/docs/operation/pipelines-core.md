
# Digital Assistant pipeline

## Build definition

https://unit4-peopleplatform.visualstudio.com/People%20Platform/People%20Platform%20Team/_apps/hub/ms.vss-ciworkflow.build-ci-hub?_a=edit-build-definition&id=258


## Release definition

https://unit4-peopleplatform.visualstudio.com/People%20Platform/_apps/hub/ms.vss-releaseManagement-web.cd-workflow-hub?definitionId=320&_a=definition-pipeline

## Digital Assistant Push service

As described in the architecture guides, the digital assistant push service must be deployed into a regional infrastucture. That infrastucture must have corresponding Unit4 People Platform services like Unit4 MessageHub.
See the [notifications architecture](notifications.md) for details.

> Configuration in the associated Unit4 Message Hub is required. Message Hub configuration must be reflected in the application settings section of the service.


