using Donation.API.Filters;
using Donation.Application.Contracts;
using Donation.Application.Services;
using Donation.BLL.Contracts;
using Donation.BLL.Services;
using Donation.DAL.Model;
using Donation.Domain.Contract;
using Donation.Domain.Contracts;
using Donation.Domain.Notifications;
using Donation.Domain.Security;
using Donation.Domain.Services;
using Donation.Infrastructure.Contract;
using Donation.Infrastructure.Model;
using Donation.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Donation.API.Ioc
{
    public static class DependencyConfiguration
    {
        public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<Safe2paySettings>(configuration.GetSection("Safe2paySettings"));

            // Add infra layer dependencies
            services.AddScoped<IIdentityProvider, IdentityManager>();
            services.AddScoped<IEmailService, SendGridService>();
            services.AddScoped<IIdentityProvider, IdentityManager>();
            services.AddScoped<INotificationService, NotificationService>();

            // Add domain layer dependencies
            services.AddScoped<INotification, NotificationContext>();
            services.AddScoped<IUserDomainService, UserDomainService>();

            // Add appliation layer dependencies
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IManagementService, ManagementService>();
            services.AddScoped<IRecoveryService, RecoveryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISafe2payService, Safe2payService>();
            services.AddScoped<IDonationCampaignService, DonationCampaignService>();

            //Add filters
            services.AddMvc(options => options.Filters.Add<NotificationFilter>());
        }
    }
}