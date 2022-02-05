namespace Idco.Balances.Api.AccountBalanceReport
{
    using System;

    public class EodBalanceReportDto
    {
        public DateTime Date { get; set; }
        public long Balance { get; set; }

        public EodBalanceReportDto()
        { }
    }
}