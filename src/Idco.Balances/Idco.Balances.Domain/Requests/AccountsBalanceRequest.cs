namespace Idco.Balances.Domain.Requests
{
    using Idco.Balances.Domain.Accounts;
    using System;
    using System.Collections.Generic;

    public class AccountsBalanceRequest
    {
        public string BrandName { get; }
        public string DataSourceName { get; }
        public string DataSourceType { get; }
        public DateTime RequestDateTime { get; }
        public ICollection<Account> Accounts { get; }

        public AccountsBalanceRequest(
            string brandName,
            string dataSourceName,
            string dataSourceType,
            DateTime requestDateTime,
            ICollection<Account> accounts)
        {
            BrandName = brandName;
            DataSourceName = dataSourceName;
            DataSourceType = dataSourceType;
            RequestDateTime = requestDateTime;
            Accounts = accounts;
        }
    }
}
