namespace Idco.Balances.Api.AccountBalanceReport
{
    using System.Collections.Generic;

    public class EodBalanceListReportDto
    {
        public long TotalCredits { get; set; }
        public long TotalDebits { get; set; }
        public ICollection<EodBalanceReportDto> EndOfDayBalances { get; set; }

        public EodBalanceListReportDto()
        { }
    }
}