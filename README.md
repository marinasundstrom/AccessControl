# Access Control

For Fun Physical Access Control system (door lock and alarm) - built with .NET 5 and ASP.NET Core.

## Parts

The project consists of the 2 main services:
* AppService
* AccessPoint

It also contains these apps: 
* Web App (Blazor)
* Mobile App (Xamarin.Forms)

## Purpose

Learning about building microservices, better structuring code, and some electronic low-level programming.

Expressing my creativity - having fun.

## Updates

Since publishing this project, I have been updating everything to .NET 5, from an earlier version of .NET Core.

There has been some drastic restructuring going on since then.

My goal is to recreate my original Raspberry PI set up, and to create guides on how to set everything up.

## Todo
Here is a list of what is to be dones:

* Refactor code

* Access Point
    * Introduce Clean Architecture

* Web App
    * Upgrade to Bootstrap 5
    * Fix user experience

* Data
    * Seed initial and test data in database

* Configuration
    * Improve storing and retrieving settings, including connection strings.

* Docs
    * Improve docs on configuration

## Architecture
* Clean architecture, CQRS with Mediator-pattern.
* Azure Services - IoT Hub, Event Bus, Notification Hub

## Developer Requirements

* .NET 5.0.* SDK for building

Addional tools:

* Project "Tye" - to simplify launching and running multiple services in parallel when developing.

## Running the project

You can run services separately but that requires some configuration. Instead, Project Tye is strongly recommended.

### Using Project Tye

Having the Tye global tools installed.

To run the projects simply write the following command when in the root directory:

```
tye run
```

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

Using the ```System.Devices``` package (dotnet/iot on GitHub).

## Apps

### Web App
Basic UI for monitoring the system.

Functionality:

* Request access (Mirrors mobile app)
* Create identities. 
* View  Live Access Logs

### Mobile App
Login and request access.

Currently only the Android app is working