namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.BalanceReports;
    using Idco.Balances.Domain.Requests;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountsBalanceReportService : IAccountsBalanceReportService
    {
        private readonly IAccountBalanceReportService _accountsBalanceReportService;
        private readonly ILogger<AccountsBalanceReportService> _logger;

        public AccountsBalanceReportService(
            IAccountBalanceReportService accountBalanceService,
            ILogger<AccountsBalanceReportService> logger)
        {
            _accountsBalanceReportService = accountBalanceService;
            _logger = logger;
        }

        public async Task<EodBalanceListReport> GetEodBalanceReport(AccountsBalanceRequest request)
        {
            try
            {
                var eodBalanceLists = new List<EodBalanceListReport>();
                foreach (var account in request.Accounts)
                {
                    var accountEodBalances = await _accountsBalanceReportService.GetEodBalanceReport(account);
                    eodBalanceLists.Add(accountEodBalances);
                }

                return await CombineAccountBalanceReports(eodBalanceLists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to calculate balance report");
                throw;
            }
        }

        /// <summary>
        /// Given a list of account balance reports, combine them to get an aggregate report across all accounts
        /// </summary>
        private Task<EodBalanceListReport> CombineAccountBalanceReports(IEnumerable<EodBalanceListReport> eodBalances)
        {
            var perDayAggregateReports = eodBalances
                .SelectMany(eodbl => eodbl.Balances)
                .GroupBy(
                    eodb => eodb.Date.Date,
                    eodb => eodb,
                    (date, dayBalanceReports) =>
                    {
                        long rollingBalance = 0L, rollingCredits = 0L, rollingDebits = 0L;

                        foreach(var balanceReport in dayBalanceReports)
                        {
                            rollingBalance += balanceReport.Balance;
                            rollingCredits += balanceReport.TotalCredits;
                            rollingDebits += balanceReport.TotalDebits;
                        }

                        return new EodBalanceReport(date, rollingBalance, rollingCredits, rollingDebits);
                    });

            return Task.FromResult(new EodBalanceListReport(perDayAggregateReports));
        }
    }
}
