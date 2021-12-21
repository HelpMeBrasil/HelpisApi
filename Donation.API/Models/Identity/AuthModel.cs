using FluentValidation;

namespace Donation.API.Models.Identity
{
    public class AuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthModelValidator : AbstractValidator<AuthModel>
    {
        public AuthModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}