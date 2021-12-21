using FluentValidation;
using Donation.Domain.ValueObjects;
using Donation.Infrastructure.Models;
using System;

namespace Donation.API.Models.Recovery
{
    public class PasswordRecoveryModel
    {
        public Guid RecoveryId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }

    public class PasswordRecoveryModelValidator : AbstractValidator<PasswordRecoveryModel>
    {
        public PasswordRecoveryModelValidator()
        {
            RuleFor(x => x.RecoveryId).NotEmpty().NotEqual(Guid.Empty);

            RuleFor(x => x.Username).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty()
                                    .Must(x => Password.ValidatePasswordPattern(x))
                                    .WithMessage(ResourceHelper.GetResources("pt-BR").GetString("PasswordValidationMessage"));

            RuleFor(x => x.PasswordConfirmation).NotEmpty().Equal(e => e.Password);
        }
    }
}