namespace Idco.Balances.UnitTest.Services
{
    using FluentAssertions;
    using Idco.Balances.Domain.Accounts;
    using Idco.Balances.Domain.BalanceReports;
    using Idco.Balances.Domain.Common;
    using Idco.Balances.Domain.Requests;
    using Idco.Balances.Domain.Services;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Moq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class EndOfDayBalanceReportServiceTests
    {
        private Mock<IAccountBalanceReportService> GetMockAccountBalanceReportService()
        {
            var mock = new Mock<IAccountBalanceReportService>();

            // Let's make the mock always return aggregate balance reports of +200,-100 (+100, -50 /pcd) 
            // so we can easily assert on the sum for the number of accounts in the inbound request
            mock.Setup(m => m.GetEodBalanceReport(It.IsAny<Account>()))
                .Returns(Task.FromResult(
                    new EodBalanceListReport(
                        new List<EodBalanceReport>()
                        {
                            new EodBalanceReport(DateTime.UtcNow.Date, 50, 100, 50),
                            new EodBalanceReport(DateTime.UtcNow.Date.AddDays(-1), 0, 100, 50)
                        })));

            return mock;
        }

        private Mock<ILogger<AccountsBalanceReportService>> GetMockLogger()
        {
            return new Mock<ILogger<AccountsBalanceReportService>>();
        }

        [Theory]
        [MemberData(nameof(TestData_AccountRequests))]
        public async Task AccountsBalanceReportService_GetEodBalanceReport(TestRecord testRecord)
        {
            var mockAccountService = GetMockAccountBalanceReportService();
            var mockLogger = GetMockLogger();
            var target = new AccountsBalanceReportService(mockAccountService.Object, mockLogger.Object);

            var result = await target.GetEodBalanceReport(testRecord.InputRequest);

            result.Should().BeEquivalentTo(testRecord.ExpectedBalanceReport);
        }

        public static IEnumerable<object[]> TestData_AccountRequests()
        {
            var totalCreditPerAccount = 200;
            var totalDebitPerAccount = 100;
            var totalEndBalancePerAccount = 50;
            var totalStartBalancePerAccount = 0;

            var creditPerAccountPerDay = 100;
            var debitPerAccountPerDay = 50;


            // Single Account, should give us our mock +200,-100
            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(DateTime.UtcNow, new List<Account>() { GetBasisAccount() }),

                    expectedReport: new EodBalanceListReport(
                        endOfDayBalances: new List<EodBalanceReport>()
                        {
                            new EodBalanceReport(DateTime.UtcNow.Date, totalEndBalancePerAccount, creditPerAccountPerDay, debitPerAccountPerDay),
                            new EodBalanceReport(DateTime.UtcNow.Date.AddDays(-1), totalStartBalancePerAccount, creditPerAccountPerDay, debitPerAccountPerDay)
                        }),

                    totalCredits: totalCreditPerAccount,
                    totalDebits: totalDebitPerAccount)
            };


            // Two accounts, should double up total credits/debits but combine
            // eod balances (as mock will return same day for all accounts)
            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        new List<Account>() { GetBasisAccount(), GetBasisAccount() }),

                    expectedReport: new EodBalanceListReport(
                        endOfDayBalances: new List<EodBalanceReport>()
                        {
                            new EodBalanceReport(DateTime.UtcNow.Date, totalEndBalancePerAccount * 2, creditPerAccountPerDay * 2, debitPerAccountPerDay * 2),
                            new EodBalanceReport(DateTime.UtcNow.Date.AddDays(-1), totalStartBalancePerAccount * 2, creditPerAccountPerDay * 2, debitPerAccountPerDay * 2)
                        }),

                    totalCredits: totalCreditPerAccount * 2,
                    totalDebits: totalDebitPerAccount * 2)
            };


            // 13 accounts, you guessed it! 13x the amounts, eods combined
            var accounts = new List<Account>();
            for (int i = 0; i < 13; i++) accounts.Add(GetBasisAccount());

            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        accounts),

                    expectedReport: new EodBalanceListReport(
                        endOfDayBalances: new List<EodBalanceReport>()
                        {
                            new EodBalanceReport(DateTime.UtcNow.Date, totalEndBalancePerAccount * 13, creditPerAccountPerDay * 13, debitPerAccountPerDay * 13),
                            new EodBalanceReport(DateTime.UtcNow.Date.AddDays(-1), totalStartBalancePerAccount * 13, creditPerAccountPerDay * 13, debitPerAccountPerDay * 13)
                        }),

                    totalCredits: totalCreditPerAccount * 13,
                    totalDebits: totalDebitPerAccount * 13)
            };
        }

        public class TestRecord
        {
            public AccountsBalanceRequest InputRequest { get; set; }
            public EodBalanceListReport ExpectedBalanceReport { get; set; }

            public long TotalCredits { get; set; }
            public long TotalDebits { get; set; }

            public TestRecord(
                AccountsBalanceRequest inputRequest,
                EodBalanceListReport expectedReport,
                long totalCredits,
                long totalDebits)
            {
                InputRequest = inputRequest;
                ExpectedBalanceReport = expectedReport;
                TotalCredits = totalCredits;
                TotalDebits = totalDebits;
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
    }
}
