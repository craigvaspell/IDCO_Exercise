namespace Idco.Balances.Domain.Accounts
{
    using Idco.Balances.Domain.Common;
    using System.Collections.Generic;

    public class Balance
    {
        public BalanceType BalanceType { get; }
        public long Amount { get; }
        public CreditDebitIndicator CreditDebitIndicator { get; }
        public IEnumerable<CreditLine> CreditLines { get; }

        public Balance(
            BalanceType balanceType,
            long amount,
            CreditDebitIndicator creditDebitIndicator,
            IEnumerable<CreditLine> creditLines)
        {
            BalanceType = balanceType;
            Amount = amount;
            CreditDebitIndicator = creditDebitIndicator;
            CreditLines = creditLines;
        }
    }
}