using Donation.Application.Contracts;
using Donation.Application.Transfers;
using Donation.BLL.Contracts;
using Donation.Domain.Contracts;
using Donation.Domain.Entities;
using Donation.Domain.Enumerators;
using Donation.DTO.Entities;
using Donation.Infrastructure.Contract;
using Mapster;
using Microsoft.Extensions.Logging;
using Safe2Pay.Models;
using Safe2Pay.Response;
using System.Collections.Generic;
using System.Linq;

namespace Donation.Application.Services
{
    public class UserService : IUserService
    {
        private readonly INotification notification;
        private readonly ISafe2payService safe2pay;
        private readonly IMongoRepository<User> repository;
        private readonly ILogger<UserService> logger;

        public UserService(INotification notification, IMongoRepository<User> repository, ILogger<UserService> logger, ISafe2payService safe2pay)
        {
            this.notification = notification;
            this.repository = repository;
            this.logger = logger;
            this.safe2pay = safe2pay;
        }

        public UserDTO Get(string id)
        {
            User user = repository.FindById(id);

            if(user is null)
            {
                logger.LogWarning("User not found.");

                notification.AddNotification(MappedErrors.UserNotFound);

                return default;
            }


            if(user != null)
            {
                var response = safe2pay.GetSubAccount(id);
                user.Merchant = new Merchant {
                    BankData = new BankData
                    {
                        Bank = new Bank
                        {
                            Code = response.BankData.Bank
                        },
                        AccountType = new AccountType
                        {
                            Code = response.BankData.AccountType
                        },
                        BankAccount = response.BankData.Account,
                        BankAccountDigit = response.BankData.AccountDigit,
                        BankAgency = response.BankData.Agency,
                        BankAgencyDigit = response.BankData.AgencyDigit
                    },
                    Address = new Address
                    {
                        CityName = response.Address.City,
                        Complement = response.Address.Complement,
                        CountryName = response.Address.Country,
                        District = response.Address.District,
                        Number = response.Address.Number,
                        StateInitials = response.Address.State,
                        Street = response.Address.Street,
                        ZipCode = response.Address.ZipCode
                    },
                    Name = response.Name,
                    Identity = response.Identity,
                    Email = response.Email,
                    CommercialName = response.CommercialName,
                    IsPanelRestricted = true,
                    ResponsibleIdentity = response.ResponsibleIdentity,
                    ResponsibleName = response.ResponsibleName
                };

                var paymentMethods = new List<MerchantSplit>();

                foreach (PaymentMethodResponse merchantSplit in response.PaymentMethods)
                {
                    if(merchantSplit.PaymentMethod == "1")
                    {
                        var paymentMethod = new MerchantSplit
                        {
                            PaymentMethodCode = "1"
                        };
                        paymentMethods.Add(paymentMethod);
                    }
                    if (merchantSplit.PaymentMethod == "2")
                    {
                        var paymentMethod = new MerchantSplit
                        {
                            PaymentMethodCode = "2"
                        };
                        paymentMethods.Add(paymentMethod);
                    }
                    if (merchantSplit.PaymentMethod == "4")
                    {
                        var paymentMethod = new MerchantSplit
                        {
                            PaymentMethodCode = "4"
                        };
                        paymentMethods.Add(paymentMethod);
                    }
                    if (merchantSplit.PaymentMethod == "3")
                    {
                        var paymentMethod = new MerchantSplit
                        {
                            PaymentMethodCode = "3"
                        };
                        paymentMethods.Add(paymentMethod);
                    }
                    if (merchantSplit.PaymentMethod == "6")
                    {
                        var paymentMethod = new MerchantSplit
                        {
                            PaymentMethodCode = "6"
                        };
                        paymentMethods.Add(paymentMethod);
                    }
                }

                user.Merchant.MerchantSplit = paymentMethods;
            }

            return user.Adapt<UserDTO>();
        }

        public void Add(UserDTO user)
        {
            // Check if exists
            bool exists = repository.FilterBy(e => e.Username == user.Username || e.Email == user.Username).Any();

            if (exists)
            {
                logger.LogWarning($"User already exists. | User: {user.Username}.");

                notification.AddNotification(MappedErrors.UserAlreadyExists);

                return;
            }

            user.Merchant.Email = user.Email;
            user.Identity = user.Merchant.ResponsibleIdentity;

            var response = safe2pay.AddSubAccount(user.Merchant);

            if (response.Id > 0)
            {
                var merchantSafe2pay = new MerchantSafe2pay
                {
                    Id = response.Id,
                    Token = response.Token,
                    TokenSandbox = response.TokenSandbox,
                    SecretKey = response.SecretKey,
                    SecretKeySandbox = response.SecretKeySandbox
                };
                user.MerchantSafe2Pay = merchantSafe2pay;
            }
            else
            {
                logger.LogWarning($"User could not be registrated to safe2pay.");

                return;
            }
            // Create a new user instance
            User data = new User(user.FirstName, user.Surname, user.Username,
                                 user.Email, user.PhoneNumber, user.Password,
                                 user.Identity, user.MerchantSafe2Pay);

            // Persist it
            repository.InsertOne(data);
        }

        public void Update(UserDTO user, Merchant merchant)
        {
            // Retrieve user by id
            User data = repository.FindById(user.Id);

            if (data is null)
            {
                logger.LogWarning($"User not found. | User: {user.Id}.");

                notification.AddNotification(MappedErrors.UserNotFound);

                return;
            }



            //update safe2pay merchant config
            if (merchant != null)
            {
                user.Merchant = merchant;

                user.Merchant.Email = merchant.Email; 

                safe2pay.UpdateSubAccount(user.Merchant, user.Id);
            }

            // Update user data
            data.UpdateUser(user.FirstName, user.Surname, user.Username, user.Email, user.PhoneNumber, user.Active);

            // Persist it
            repository.ReplaceOne(data);
        }
    }
}