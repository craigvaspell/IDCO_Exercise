namespace Idco.Balances.Domain.BalanceReports
{
    using System;

    public class EodBalanceReport

    {
        public DateTime Date { get; }
        public long TotalCredits { get; }
        public long TotalDebits { get; }

        public long Balance { get; }

        public EodBalanceReport(
            DateTime date,
            long balance,
            long totalCredits,
            long totalDebits
        )
        {
            Date = date;
            Balance = balance;
            TotalCredits = totalCredits;
            TotalDebits = totalDebits;
        }
    }
}