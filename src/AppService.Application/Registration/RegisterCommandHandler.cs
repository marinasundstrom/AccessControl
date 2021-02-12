using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AppService.Application.Registration
{
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
