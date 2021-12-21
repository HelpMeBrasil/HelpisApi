using Donation.Domain.Helpers;
using Donation.Domain.ValueObjects;
using Donation.DTO.Entities;
using Safe2Pay.Models;
using System.Collections.Generic;

namespace Donation.Domain.Entities
{
    [BsonCollection("User")]
    public class User : Entity
    {
        public User()
        {
            Recoveries = new List<Recovery>();
        }

        public User(string firstName, string surname,
                    string username, string email,
                    string phoneNumber, string password,
                    string identity, MerchantSafe2pay merchantsafe2pay)
        {
            Surname = surname;
            FirstName = firstName;
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = new Password(password);
            Recoveries = new List<Recovery>();
            Active = true;
            Identity = identity;
            MerchantSafe2Pay = merchantsafe2pay;
        }

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Identity { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        public bool Active { get; set; }
        public bool Blocked { get; set; }   
        public Password Password { get; set; }
        public List<Recovery> Recoveries { get; set; }
        public MerchantSafe2pay MerchantSafe2Pay { get; set; }
        public Merchant Merchant { get; set; }

        public void UpdateUser(string firstName, string surname, string username, string email, string phoneNumber, bool active)
        {
            FirstName = firstName;
            Surname = surname;
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            Active = active;
        }

        public Recovery GenerateRecoveryCode()
        {
            // Clear all recoveries previous recovery codes
            Recoveries.Clear();

            // Generate a new one
            Recovery recovery = new Recovery();

            // Add a new one
            Recoveries.Add(recovery);

            return recovery;
        }
    }
}