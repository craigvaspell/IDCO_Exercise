namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.BalanceReports;
    using Idco.Balances.Domain.Requests;
    using System;
    using System.Threading.Tasks;

    public class AccountsBalanceReportService : IAccountsBalanceReportService
    {
        private readonly IAccountBalanceReportService _accountsBalanceReportService;

        public AccountsBalanceReportService(IAccountBalanceReportService accountBalanceService)
            => _accountsBalanceReportService = accountBalanceService;

        public async Task<BalanceReport> GetEodBalanceReport(AccountsBalanceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
