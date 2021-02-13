using System;
using System.Threading.Tasks;

namespace AccessPoint.Services
{
    public interface ICommandReceiver
    {
        Task SetCommandHandler<A, R>(Func<A, Task<R>> handler);
    }
}