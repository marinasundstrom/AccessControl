using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AppService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AppService.Application.Registration.Commands
{
    public class RegisterCommand : IRequest<RegistrationResult>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegistrationResult>
        {
            private UserManager<User> _userManager;

            public RegisterCommandHandler(
                UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<RegistrationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                IdentityResult result = await _userManager.CreateAsync(new User()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Email,
                    Email = request.Email,
                }, request.Password);

                return new RegistrationResult()
                {
                    Succeeded = result.Succeeded,
                    Errors = result.Errors
                };
            }
        }
    }
}
