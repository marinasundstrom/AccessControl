using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccessControl.Services
{
    public interface IPage
    {
        Task InitializeAsync(object arg);
    }
}
