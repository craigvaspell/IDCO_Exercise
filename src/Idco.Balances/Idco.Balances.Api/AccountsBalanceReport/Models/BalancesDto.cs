namespace Idco.Balances.Api.AccountBalanceReport
{
    using System.ComponentModel.DataAnnotations;

    public class BalancesDto
    {
        [Required]
        public BalanceDto Current { get; set; }

        [Required]
        public BalanceDto Available { get; set; }

        public BalancesDto()
        { }
    }
}