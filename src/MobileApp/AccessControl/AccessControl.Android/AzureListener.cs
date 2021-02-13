using Android.App;
using Android.Content;
using Android.Support.V4.App;
using WindowsAzure.Messaging.NotificationHubs;

namespace AccessControl.Droid
{
    public class AzureListener : Java.Lang.Object, INotificationListener
    {
        public void OnPushNotificationReceived(Context context, INotificationMessage message)
        {
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(context, MainActivity.CHANNEL_ID);

            notificationBuilder.SetContentTitle(message.Title)
                        .SetSmallIcon(Resource.Drawable.ic_stat_button_click)
                        .SetContentText(message.Body)
                        .SetAutoCancel(true)
                        .SetShowWhen(false)
                        .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(context);

            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}