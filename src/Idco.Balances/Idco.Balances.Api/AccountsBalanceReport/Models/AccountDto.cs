namespace Idco.Balances.Api.AccountBalanceReport
{
    using Idco.Balances.Domain.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AccountDto
    {
        [Required]
        public string AccountId { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EnumDataType(typeof(AccountType))]
        public AccountType AccountType { get; set; }

        [Required]
        [EnumDataType(typeof(AccountSubType))]
        public AccountSubType AccountSubType { get; set; }

        [Required]
        public IdentifiersDto Identifiers { get; set; }

        public ICollection<PartyDto> Parties { get; set; }

        public ICollection<StandingOrderDto> StandingOrders { get; set; }

        public ICollection<DirectDebitDto> DirectDebits { get; set; }

        [Required]
        public BalancesDto Balances { get; set; }

        [Required]
        public ICollection<TransactionDto> Transactions { get; set; }

        public AccountDto()
        { }
    }
}
