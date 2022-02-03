namespace Idco.Balances.Domain.BalanceReports
{
    using System.Collections.Generic;

    public class BalanceReport
    {
        public long TotalCredits { get; }
        public long TotalDebits { get; }

        public ICollection<EndOfDayBalance> EndOfDayBalances { get; }

        public BalanceReport(
            long totalCredits,
            long totalDebits,
            ICollection<EndOfDayBalance> endOfDayBalances)
        {
            TotalCredits = totalCredits;
            TotalDebits = totalDebits;
            EndOfDayBalances = endOfDayBalances;
        }
    }
}
