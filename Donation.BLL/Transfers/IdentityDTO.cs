using System;

namespace Donation.Application.Transfers
{
    public class IdentityDTO
    {
        public string AccessToken { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}