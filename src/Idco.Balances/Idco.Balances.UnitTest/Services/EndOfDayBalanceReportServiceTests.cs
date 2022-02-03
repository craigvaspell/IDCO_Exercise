namespace Idco.Balances.UnitTest.Services
{
    using FluentAssertions;
    using Idco.Balances.Domain.Accounts;
    using Idco.Balances.Domain.AccountsBalanceReports;
    using Idco.Balances.Domain.Common;
    using Idco.Balances.Domain.Requests;
    using Idco.Balances.Domain.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class EndOfDayBalanceReportServiceTests
    {
        [Theory]
        [MemberData(nameof(TestData_AccountRequests))]
        public void EndOfDayBalanceReportService_GetAccountBalanceReport(TestRecord testRecord)
        {
            var target = new EndOfDayBalanceReportService();

            var result = target.GetAccountBalanceReport(testRecord.InputRequest);

            result.Should().BeEquivalentTo(testRecord.ExpectedBalanceReport);
        }

        public static IEnumerable<object[]> TestData_AccountRequests()
        {
            // Single Account, No balances to calculate
            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        new List<Account>() { GetBasisAccount() }),
                    expectedReport: new AccountsBalanceReport(
                        totalCredits: 0,
                        totalDebits: 0,
                        endOfDayBalances: new List<EndOfDayBalance>()))
            };


            // Single Account, transactions on one day
            var singleDayTrans = GetTransactions(
                5,
                DateTime.UtcNow.AddDays(-2),
                DateTime.UtcNow.AddDays(-1));

            var singleDayAccount = GetBasisAccount();
            singleDayAccount.Transactions = singleDayTrans.transactions;

            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        new List<Account>() { singleDayAccount }),
                    expectedReport: new AccountsBalanceReport(
                        totalCredits: singleDayTrans.totalCredit,
                        totalDebits: singleDayTrans.totalDebit,
                        endOfDayBalances: new List<EndOfDayBalance>()
                        {
                            new EndOfDayBalance(
                                DateTime.UtcNow.AddDays(-1),
                                singleDayTrans.totalCredit - singleDayTrans.totalDebit)
                        }))
            };


            // Single Account, transactions on multiple days
            var day1Trans = GetTransactions(
                5,
                DateTime.UtcNow.AddDays(-2),
                DateTime.UtcNow.AddDays(-1));
            var day2Trans = GetTransactions(
                5,
                DateTime.UtcNow.AddDays(-3),
                DateTime.UtcNow.AddDays(-2));

            var multiDayAccount = GetBasisAccount();
            multiDayAccount.Transactions = day1Trans.transactions.Union(day2Trans.transactions);

            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        new List<Account>() { multiDayAccount }),
                    expectedReport: new AccountsBalanceReport(
                        totalCredits: day1Trans.totalCredit + day2Trans.totalCredit,
                        totalDebits: day1Trans.totalDebit + day2Trans.totalDebit,
                        endOfDayBalances: new List<EndOfDayBalance>()
                        {
                            new EndOfDayBalance(
                                DateTime.UtcNow.AddDays(-1),
                                day1Trans.totalCredit - day1Trans.totalDebit),
                            new EndOfDayBalance(
                                DateTime.UtcNow.AddDays(-2),
                                day2Trans.totalCredit - day2Trans.totalDebit)
                        }))
            };
        }

        public class TestRecord
        {
            public AccountsBalanceRequest InputRequest { get; set; }
            public AccountsBalanceReport ExpectedBalanceReport { get; set; }

            public TestRecord(
                AccountsBalanceRequest inputRequest,
                AccountsBalanceReport expectedReport)
            {
                InputRequest = inputRequest;
                ExpectedBalanceReport = expectedReport;
            }
        }

        private static AccountsBalanceRequest GetAccountsBalanceRequest(DateTime requestDateTime, ICollection<Account> accounts)
        {
            return new AccountsBalanceRequest(
                brandName: "TestBank",
                dataSourceName: "TestDataSource",
                dataSourceType: "CredentialSharing",
                requestDateTime: requestDateTime,
                accounts: accounts
                );
        }

        private static Account GetBasisAccount()
        {
            return new Account(
                accountId: "TestAccountId",
                currencyCode: "GBP",
                displayName: "Test Account",
                accountType: AccountType.Personal,
                accountSubType: AccountSubType.CurrentAccount,
                identifiers: new Dictionary<string, string>(),
                parties: new List<Party>(),
                directDebits: new List<DirectDebit>(),
                balances: new List<Balance>()
                {
                    new Balance(
                        balanceType: BalanceType.Current,
                        amount: 100,
                        creditDebitIndicator: CreditDebitIndicator.Credit,
                        creditLines: null),
                    new Balance(
                        balanceType: BalanceType.Available,
                        amount: 100,
                        creditDebitIndicator: CreditDebitIndicator.Credit,
                        creditLines: null)
                },
                transactions: new List<Transaction>());
        }

        private static (long totalCredit, long totalDebit, IEnumerable<Transaction> transactions) GetTransactions(
            int count,
            DateTime minDate,
            DateTime maxDate)
        {
            var random = new Random();
            var transactions = new List<Transaction>();

            for (int i = 0; i < count; i++)
            {
                var amount = random.Next(-1000, 1000);
                var range = (maxDate - minDate);
                var date = minDate + TimeSpan.FromSeconds(range.TotalSeconds - random.Next(0, (int)range.TotalSeconds));

                transactions.Add(new Transaction(
                    "Test Transaction",
                    amount,
                    amount < 0
                        ? CreditDebitIndicator.Credit
                        : CreditDebitIndicator.Debit,
                    TransactionStatus.Booked,
                    date,
                    null));
            }

            return (
                transactions.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount)),
                transactions.Where(t => t.Amount > 0).Sum(t => t.Amount),
                transactions);
        }
    }
}
