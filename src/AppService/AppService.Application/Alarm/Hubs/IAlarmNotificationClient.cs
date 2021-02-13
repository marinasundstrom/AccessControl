using System.Threading.Tasks;

namespace AppService.Application.Alarm.Hubs
{
    public interface IAlarmNotificationClient
    {
        Task ReceiveAlarmNotification(AlarmNotification notification);
    }
}
