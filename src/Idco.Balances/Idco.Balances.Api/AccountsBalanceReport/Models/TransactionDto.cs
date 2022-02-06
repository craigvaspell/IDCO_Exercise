namespace Idco.Balances.Api.AccountBalanceReport
{
    using Idco.Balances.Domain.Common;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TransactionDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public long Amount { get; set; }

        [Required]
        [EnumDataType(typeof(CreditDebitIndicator))]
        public CreditDebitIndicator CreditDebitIndicator {get; set; }

        [Required]
        [EnumDataType(typeof(TransactionStatus))]
        public TransactionStatus Status { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        public MerchantDetailsDto MerchantDetails { get; set; }

        public TransactionDto()
        { }
    }
}