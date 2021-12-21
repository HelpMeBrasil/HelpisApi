namespace Donation.Application.Transfers
{
    public class AuthDTO
    {
        public AuthDTO()
        { }

        public AuthDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}