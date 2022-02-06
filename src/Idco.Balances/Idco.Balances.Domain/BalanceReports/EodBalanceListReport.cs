namespace Idco.Balances.Domain.BalanceReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EodBalanceListReport
    {
        public long TotalCredits => totalCredits?.Value ?? 0;
        public long TotalDebits => totalDebits?.Value ?? 0;

        private Lazy<long> totalCredits;
        private Lazy<long> totalDebits;

        public IEnumerable<EodBalanceReport> Balances { get; set; }
            = new List<EodBalanceReport>();


        // No credits/debits, no balances
        private EodBalanceListReport() { }
        public static EodBalanceListReport EmptyBalanceListReport()
            => new EodBalanceListReport();

        // No balances, but we have an existing known set of credit/debit totals
        public EodBalanceListReport(long totalCredits, long totalDebits)
        {
            this.totalCredits = new Lazy<long>(totalCredits);
            this.totalDebits = new Lazy<long>(totalDebits);
        }

        // Calculate by balances
        public EodBalanceListReport(IEnumerable<EodBalanceReport> endOfDayBalances)
        {
            Balances = endOfDayBalances;

            // Only need to calculate these once, and we might end up just
            // pulling the balances instead of the total props so use lazy
            totalCredits = new Lazy<long>(() => Balances.Sum(eodb => eodb.TotalCredits));
            totalDebits  = new Lazy<long>(() => Balances.Sum(eodb => eodb.TotalDebits));
        }
    }
}
