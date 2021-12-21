using System;
using System.Text.RegularExpressions;

namespace Donation.Domain.ValueObjects
{
    public class Password
    {
        public Password()
        { }

        public Password(string password)
        {
            if (!ValidatePasswordPattern(password))
                throw new ArgumentException("The password doesnt match with the security patterns.");

            GeneratePassword(password);
        }

        public string Salt { get; set; }
        public string Hash { get; set; }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, this.Hash);
        }

        private void GeneratePassword(string password)
        {
            Salt = BCrypt.Net.BCrypt.GenerateSalt();

            Hash = BCrypt.Net.BCrypt.HashPassword(password, Salt);
        }

        /// <summary>
        /// The string must contain at least 1 lowercase alphabetical character
        /// The string must contain at least 1 uppercase alphabetical character
        /// The string must contain at least 1 numeric character
        /// The string must contain at least one special character
        /// The string must be eight characters or longer
        /// </summary>
        /// <param name="password"></param>
        /// <returns>bool</returns>
        public static bool ValidatePasswordPattern(string password)
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");

                return regex.IsMatch(password);
            }

            return false;
        }
    }
}