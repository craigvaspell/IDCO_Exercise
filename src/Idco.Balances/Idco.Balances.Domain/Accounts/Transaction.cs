namespace Idco.Balances.Domain.Accounts
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
            CreditDebitIndicator creditDebitIndicator,
            TransactionStatus status,
            DateTime bookingDate,
            MerchantDetails merchantDetails)
        {
            Description = description;
            Amount = amount;
            CreditDebitIndicator = creditDebitIndicator;
            Status = status;
            BookingDate = bookingDate;
            MerchantDetails = merchantDetails;
        }
    }
}
