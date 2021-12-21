using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Donation.API.Configuration
{
    public static class AuthorizationConfiguration
    {
        public static void AddAuthorizationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            byte[] key = Encoding.ASCII.GetBytes(configuration["AppSettings:SecretKey"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = configuration["AppSettings:Issuer"],
                    ValidAudience = configuration["AppSettings:Audience"],
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }

        public static void UseAuthorizationSettings(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}