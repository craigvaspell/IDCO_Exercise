namespace Idco.Balances.Domain.Accounts
{
    using Idco.Balances.Domain.Common;
    using System;

    public class Transaction
    {
        public string Description { get; set; }
        public long Amount { get; set; }
        public CreditDebitIndicator CreditDebitIndicator { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime BookingDate { get; set; }
        public MerchantDetails MerchantDetails { get; set; }

        public long SignedAmount => CreditDebitIndicator == CreditDebitIndicator.Credit
            ? Amount
            : Amount * -1;

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
