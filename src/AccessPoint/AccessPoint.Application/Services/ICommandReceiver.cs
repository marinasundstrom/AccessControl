using System;
using System.Threading.Tasks;

namespace AccessPoint.Application.Services
{
    public interface ICommandReceiver
    {
        Task SetCommandHandler<A, R>(Func<A, Task<R>> handler);
    }
}
