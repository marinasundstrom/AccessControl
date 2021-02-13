# Configuration

In order to connect to external services, the apps require some configuration. 

That is mainly done in through the .NET Configuration infrastructure, and settings are set in the ```appsettings.json``` files.

## Prerequisites 
In order to get going you need to set up the following external services:

* Azure IoT Hub
* Azure Notification Hub
* Azure Event Hub

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

## AppService

```json
{
    "Hub": {
        "ConnectionString": "<CONNECTION-STRING>"
    },
    "Notifications": {
        "ConnectionString": "<CONNECTION-STRING>",
        "Path": "<HUB-NAME>"
    },
    "Events": {
        "ConnectionString": "<CONNECTION-STRING>"
    }
}
```

## AccessPoint

```json
{
    "Events": {
        "ConnectionString": "<CONNECTION-STRING>"
    }
}
```


## Mobile App

### Android

#### Constants

Update ```Constants.cs``` with the Connection String for Azure Notification Hub:

```csharp
    public static class Constants
    {
        public const string ListenConnectionString = "";
        public const string NotificationHubName = "AccessControl-NotificationHub";
    }
```

#### google-services.json

Add the ```google-services.json``` that you probably have got from Firebase or another service.