# Disaster recovery

The Wanda ecosystem system supports failover/disaster recovery environment in case the primary ecosystem is unhealthy.
To understand the disaster recovery strategy for Wanda knowledge of the both the system architecture and release piplines are required. The disaster recovery environments for the services in the ecosystem are defined by the release piplines. All services are by default deployed to the North Europe geo-political region. The secondary environments (disaster recovery) are deployed to West Europe.

## Overview
The Wanda ecosystem does not support failover for single services in the core ecosystem. A failover will cause the system to use the disaster recovery for all services in the core ecosystem. This includes the assistant service, all chatbots and supporting domain APIs - but not the global services (IDS, IDM, DISCO, etc.). 

A single traffic manager in front of the digital assistant Web API controls the failover. If activated, all incoming traffic will be routed to the secondary environment.

## Disaster recovery deployment
All release piplines for the Wanda ecosystem contain a manual step to deploy replicas of the primary services to the disaster recovery environment. These deployments use disaster recovery specific release parameters. 
The disaster recovery chatbots listen to the disaster recovery service bus infrastructure and call the disaster recovery version of the associated domain APIs.

> Deployments to disaster recovery are manual, and need to be triggered by an operations engineer using Azure DevOps.

## Swap to disaster recovery
There is no automatic swap to the disaster recovery ecosystem, this has to be done manually on the traffic manager in the digital assistant resource group.

> The autoswap based on the availability/health check on the primary environment is currently not enabled because of transient failures reported by the health monitoring.

## Considerations
A failover to the disaster recovery environment (and also a swap back to the primary environment) will affect users that have active conversations with Wanda at that moment. A conversation is considered active if it is not ended by either bot or user and not older than 20 minutes. Those users will not able to continue their conversation after the switch and have to start over. The reason for this is that the conversation state storage (Azure Table Storage) is not shared or replicated between primary and disaster recovery environment. 

> This weakness can be solved by using the same storage account for both environments and setting up specific geo-replication on the storage account.

## Tasks portal
The tasks portal web application is deployed with its own traffic manager. And therefore has its own disaster recovery strategy, disconnected from the Wanda disaster recovery ecosystem.

