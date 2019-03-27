
# Release pipelines

This section describes the release pipelines used by the central services in the Wanda ecosystem and provides an overview of which Git repositories, processes and artifacts are used. The specific details are not provided here as these are included in the build definitions, release pipelines and ARM templates.

> Knowledge of Azure DevOps pipelines is required and isn't covered here.

> This section only covers what are considered core services of the Wanda ecosystem and does not cover the core Unit4 People Platforms services such as Unit4 Discovery Service and Unit4 Indetity Mapper etc.

## Environments

The release pipeline is setup to support the following digital assistant environments:

- **Wendy** - dev environment
- **Wanda demo** - demo environment, that supports specific conversation flows
- **Wanda** - production, used by customers

## Outline of repositories

### DigitalAssistant
- Assistant service and core webjobs
- Help chatbot
- Salutation chatbot
- Diagnostics chatbot
- Adaptive cards renderer service

### DigitalAssistant.PushService
- Digital Assistant Push Service (webjob)

### TravelDomain
- Travel and expenses domain API
- TravelRequest chatbot
- Expenses chatbot

### TimesheetDomain
- Timesheet domain API
- Timesheet chatbot

### HumanResourcesFunctionalAggregator
- Human resources domain API
- Payslip chatbot
- Balances chatbot
- Absence chatbot

### Purchasing
- Purchasing domain API
- Purchasing chatbot

### WorkflowDomain
- Tasks domain API
- Tasks chatbot
- Tasks notifications webjob
- Tasks portal (web app)

## ARM and parameters

The Azure Resource Manager templates used to create and deploy the services in the ecosystem are located in a dedicated Git repository - **DigitalAssistant.DevOps**.
The Git repository contains environment-specific parameters for the environments supported. 

> Some ARM parameters are defined in variable groups or in pipeline variables, not in the parameter files.

## Build definitions

The build definitions for the Git repositories are maintained in Azure DevOps.

## Outline of release pipelines

All repositories have the following branch structure:

- **feature branch**
- master 
- release
- demo

All development is done in feature branches. New development or other changes are merged to the master branch using pull requests. Rules for pull requests include code review and green test runs.
All master branches are setup for automatic release to the environment - a sucessful build results in a release of the build artifacts to the corresponding infrastucture (Azure).

> Release definitions are maintained in Azure DevOps.

Release to the production environment is manual. Changes from the master branch are merged to a release branch via the creation of a pull request. The release step is also manual but requires approval.

The following guides highlight important characteristics of the specific release pipelines.







