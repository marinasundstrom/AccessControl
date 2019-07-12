using System.Threading.Tasks;

namespace Foobiq.AccessControl.AppService.Application.Hubs
{
    public interface IAlarmNotificationClient
    {
        Task ReceiveAlarmNotification(AlarmNotification notification);
    }
}
