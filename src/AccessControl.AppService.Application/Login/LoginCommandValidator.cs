using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace AccessControl.AppService.Application.Login
{
    public class LoginCommandValidator : AbstractValidator<AuthCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
             .NotEmpty()
             .WithMessage($"{nameof(AuthCommand.Email)} is required.");

            RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage($"{nameof(AuthCommand.Password)} is required.");
        }
    }
}
