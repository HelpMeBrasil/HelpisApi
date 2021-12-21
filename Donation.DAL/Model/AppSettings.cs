namespace Donation.Infrastructure.Model
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; }
        public int RefreshExpiration { get; set; }
        public int TokenExpirationTime { get; set; }
        public int RefreshExpirationTime { get; set; }
    }
}