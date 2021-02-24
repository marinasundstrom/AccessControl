# Overview

## Architecture
* Clean architecture, CQRS with Mediator-pattern.
* Azure Services - IoT Hub, Notification Hub

## Services

### AppService
Responsible for handling requests and granting access to an AccessPoint.

It is controlled through a Web API, which both the Web App and Mobile App uses.

Communicates with AccessPoint through IoT Hub and Event Hub.

The service also sends Push Notifications to phones that have the Mobile App installed.

### AccessPoint
Represents a physical AccessPoint. (There can be multiple AccessPoints)

It controls the hardware on behalf of the App Service.

Runs in Raspberry Pi OS (Raspbian), on a Raspberry Pi. 

It can even run in a Docker container, if desired.

Peripherals:
* RFID reader
* Relay
* LED
* Button

Uses the ```System.Devices``` package.

## Apps

### Web App
Basic UI for monitoring the system.

Functionality:

* Request access (Arm and Disarm)
* Create identities. 
* View  Live Access Logs

### Mobile App

Functionality:

* Request access (Arm and Disarm)
* Receive Push Notifications.

Currently only the Android app is working