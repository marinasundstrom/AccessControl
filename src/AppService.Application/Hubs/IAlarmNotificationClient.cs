using System.Threading.Tasks;

namespace AppService.Application.Hubs
{
    public interface IAlarmNotificationClient
    {
        Task ReceiveAlarmNotification(AlarmNotification notification);
    }
}
