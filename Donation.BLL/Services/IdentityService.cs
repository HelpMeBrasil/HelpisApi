using Donation.Application.Contracts;
using Donation.Application.Transfers;
using Donation.Domain.Contract;
using Donation.Domain.Entities;
using Donation.Infrastructure.Contract;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Donation.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityProvider provider;
        private readonly IMongoRepository<User> repository;
        private readonly IMongoRepository<Token> tokenRepository;
        private readonly IUserDomainService domainService;
        private readonly ILogger<IdentityService> logger;

        public IdentityService(
            IIdentityProvider provider,
            IMongoRepository<User> repository,
            IMongoRepository<Token> tokenRepository,
            IUserDomainService domainService,
            ILogger<IdentityService> logger
            )
        {
            this.provider = provider;
            this.repository = repository;
            this.tokenRepository = tokenRepository;
            this.domainService = domainService;
            this.logger = logger;
        }

        public async Task<IdentityDTO> SignInAsync(AuthDTO auth)
        {
            // Find the user
            User user = await repository.FindOneAsync(e => e.Username == auth.Username);

            // Validate user signIn inside domain layer
            if (!domainService.ValidateUserSignIn(user, auth.Password))
            {
                logger.LogInformation($"The authentication was not allowed. | User: {auth.Username}.");

                return default;
            }

            // Return the data with access token to presentation
            // Create acccess token
            Token data = provider.SignIn(user);

            // Persist token
            await tokenRepository.InsertOneAsync(data);

            // Map it
            return data.Adapt<IdentityDTO>();
        }
    }
}