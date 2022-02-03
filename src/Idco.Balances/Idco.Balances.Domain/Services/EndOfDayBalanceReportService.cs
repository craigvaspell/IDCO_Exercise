namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.AccountsBalanceReports;
    using Idco.Balances.Domain.Requests;
    using System.Threading.Tasks;

    public class EndOfDayBalanceReportService : IEndOfDayBalanceReportService
    {
        public async Task<AccountsBalanceReport> GetAccountBalanceReport(AccountsBalanceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
