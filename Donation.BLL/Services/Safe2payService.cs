using Donation.BLL.Contracts;
using Donation.DAL.Model;
using Donation.Domain.Contracts;
using Donation.Domain.Entities;
using Donation.Domain.Enumerators;
using Donation.DTO.Entities;
using Donation.Infrastructure.Contract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safe2Pay;
using Safe2Pay.Models;
using Safe2Pay.Models.Payment;
using Safe2Pay.Response;
using System;
using System.Collections.Generic;

namespace Donation.BLL.Services
{
    public class Safe2payService : ISafe2payService
    {
        private readonly INotification notification;
        private readonly IMongoRepository<User> userRepository;
        private readonly IMongoRepository<DonationCampaign> CampaignRepository;
        private readonly Safe2paySettings safe2paySettings;
        private readonly ILogger<Safe2payService> logger;


        public Safe2payService(IOptions<Safe2paySettings> safe2paySettings, IMongoRepository<DonationCampaign> CampaignRepository, ILogger<Safe2payService> logger, IMongoRepository<User> userRepository, INotification notification)
        {
            this.safe2paySettings = safe2paySettings.Value;
            this.userRepository = userRepository;
            this.CampaignRepository = CampaignRepository;
            this.logger = logger;
            this.notification = notification;
        }

        #region Marketplace
        public MarketplaceResponse AddSubAccount(Merchant merchant)
        {
            var merchansplit = new List<MerchantSplit>
                {
                    new MerchantSplit
                    {
                        PaymentMethodCode = "1",
                        IsSubaccountTaxPayer = true,
                        Taxes = new List<MerchantSplitTax>
                        {
                            new MerchantSplitTax { Tax = 20m, TaxTypeName = EnumTaxType.Amount }
                        }
                    },
                    new MerchantSplit
                    {
                        PaymentMethodCode = "2",
                        IsSubaccountTaxPayer = true,
                        Taxes = new List<MerchantSplitTax>
                        {
                            new MerchantSplitTax { Tax = 2m, TaxTypeName = EnumTaxType.Percentage }
                        }
                    },
                    new MerchantSplit
                    {
                        PaymentMethodCode = "3",
                        IsSubaccountTaxPayer = true,
                        Taxes = new List<MerchantSplitTax>
                        {
                            new MerchantSplitTax { Tax = 3m, TaxTypeName = EnumTaxType.Percentage }
                        }
                    },
                    new MerchantSplit
                    {
                        PaymentMethodCode = "4",
                        IsSubaccountTaxPayer = true,
                        Taxes = new List<MerchantSplitTax>
                        {
                            new MerchantSplitTax { Tax = 4m, TaxTypeName = EnumTaxType.Percentage }
                        }
                    },
                    new MerchantSplit
                    {
                        PaymentMethodCode = "6",
                        IsSubaccountTaxPayer = true,
                        Taxes = new List<MerchantSplitTax>
                        {
                            new MerchantSplitTax { Tax = 1m, TaxTypeName = EnumTaxType.Percentage }
                        }
                    }
            };

            merchant.IsPanelRestricted = true;

            merchant.MerchantSplit = merchansplit;

            Safe2Pay_Request request = new(safe2paySettings.Token);

            var response = request.Marketplace.New(merchant);

            if (response.Id > 0)
                return response;
            else
                return null;
        }
        public MarketplaceResponse UpdateSubAccount(Merchant merchant, string idUser)
        {
            User user = userRepository.FindById(idUser);

            Safe2Pay_Request request = new(safe2paySettings.Token);

            MarketplaceResponse response = request.Marketplace.Update(merchant, user.MerchantSafe2Pay.Id);

            return response;
        }
        public MarketplaceResponse GetSubAccount(string idUser)
        {
            User user = userRepository.FindById(idUser);

            Safe2Pay_Request request = new(safe2paySettings.Token);

            MarketplaceResponse response = request.Marketplace.Get(user.MerchantSafe2Pay.Id);

            response.Integration = null;

            return response;
        }
        public bool DeleteSubAccount(string idUser)
        {
            User user = userRepository.FindById(idUser);

            Safe2Pay_Request request = new(safe2paySettings.Token);

            bool safe2payResponse = request.Marketplace.Delete(Convert.ToInt32(user.MerchantSafe2Pay.Id));

            return safe2payResponse;
        }

        public List<MerchantPaymentMethodResponse> GetPaymentMethods(string hash)
        {
            DonationCampaign data = CampaignRepository.FindOne(data => data.Hash == hash);

            data.User = userRepository.FindOne(e => e.Username == data.User.Username);

            var safe2PayRequest = new Safe2Pay_Request(data.User.MerchantSafe2Pay.Token);

            var response = safe2PayRequest.merchantPaymentMethod.List();

            return response;
        }
        #endregion

        #region Transações
        public TransactionResponse GetTransaction(int idTransaction, string idUser)
        {
            User user = userRepository.FindById(idUser);

            var safe2PayRequest = new Safe2Pay_Request(user.MerchantSafe2Pay.TokenSandbox, "");

            var response = safe2PayRequest.Transaction.Detail(idTransaction);

            return response;
        }
        public List<TransactionResponse> ListTransaction(string idUser, int rowsPerPage, int pageNumber)
        {
            User user = userRepository.FindById(idUser);

            var safe2PayRequest = new Safe2Pay_Request(user.MerchantSafe2Pay.TokenSandbox, "");

            var response = safe2PayRequest.Transaction.List(pageNumber, rowsPerPage, true);

            return response;
        }
        public decimal ListTransactionByReference(string hash)
        {
            DonationCampaign data = CampaignRepository.FindOne(data => data.Hash == hash && data.IsActive == true);

            if (data is null)
            {
                logger.LogWarning("Campaign not found or is inactive");

                notification.AddNotification(MappedErrors.CampaigNotFound);

                return default;
            }

            data.User = userRepository.FindOne(e => e.Username == data.User.Username);

            var safe2PayRequest = new Safe2Pay_Request(data.User.MerchantSafe2Pay.TokenSandbox, "");

            var response = safe2PayRequest.Transaction.ListByReference(hash.ToString());

            decimal currentAmount = 0m;

            foreach (TransactionResponse transactionResponse in response)
            {
                if (transactionResponse.Status == "3")
                {
                    currentAmount += transactionResponse.Amount;
                }
            }

            return currentAmount;
        }
        public BankSlipResponse AddTransactionBankSlip(Transaction<BankSlip> transaction, string hash)
        {
            DonationCampaign data = CampaignRepository.FindOne(data => data.Hash == hash);

            data.User = userRepository.FindOne(e => e.Username == data.User.Username);

            var safe2PayRequest = new Safe2Pay_Request(data.User.MerchantSafe2Pay.TokenSandbox, "");

            transaction.Reference = hash.ToString();
            transaction.PaymentObject = new BankSlip
            {
                CancelAfterDue = true,
                DaysBeforeCancel = 0,
                DueDate = DateTime.UtcNow.Date.AddDays(3),
                IsEnablePartialPayment = false,
                Message = new List<string> { "Doação campanha" + data.Title },
                Instruction = "Pagável em qualquer banco ou lotérica até o vencimento"
            };

            var response = safe2PayRequest.Payment.BankSlip(transaction);

            return response;
        }
        public CreditCardResponse AddTransactionCredit(Transaction<CreditCard> transaction, string hash)
        {
            DonationCampaign data = CampaignRepository.FindOne(data => data.Hash == hash);

            data.User = userRepository.FindOne(e => e.Username == data.User.Username);

            var safe2PayRequest = new Safe2Pay_Request(data.User.MerchantSafe2Pay.TokenSandbox, "");

            transaction.Reference = hash.ToString();
            transaction.PaymentObject.InstallmentQuantity = 1;
            transaction.PaymentObject.IsPreAuthorization = false;
            transaction.PaymentObject.IsApplyInterest = false;
            transaction.PaymentObject.SoftDescriptor = "Helpis";

            var response = safe2PayRequest.Payment.Credit(transaction);

            return response;
        }
        public DebitCardResponse AddTransactionDebit(Transaction<DebitCard> transaction, string hash)
        {
            DonationCampaign data = CampaignRepository.FindOne(data => data.Hash == hash);

            data.User = userRepository.FindOne(x => x.Username == data.User.Username);

            var safe2PayRequest = new Safe2Pay_Request(data.User.MerchantSafe2Pay.TokenSandbox, "");

            transaction.Reference = hash.ToString();
            transaction.PaymentObject.SoftDescriptor = "Helpis";
            transaction.PaymentObject.ReturnUrl = "https://helpisdonation.herokuapp.com/";

            var response = safe2PayRequest.Payment.Debit(transaction);

            return response;
        }
        public PixResponse AddTransactionPix(Transaction<Pix> transaction, string hash)
        {
            DonationCampaign data = CampaignRepository.FindOne(data => data.Hash == hash);

            data.User = userRepository.FindOne(e => e.Username == data.User.Username);

            var safe2PayRequest = new Safe2Pay_Request(data.User.MerchantSafe2Pay.Token, "");

            transaction.Reference = hash.ToString();

            transaction.PaymentObject = new Pix
            {
                Expiration = 86400
            };

            transaction.IsSandbox = false;

            var response = safe2PayRequest.Payment.Pix(transaction);

            return response;
        }
        public CryptoCurrencyResponse AddTransactionCripto(Transaction<CryptoCoin> transaction, string hash)
        {
            DonationCampaign data = CampaignRepository.FindOne(data => data.Hash == hash);

            data.User = userRepository.FindOne(e => e.Username == data.User.Username);

            var safe2PayRequest = new Safe2Pay_Request(data.User.MerchantSafe2Pay.TokenSandbox, "");

            transaction.Reference = hash.ToString();

            transaction.PaymentObject = new CryptoCoin
            {
                Symbol = "BTC"
            };

            var response = safe2PayRequest.Payment.CryptoCurrency(transaction);

            return response;
        }
        #endregion
    }
}
