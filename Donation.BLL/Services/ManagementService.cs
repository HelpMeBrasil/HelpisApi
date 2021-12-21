using Donation.Application.Contracts;
using Donation.Application.DataTransfer;
using Donation.Domain.Contract;
using Donation.Domain.Entities;
using Donation.Infrastructure.Contract;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Donation.Application.Services
{
    public class ManagementService : IManagementService
    {
        private readonly IMongoRepository<User> repository;
        private readonly IUserDomainService domainService;
        private readonly ILogger<ManagementService> logger;

        public ManagementService(IMongoRepository<User> repository, IUserDomainService domainService, ILogger<ManagementService> logger)
        {
            this.repository = repository;
            this.domainService = domainService;
            this.logger = logger;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO model)
        {
            User user = await repository.FindOneAsync(e => e.Username == model.Username);

            if (domainService.UpdateUserPassword(user, model.OldPassword, model.Password))
            {
                await repository.ReplaceOneAsync(user);

                return true;
            }

            logger.LogInformation($"The change password failed. | User: {model.Username}.");

            return false;
        }
    }
}