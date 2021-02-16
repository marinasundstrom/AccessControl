# Mobile app

## Android

### Constants

Update ```Constants.cs``` with the Connection String for Azure Notification Hub:

```csharp
    public static class Constants
    {
        public const string ListenConnectionString = "";
        public const string NotificationHubName = "AccessControl-NotificationHub";
    }
```

### google-services.json

Add the ```google-services.json``` that you probably have got from Firebase or another service.