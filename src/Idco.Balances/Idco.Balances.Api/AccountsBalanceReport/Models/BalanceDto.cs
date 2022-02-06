namespace Idco.Balances.Api.AccountBalanceReport
{
    using Idco.Balances.Domain.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BalanceDto
    {
        [Required]
        public long Amount { get; set; }

        [Required]
        [EnumDataType(typeof(CreditDebitIndicator))]
        public CreditDebitIndicator CreditDebitIndicator { get; set; }

        public ICollection<CreditLineDto> CreditLines { get; set; }

        public BalanceDto()
        { }
    }
}