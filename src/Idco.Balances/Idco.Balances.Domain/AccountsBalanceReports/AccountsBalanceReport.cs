namespace Idco.Balances.Domain.AccountsBalanceReports
{
    using System.Collections.Generic;

    public class AccountsBalanceReport
    {
        public long TotalCredits { get; }
        public long TotalDebits { get; }

        public ICollection<EndOfDayBalance> EndOfDayBalances { get; }

        public AccountsBalanceReport(
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
