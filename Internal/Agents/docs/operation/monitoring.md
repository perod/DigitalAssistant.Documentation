
# Monitoring

The Wanda ecosystem consists of a set of independent services which together provide the digital assistant experience.
For the core web application Wanda, all chatbot skills and the supporting domain APIs are logged in a central Azure Application Insights instance.

> Knowledge about how to use Application Insights is required to be able to monitor the Wanda ecosystem. See [Microsoft's documentation](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-overview) for details on Application Insights.

## Application insights

Most of the services in the ecosystem log events to the central Application Insights. 
The standard log entries will result in trace entries with associated severity levels.

All services in the ecosystem are implemented so that any runtime exceptions are written as exceptions in Application Insights.

> All exceptions written to the application insights reflect the .Net exception type, but exceptions that occur in a chatbot are of type `AssistantService.Core.Exceptions.ChatbotException`. These exceptions carry the **chatbotId** property to indicate the origin of the exception.

All services can also be configured individually with the minimum log level. 

> By default all services are configured with minimum log level **information**.

## Logs

All chatbots deployed in the ecosystem log events on the central service bus.
The log messages are picked up by the service that write the events to the centrally configured log sinks. The standard sinks are trace in Application Insights.

All core domain APIs are configured to log events to the central Application Insights.

### Custom properies

To aggregate data, custom properties are added to the log entries (trace) when written to Application Insights. 
The log system supports the following custom properties:

- chatbot/chatbotId
- tenantId
- SourceSystem

#### chatbot
The **chatbot** property can be used to identify which chatbot wrote a log entry.

#### tenantId

#### SourceSystem
The ecosystem currently supports the following **SourceSystem** identifieres:

- u4-travel-fa
- u4-human-resources-fa
- u4-purchasing-fa
- u4-timesheet-fa
- u4-tasks-fa

These identifiers refer to the functional aggregators (domain API) used in the Wanda ecosystem.

### Data privacy

The log entries in the ecosystem are stripped for all personal identifiers.  

## Web telemetry

All web applications in the ecosystem are setup to capture web telemetry. 
The services will capture all incoming operations and dependecy calls, this telemetry is written to the central Application Insights.
The application map in the Application Insights give an overview of the source systems (individual services) and the associated web telemetry.

> The cloud role name property of web telemetry can be used to identify the telemetry origin.

### Custom events

The Wanda ecosystem supports the following set of predefined custom telemetry events which can be used to better understanding how the system behaves:

- Activity
- MessageToChatbot
- MainIntentClassified
- ConversationCompleted
- ConversationAbandon
- HelpRequested
- ConversationFailed
- DiagnosticRequested

All of these carry properties can be used further to analyze how the system operates.

## Dashboards

Creating a Dashboard provides a common view of the data available in Application Insights. See [Microsoft's documentation](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-dashboards#dashboards) for information about Azure dashboards.

> Such dashboards exist but must be recreated if the ecosystem moves to a new deployment infrastucture

## Healthcheck

The core assistant service web API is deployed with a healthcheck endpoint. 
This endpoint will return the result of the latest scheduled health check. A health check is an extensive check of the dependant services and chatbots.
If any of the services report health issues, these will be written to the health repository and returned by the health endpoint.

In Application Insights the health endpoint is used in a standard ping test. The pulse check is presented in the availability chart in Application Insights.
All pulse checks are also logged as **Availability** telemetry in Application Insights. 

