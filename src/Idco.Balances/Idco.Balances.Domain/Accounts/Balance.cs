namespace Idco.Balances.Domain.Accounts
{
    using Idco.Balances.Domain.Common;
    using System.Collections.Generic;

    public class Balance
    {
        public long Amount { get; set; }
        public CreditDebitIndicator CreditDebitIndicator { get; set; }
        public IEnumerable<CreditLine> CreditLines { get; set; }

        public long SignedAmount => CreditDebitIndicator == CreditDebitIndicator.Credit
            ? Amount
            : Amount * -1;

        public Balance(
            long amount,
            CreditDebitIndicator creditDebitIndicator,
            IEnumerable<CreditLine> creditLines)
        {
            Amount = amount;
            CreditDebitIndicator = creditDebitIndicator;
            CreditLines = creditLines;
        }
    }
}