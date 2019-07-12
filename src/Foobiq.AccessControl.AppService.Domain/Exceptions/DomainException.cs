using System;

namespace Foobiq.AccessControl.AppService.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
