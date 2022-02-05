namespace Idco.Balances.Domain.Accounts
{
    using Idco.Balances.Domain.Common;
    using System.Collections.Generic;

    public class Account
    {
        public string AccountId { get; set; }
        public string CurrencyCode { get; set; }
        public string DisplayName { get; set; }
        public AccountType AccountType { get; set; }
        public AccountSubType AccountSubType { get; set; }
        public Identifiers Identifiers { get; set; }
        public IEnumerable<Party> Parties { get; set; }
        public IEnumerable<StandingOrder> StandingOrders { get; set; }
        public IEnumerable<DirectDebit> DirectDebits { get; set; }
        public Balances Balances { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }

        public Account(
            string accountId,
            string currencyCode,
            string displayName,
            AccountType accountType,
            AccountSubType accountSubType,
            Identifiers identifiers,
            IEnumerable<Party> parties,
            IEnumerable<DirectDebit> directDebits,
            Balances balances,
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
