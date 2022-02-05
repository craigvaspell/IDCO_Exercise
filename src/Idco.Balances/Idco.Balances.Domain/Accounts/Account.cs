namespace Idco.Balances.Domain.Accounts
{
    using Idco.Balances.Domain.Common;
    using System.Collections.Generic;

    public class Account
    {
        public string AccountId { get; }
        public string CurrencyCode { get; }
        public string DisplayName { get; }
        public AccountType AccountType { get; }
        public AccountSubType AccountSubType { get; }
        public IDictionary<string, string> Identifiers { get; }
        public IEnumerable<Party> Parties { get; set; }
        public IEnumerable<StandingOrder> StandingOrders { get; set; }
        public IEnumerable<DirectDebit> DirectDebits { get; set; }
        public IEnumerable<Balance> Balances { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }

        public Account(
            string accountId,
            string currencyCode,
            string displayName,
            AccountType accountType,
            AccountSubType accountSubType,
            IDictionary<string, string> identifiers,
            IEnumerable<Party> parties,
            IEnumerable<DirectDebit> directDebits,
            IEnumerable<Balance> balances,
            IEnumerable<Transaction> transactions)
        {
            AccountId = accountId;
            CurrencyCode = currencyCode;
            DisplayName = displayName;
            AccountType = accountType;
            AccountSubType = accountSubType;
            Identifiers = identifiers;
            Parties = parties;
            DirectDebits = directDebits;
            Balances = balances;
            Transactions = transactions;
        }
    }
}
