using System.Threading.Tasks;

namespace AccessControl.AppService.Application.Hubs
{
    public interface IAlarmNotificationClient
    {
        Task ReceiveAlarmNotification(AlarmNotification notification);
    }
}
