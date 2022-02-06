namespace Idco.Balances.Api.AccountBalanceReport
{
    using System.ComponentModel.DataAnnotations;

    public class IdentifiersDto
    {
        [Required]
        public string SortCode { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        public string SecondaryIdentification { get; set; }

        public IdentifiersDto()
        { }
    }
}