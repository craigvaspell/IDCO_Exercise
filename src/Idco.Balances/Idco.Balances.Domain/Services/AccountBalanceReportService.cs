namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.Accounts;
    using Idco.Balances.Domain.BalanceReports;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DayTxnGrps = System.Collections.Generic.IEnumerable<(
        System.DateTime Date,
        System.Collections.Generic.IEnumerable<Accounts.Transaction> Txns)>;

    public class AccountBalanceReportService : IAccountBalanceReportService
    {
        private readonly ILogger<AccountBalanceReportService> _logger;

        public AccountBalanceReportService(
            ILogger<AccountBalanceReportService> logger)
        {
            _logger = logger;
        }

        public async Task<EodBalanceListReport> GetEodBalanceReport(Account account)
        {
            try
            {
                var initialBalance = account.Balances.Current
                    ?? throw new ArgumentException("Account has no indicated current balance.");

                if (account.Transactions == null || !account.Transactions.Any())
                    return EodBalanceListReport.EmptyBalanceListReport();

                var dayTxnGrps = await GetDayTxnGrps(account.Transactions);
                var eodBalances = await GetEodBalances(initialBalance.SignedAmount, dayTxnGrps);

                return new EodBalanceListReport(eodBalances.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to calculate balance report");
                throw;
            }
        }

        private Task<DayTxnGrps> GetDayTxnGrps(IEnumerable<Transaction> txns)
        {
            var dayTxnGrps = txns.GroupBy(
                txn => txn.BookingDate.Date,
                txn => txn,
                (Date, Txns) => (Date, Txns));

            return Task.FromResult(dayTxnGrps);
        }

        private Task<IEnumerable<EodBalanceReport>> GetEodBalances(long finalBalance, DayTxnGrps dayTxnGrps)
        {
            var rollingBalance = finalBalance;
            var eodBalances = dayTxnGrps
                .OrderByDescending(dayTxns => dayTxns.Date)
                .Select(dayTxns =>
                {
                    long totalCredits = 0, totalDebits = 0;

                    foreach(var txn in dayTxns.Txns)
                    {
                        switch (txn.CreditDebitIndicator)
                        {
                            case Common.CreditDebitIndicator.Credit:
                                totalCredits += txn.Amount;
                                break;
                            case Common.CreditDebitIndicator.Debit:
                                totalDebits += txn.Amount;
                                break;
                        }
                        rollingBalance -= txn.SignedAmount;
                    }

                    return new EodBalanceReport(dayTxns.Date, rollingBalance, totalCredits, totalDebits);
                });

            return Task.FromResult(eodBalances);
        }
    }
}
