using FluentValidation;
using Donation.Domain.ValueObjects;
using Donation.Infrastructure.Models;

namespace Donation.API.Models.Management
{
    public class ChangePasswordModel
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string PasswordConfirmation { get; set; }
    }

    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().MinimumLength(8);

            RuleFor(x => x.Password).NotEmpty()
                                    .NotEqual(x => x.OldPassword)
                                    .Must(x => Password.ValidatePasswordPattern(x))
                                    .WithMessage(ResourceHelper.GetResources("pt-BR").GetString("PasswordValidationMessage"));

            RuleFor(x => x.PasswordConfirmation).NotEmpty().Equal(e => e.Password);
        }
    }
}