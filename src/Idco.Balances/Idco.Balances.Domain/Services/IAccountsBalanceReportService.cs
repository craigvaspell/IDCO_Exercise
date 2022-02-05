namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.BalanceReports;
    using Idco.Balances.Domain.Requests;
    using System.Threading.Tasks;

    public interface IAccountsBalanceReportService
    {
        Task<EodBalanceListReport> GetEodBalanceReport(AccountsBalanceRequest request);
    }
}
