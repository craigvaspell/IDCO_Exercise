namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.AccountsBalanceReports;
    using Idco.Balances.Domain.Requests;
    using System.Threading.Tasks;

    public interface IEndOfDayBalanceReportService
    {
        Task<AccountsBalanceReport> GetAccountBalanceReport(AccountsBalanceRequest request);
    }
}
