using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Donation.API.Configuration
{
    public static class ValidatorConfiguration
    {
        public static void AddValidatorConfiguration(this IServiceCollection services)
        {
            // Fluent validation settings
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}