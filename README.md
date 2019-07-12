# Access Control

Proof-of-concept of an Physical Access Control system - built with .NET Core, ASP.NET Core and Docker.

Consists of 2 microservices (Access Point and App Service), a Web UI, and a Mobile App.

## Architecture
* Mediator-pattern (CQRS)
* Azure Services - IoT Hub and Event Bus

## AppService
Responsible for handling requests and granting access.

Web API.

## AccessPoint
Controls the hardware on behalf of the App Service.

Runs on a Raspberry Pi, in a Docker container.

Peripherals:
* RFID reader
* Relay
* LED
* Button

## Mobile App
Request access.

## Web UI
* Mirrors mobile app. 
* Create identities. 
* View  Live Access Logs