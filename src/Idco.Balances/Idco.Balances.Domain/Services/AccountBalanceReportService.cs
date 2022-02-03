namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.Accounts;
    using Idco.Balances.Domain.BalanceReports;
    using System;
    using System.Threading.Tasks;

    public class AccountBalanceReportService : IAccountBalanceReportService
    {
        public async Task<BalanceReport> GetEodBalanceReport(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
