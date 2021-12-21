using Safe2Pay.Response;
using System.Collections.Generic;

namespace Donation.DTO.Entities
{
    public class MerchantSafe2pay
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string TokenSandbox { get; set; }
        public string SecretKey { get; set; }
        public string SecretKeySandbox { get; set; }
    }
}
