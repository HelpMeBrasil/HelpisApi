using Donation.Domain.Enumerators;
using System;

namespace Donation.Application.DataTransfer
{
    public class ChangePasswordDTO
    {
        public ChangePasswordDTO()
        { }

        public ChangePasswordDTO(string password, string oldPassword, string username)
        {
            Password = password;
            OldPassword = oldPassword;
            Username = username;
        }

        public Guid RecoveryId { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string Username { get; set; }
        public string ConfirmationCode { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}