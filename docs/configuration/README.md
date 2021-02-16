# Configuration

In order to connect to external services, the apps require some configuration. 

That is mainly done in through the .NET Configuration infrastructure, and settings are set in the ```appsettings.json``` files.

## Prerequisites 
In order to get going you need to set up the following external services:

* Azure IoT Hub
* Azure Notification Hub

## Development environment 

### Source control

***Do not check in sensitive data, like API keys and Connections Strings containing keys into Source Control!***

### Secrets

In a development environment it is recommended to store sensitive data as secrets.

These settings will be merged with the Configuration when the app is started.

#### Set secrets from a file

```
cat ~/Projects/secrets.json | dotnet user-secrets set
```