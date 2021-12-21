using Safe2Pay.Models;
using Safe2Pay.Models.Payment;
using Safe2Pay.Response;
using System;
using System.Collections.Generic;

namespace Donation.BLL.Contracts
{
    public interface ISafe2payService
    {
        public MarketplaceResponse AddSubAccount(Merchant merchant);
        public MarketplaceResponse UpdateSubAccount(Merchant merchant, string idUser);
        public MarketplaceResponse GetSubAccount(string idUser);
        public bool DeleteSubAccount(string idUser);
        public List<MerchantPaymentMethodResponse> GetPaymentMethods(string hash);
        public TransactionResponse GetTransaction(int idTransaction, string idUser);
        public List<TransactionResponse> ListTransaction(string idUser, int rowsPerPage, int pageNumber);
        public decimal ListTransactionByReference(string hash);
        public BankSlipResponse AddTransactionBankSlip(Transaction<BankSlip> transaction, string hash);
        public CreditCardResponse AddTransactionCredit(Transaction<CreditCard> transaction, string hash);
        public DebitCardResponse AddTransactionDebit(Transaction<DebitCard> transaction, string hash);
        public PixResponse AddTransactionPix(Transaction<Pix> transaction, string hash);
        public CryptoCurrencyResponse AddTransactionCripto(Transaction<CryptoCoin> transaction, string hash);

    }
}
