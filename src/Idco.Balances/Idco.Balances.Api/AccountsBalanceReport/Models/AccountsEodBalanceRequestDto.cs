namespace Idco.Balances.Api.AccountBalanceReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AccountsEodBalanceRequestDto
    {
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string DataSourceName { get; set; }
        [Required]
        public string DataSourceType { get; set; }
        [Required]
        public DateTime RequestDateTime { get; set; }
        [Required]
        public ICollection<AccountDto> Accounts { get; set; }

        public AccountsEodBalanceRequestDto()
        { }
    }
}