using Donation.Domain.Entities;
using Donation.Domain.Helpers;
using Donation.Infrastructure.Context;
using Donation.Infrastructure.Contract;
using Donation.Infrastructure.Model;
using Donation.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace Donation.API.Configuration
{
    public static class DataContextConfiguration
    {
        public static void AddDataContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind settings
            KeySettings settings = new KeySettings();
            configuration.GetSection("KeySettings").Bind(settings);

            AppSettings appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);

            // Configure database
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            services.AddSingleton(database);

            // Add infrastructure layer dependencies
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoContext<>));
        }
    }
}