using Donation.DTO.Entities;
using Safe2Pay.Models;
using System.Text.Json.Serialization;

namespace Donation.Application.Transfers
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Identity { get; set; }
        public bool Active { get; set; }
        public bool Blocked { get; set; }
        public bool Trustable { get; set; }
        public MerchantSafe2pay MerchantSafe2Pay { get; set; }
        public Merchant Merchant { get; set; }


        [JsonIgnore]
        public string Password { get; set; }
    }
}