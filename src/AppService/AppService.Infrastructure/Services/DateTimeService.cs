using System.Net.Mail;

using AppService.Application.Services;

namespace AppService.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}