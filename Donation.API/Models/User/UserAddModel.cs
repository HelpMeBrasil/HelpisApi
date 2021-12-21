using Donation.Domain.ValueObjects;
using Donation.Infrastructure.Models;
using FluentValidation;
using Safe2Pay.Models;

namespace Donation.API.Models.User
{
    public class UserAddModel
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Identity { get; set; }
        public Merchant Merchant { get; set; }
    }

    public class UserAddModelValidator : AbstractValidator<UserAddModel>
    {
        public UserAddModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Username).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Password).NotEmpty()
                                    .Must(x => Password.ValidatePasswordPattern(x))
                                    .WithMessage(ResourceHelper.GetResources("pt-BR").GetString("PasswordValidationMessage"));
            RuleFor(x => x.Merchant).NotEmpty();
        }
    }
}