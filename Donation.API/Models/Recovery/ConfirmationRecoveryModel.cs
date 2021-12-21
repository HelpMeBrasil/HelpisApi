using FluentValidation;

namespace Donation.API.Models.Recovery
{
    public class ConfirmationRecoveryModel
    {
        public string Username { get; set; }
        public string ConfirmationCode { get; set; }
    }

    public class ConfirmationRecoveryModelValidator : AbstractValidator<ConfirmationRecoveryModel>
    {
        public ConfirmationRecoveryModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().EmailAddress();
            RuleFor(x => x.ConfirmationCode).NotEmpty().Length(6);
        }
    }
}