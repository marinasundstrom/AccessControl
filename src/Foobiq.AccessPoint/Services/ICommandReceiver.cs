using System;
using System.Threading.Tasks;

namespace Foobiq.AccessPoint.Services
{
    public interface ICommandReceiver
    {
        Task SetCommandHandler<A, R>(Func<A, Task<R>> handler);
    }
}