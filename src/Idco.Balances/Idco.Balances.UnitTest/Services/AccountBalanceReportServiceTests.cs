namespace Idco.Balances.UnitTest.Services
{
    using FluentAssertions;
    using Idco.Balances.Domain.Accounts;
    using Idco.Balances.Domain.BalanceReports;
    using Idco.Balances.Domain.Common;
    using Idco.Balances.Domain.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class AccountBalanceReportServiceTests
    {
        private Mock<ILogger<AccountBalanceReportService>> GetMockLogger()
        {
            return new Mock<ILogger<AccountBalanceReportService>>();
        }


        [Theory]
        [MemberData(nameof(TestData_AccountRequests))]
        public async Task AccountBalanceReportService_GetEodBalanceReport(TestRecord testRecord)
        {
            var mockLogger = GetMockLogger();
            var target = new AccountBalanceReportService(mockLogger.Object);

            var result = await target.GetEodBalanceReport(testRecord.Input);

            result.Should().BeEquivalentTo(testRecord.ExpectedBalanceReport);
        }

        public static IEnumerable<object[]> TestData_AccountRequests()
        {
            var initialBalance = 100L;

            // Basis account with no transactions, no balances to calculate
            yield return new object[] {
                new TestRecord(
                    input: GetBasisAccount(),
                    expectedReport: new EodBalanceListReport(
                        endOfDayBalances: new List<EodBalanceReport>()),
                    totalCredits: 0,
                    totalDebits: 0)
            };


            // Transactions on one day
            var singleDayTrans = GetTransactions(
                5,
                DateTime.UtcNow.AddDays(-2).Date,
                DateTime.UtcNow.AddDays(-1).Date);
            var singleDayBalance = initialBalance - singleDayTrans.totalCredit + singleDayTrans.totalDebit;

            var singleDayAccount = GetBasisAccount();
            singleDayAccount.Transactions = singleDayTrans.transactions;

            yield return new object[] {
                new TestRecord(
                    input: singleDayAccount,
                    expectedReport: new EodBalanceListReport(
                        endOfDayBalances: new List<EodBalanceReport>()
                        {
                            new EodBalanceReport(
                                DateTime.UtcNow.AddDays(-2).Date,
                                singleDayBalance,
                                singleDayTrans.totalCredit,
                                singleDayTrans.totalDebit)
                        }),
                    totalCredits: singleDayTrans.totalCredit,
                    totalDebits: singleDayTrans.totalDebit)
            };


            // Transactions on multiple days
            var day1Trans = GetTransactions(
                5,
                DateTime.UtcNow.AddDays(-2).Date,
                DateTime.UtcNow.AddDays(-1).Date);
            var day2Trans = GetTransactions(
                5,
                DateTime.UtcNow.AddDays(-3).Date,
                DateTime.UtcNow.AddDays(-2).Date);
            var day1Balance = initialBalance - day1Trans.totalCredit + day1Trans.totalDebit;
            var day2Balance = day1Balance - day2Trans.totalCredit + day2Trans.totalDebit;

            var multiDayAccount = GetBasisAccount();
            multiDayAccount.Transactions = day1Trans.transactions.Union(day2Trans.transactions);

            yield return new object[] {
                new TestRecord(
                    input: multiDayAccount,
                    expectedReport: new EodBalanceListReport(
                        endOfDayBalances: new List<EodBalanceReport>()
                        {
                            new EodBalanceReport(
                                DateTime.UtcNow.AddDays(-2).Date,
                                day1Balance,
                                day1Trans.totalCredit,
                                day1Trans.totalDebit),
                            new EodBalanceReport(
                                DateTime.UtcNow.AddDays(-3).Date,
                                day2Balance,
                                day2Trans.totalCredit,
                                day2Trans.totalDebit)
                        }),
                    totalCredits: day1Trans.totalCredit + day2Trans.totalCredit,
                    totalDebits: day1Trans.totalDebit + day2Trans.totalDebit)
            };
        }

        public class TestRecord
        {
            public Account Input { get; set; }
            public EodBalanceListReport ExpectedBalanceReport { get; set; }
            public long TotalCredits { get; set; }
            public long TotalDebits { get; set; }

            public TestRecord(
                Account input,
                EodBalanceListReport expectedReport,
                long totalCredits,
                long totalDebits)
            {
                Input = input;
                ExpectedBalanceReport = expectedReport;
                TotalCredits = totalCredits;
                TotalDebits = totalDebits;
            }
        }

        private static Account GetBasisAccount()
        {
            return new Account(
                accountId: "TestAccountId",
                currencyCode: "GBP",
                displayName: "Test Account",
                accountType: AccountType.Personal,
                accountSubType: AccountSubType.CurrentAccount,
                identifiers: new Identifiers("", "", ""),
                parties: new List<Party>(),
                directDebits: new List<DirectDebit>(),
                balances: new Balances(             
                    current: new Balance(
                        amount: 100,
                        creditDebitIndicator: CreditDebitIndicator.Credit,
                        creditLines: null),
                    available: new Balance(
                        amount: 100,
                        creditDebitIndicator: CreditDebitIndicator.Credit,
                        creditLines: null)
                ),
                transactions: new List<Transaction>());
        }

        private static (long totalCredit, long totalDebit, IEnumerable<Transaction> transactions) GetTransactions(
            int count,
            DateTime minDate,
            DateTime maxDate)
        {
            var random = new Random();
            var transactions = new List<Transaction>();
            var rollingCredits = 0L;
            var rollingDebits = 0L;

            for (int i = 0; i < count; i++)
            {
                var range = (maxDate - minDate);
                var date = minDate + TimeSpan.FromSeconds(range.TotalSeconds - random.Next(0, (int)range.TotalSeconds));

                CreditDebitIndicator creditDebitIndicator;
                var amount = random.Next(-1000, 1000);
                var absAmount = Math.Abs(amount);

                if (amount >= 0)
                {
                    creditDebitIndicator = CreditDebitIndicator.Credit;
                    rollingCredits += absAmount;
                }
                else
                {
                    creditDebitIndicator = CreditDebitIndicator.Debit;
                    rollingDebits += absAmount;
                }

                transactions.Add(new Transaction(
                    "Test Transaction",
                    absAmount,
                    creditDebitIndicator,
                    TransactionStatus.Booked,
                    date,
                    null));
            }

            return (
                rollingCredits,
                rollingDebits,
                transactions);
        }
    }
}
