using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Messages.Events;
using AccessPoint.Application.Alarm.Queries;
using AccessPoint.Application.Lock.Commands;
using AccessPoint.Application.Services;
using AppService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessPoint.Application.Authorization.Commands
{
    public class AuthorizeCommand : IRequest<AuthorizationResult>
    {
        public string DeviceId { get; }

        public byte[] TagData { get; private set; }

        public string Pin { get; }

        public AuthorizeCommand(string deviceId, byte[] tagData, string pin = null)
        {
            DeviceId = deviceId;
            TagData = tagData;
            Pin = pin;
        }
        public class AuthorizeCommandHandler : IRequestHandler<AuthorizeCommand, AuthorizationResult>
        {
            private readonly IAuthorizationClient _authorizationClient;
            private readonly ILogger<AuthorizeCommandHandler> _logger;

            public AuthorizeCommandHandler(
                IAuthorizationClient authorizationClient,
                ILogger<AuthorizeCommandHandler> logger)
            {
                _authorizationClient = authorizationClient;
                _logger = logger;
            }

            public async Task<AuthorizationResult> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
            {
                var result = await _authorizationClient.AuthorizeAsync(new AuthorizeCardCommand()
                {
                    DeviceId = request.DeviceId,
                    CardNo = request.TagData,
                    Pin = request.Pin
                });

                return new AuthorizationResult(result.Authorized);
            }
        }
    }
}
