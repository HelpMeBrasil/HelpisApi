using Donation.Domain.Entities;
using Donation.Domain.Extensions;
using Donation.Infrastructure.Contract;
using Donation.Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Donation.Domain.Security
{
    public class IdentityManager : IIdentityProvider
    {
        private readonly AppSettings settings;
        private readonly IHttpContextAccessor accessor;

        public IdentityManager(IHttpContextAccessor accessor, IOptions<AppSettings> settings)
        {
            this.accessor = accessor;
            this.settings = settings.Value;
        }

        public Token SignIn(User user)
        {
            Token identity = new Token(user, settings.AccessExpiration);

            identity.AccessToken = CreateToken(user, identity);

            return identity;
        }

        private string CreateToken(User user, Token identity)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(settings.SecretKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Iat, identity.CreateDate.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
                }),
                Issuer = settings.Issuer,
                Audience = settings.Audience,
                Expires = identity.ExpiryDate,
                NotBefore = identity.CreateDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string Username()
        {
            ClaimsPrincipal principal = accessor.HttpContext.User;

            return principal.FindFirst(e => e.Type == ClaimTypes.Name).Value;
        }

        public string Id()
        {
            ClaimsPrincipal principal = accessor.HttpContext.User;

            return principal.FindFirst(e => e.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}