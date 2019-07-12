using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Foobiq.AccessControl.WebPortal
{
    public class DomHelpers
    {
        private readonly IJSRuntime _jSRuntime;

        public DomHelpers(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task ScrollToBottom(bool smooth, int delay)
        {
            await _jSRuntime.InvokeAsync<object>("domHelpers.scrollToBottom", smooth, delay);
        }
    }
}
