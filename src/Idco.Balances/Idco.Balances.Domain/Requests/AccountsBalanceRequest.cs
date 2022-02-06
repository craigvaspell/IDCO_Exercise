namespace Idco.Balances.Domain.Requests
{
    using Idco.Balances.Domain.Accounts;
    using System;
    using System.Collections.Generic;

    public class AccountsBalanceRequest
    {
        public string BrandName { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceType { get; set; }
        public DateTime RequestDateTime { get; set; }
        public ICollection<Account> Accounts { get; set; }


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
