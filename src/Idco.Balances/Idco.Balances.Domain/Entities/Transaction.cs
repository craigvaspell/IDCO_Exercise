namespace Idco.Balances.Domain.Entities
{
    using Idco.Balances.Domain.Common;
    using System;

    public class Transaction
    {
        public string Description { get; }
        public long Amount { get; set; }
        public CreditDebitIndicator CreditDebitIndicator { get; }
        public TransactionStatus Status { get; set; }
        public DateTime BookingDate { get; }
        public MerchantDetails MerchantDetails { get; }

        public Transaction(
            string description,
            long amount,
            CreditDebitIndicator signage,
            TransactionStatus transactionStatus,
            DateTime bookingDate,
            MerchantDetails merchantDetails)
        {
            Description = description;
            Amount = amount;
            CreditDebitIndicator = signage;
            Status = transactionStatus;
            BookingDate = bookingDate;
            MerchantDetails = merchantDetails;
        }
    }
}
