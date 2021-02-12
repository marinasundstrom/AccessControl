using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessControl.AppService.Domain.Models
{
    public class TokenResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
