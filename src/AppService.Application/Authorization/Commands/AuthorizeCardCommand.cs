using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Devices;
using AppService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppService.Application.Authorization.Commands
{
    public class AuthorizeCardCommand : IRequest<AuthorizeCardResult>
    {
        [Required]
        public string DeviceId { get; set; }

        public byte[] CardNo { get; set; }

        public string Pin { get; set; }

        public sealed class AuthorizeCardHandler : IRequestHandler<AuthorizeCardCommand, AuthorizeCardResult>
        {
            private readonly DeviceController _deviceController;
            private readonly AccessControlContext _context;

            public AuthorizeCardHandler(
                DeviceController deviceController,
                AccessControlContext context)
            {
                _deviceController = deviceController;
                _context = context;
            }

            public async Task<AuthorizeCardResult> Handle(AuthorizeCardCommand request, CancellationToken cancellationToken)
            {
                var credentials = await _context.CardCredentials.FirstOrDefaultAsync(x => x.Data.SequenceEqual(request.CardNo));
                if (credentials != null)
                {
                    await _deviceController.Disarm(request.DeviceId);
                }
                return new AuthorizeCardResult(credentials != null);
            }
        }
    }
}
