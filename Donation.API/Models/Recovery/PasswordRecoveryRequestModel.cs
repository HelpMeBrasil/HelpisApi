using FluentValidation;
using Donation.Domain.Enumerators;

namespace Donation.API.Models.Recovery
{
    public class PasswordRecoveryRequestModel
    {
        public string Username { get; set; }
        public NotificationType NotificationType { get; set; }
    }

    public class PasswordRecoveryRequestModelValidator : AbstractValidator<PasswordRecoveryRequestModel>
    {
        public PasswordRecoveryRequestModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().EmailAddress();
            RuleFor(x => x.NotificationType).NotEmpty().IsInEnum();
        }
    }
}