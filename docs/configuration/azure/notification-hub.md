# Notifications Hub

## Setup Guide

https://docs.microsoft.com/sv-se/azure/notification-hubs/xamarin-notification-hubs-push-notifications-android-gcm

## Test message

```json
{
    "notification": {
       "title":"Title1",
       "body":"Body1",
       "priority":"10",
       "sound":"default",
       "time_to_live":"600"
    },
    "data": {
       "title":"Title2",
       "body":"Body2",
       "url":"https://example.com"
    }
}
```