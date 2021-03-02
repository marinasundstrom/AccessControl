# Access Control

For Fun Physical Access Control system (door lock and alarm) - Raspberry Pi, .NET 5, ASP.NET, Azure IoT Hub.

Initially, built in 2018-2019. Code refactored in February 2021.

More info in [docs](/docs).

* [Video: WebApp](https://www.youtube.com/watch?v=VlSKTeJASYc)

* [Video: Push Notifications and Pi](https://www.youtube.com/watch?v=9nb2P9FmH2Y)

## Purpose

Learning about building microservices, better structuring code, and some electronic low-level programming.

Expressing my creativity - having fun.

### Updates

Since publishing this project, I have been updating everything to .NET 5, from an earlier version of .NET Core.

There has been some drastic restructuring going on since then.

My goal is to recreate my original Raspberry PI set up, and to create guides on how to set everything up.

## Screenshots

| Alarm         | Access Log    |
| ------------- | --------------|
| <img src="/images/screenshots/webapp-alarm.png" style="max-height: 450px"  />             | <img src="/images/screenshots/webapp-accesslog.png" style="max-height: 450px"  />   |

| Raspberry Pi  |
| ------------- |
| <img src="/images/photos/pi.jpeg" style="max-height: 450px" />  |

## Parts

The project consists of the 2 main services:
* AppService - Cloud service
* AccessPoint - Running on the Raspberry Pi

The services connect to these Azure services:
* Azure IoT Hub
* Azure Notification Hub

It also contains these apps: 
* Web App (Blazor)
* Mobile App (Xamarin.Forms)

The Raspberry Pi uses the following components:
* Magnet door sensor
* Buzzer (for signal)
* Relay (for a door lock)
* RFID Card Reader (MFRC522) 
* RGB LED (light with color)
* Motion Sensor (PIR)
* Button

[Schematics](/docs/raspberry-pi/schematics.md) for the setup.

## Development requirements

* .NET 5 SDK

Additional tools:

* Docker
* Project "Tye" - to simplify launching and running multiple services in parallel when developing.

## Running the project

You can run services separately but that requires some configuration. Instead, Project Tye is strongly recommended.

### Using Project Tye

Having the Tye global tools installed.

To run the projects simply write the following command when in the root directory:

```
tye run
```