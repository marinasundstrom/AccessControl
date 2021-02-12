using System.ComponentModel.DataAnnotations;
using MediatR;

namespace AccessControl.AppService.Application.Login
{
    public class AuthCommand : IRequest<LoginResult>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
