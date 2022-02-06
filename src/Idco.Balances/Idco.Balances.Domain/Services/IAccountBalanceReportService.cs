namespace Idco.Balances.Domain.Services
{
    using Idco.Balances.Domain.Accounts;
    using Idco.Balances.Domain.BalanceReports;
    using System.Threading.Tasks;

    public interface IAccountBalanceReportService
    {
        Task<EodBalanceListReport> GetEodBalanceReport(Account account);
    }
}
