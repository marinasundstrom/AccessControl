using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppService.Application.Services;
using AppService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AppService.Application.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<AuthCommand, LoginResult>
    {
        private IConfiguration _configuration;
        private IJwtTokenService _tokenService;
        private UserManager<User> _userManager;

        public LoginCommandHandler(
            IConfiguration configuration,
            IJwtTokenService tokenService,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<LoginResult> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);
            bool validCredentials = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!validCredentials)
            {
                throw new InvalidCredentialException("Username or password is incorrect");
            }

            string newJwtToken = _tokenService.BuildToken($"{user.FirstName} {user.LastName}", request.Email);
            string newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new LoginResult
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
