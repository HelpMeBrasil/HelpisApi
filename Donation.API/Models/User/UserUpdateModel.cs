using FluentValidation;
using Newtonsoft.Json;
using Safe2Pay.Models;

namespace Donation.API.Models.User
{
    public class UserUpdateModel
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public Merchant merchant { get; set; }
    }

    public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
    {
        public UserUpdateModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Username).NotEmpty().EmailAddress();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}