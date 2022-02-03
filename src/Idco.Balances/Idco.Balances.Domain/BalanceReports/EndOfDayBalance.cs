namespace Idco.Balances.Domain.BalanceReports
{
    using System;

    public class EndOfDayBalance
    {
        public DateTime Date { get; }
        public long Balance { get; }

        public EndOfDayBalance(
            DateTime date,
            long balance
        )
        {
            Date = date;
            Balance = balance;
        }
    }
}