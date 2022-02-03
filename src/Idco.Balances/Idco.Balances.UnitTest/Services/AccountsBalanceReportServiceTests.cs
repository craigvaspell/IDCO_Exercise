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
    using System.Linq;
    using Xunit;
    using Moq;
    using System.Threading.Tasks;

    public class EndOfDayBalanceReportServiceTests
    {
        private Mock<IAccountBalanceReportService> GetMockAccountBalanceReportService()
        {
            var mock = new Mock<IAccountBalanceReportService>();

            // Let's make the mock always return balance reports of +200,-100 so we can
            // easily assert on the sum for the number of accounts in the inbound request
            mock.Setup(m => m.GetEodBalanceReport(It.IsAny<Account>()))
                .Returns(Task.FromResult(
                    new BalanceReport(
                        200,
                        100,
                        new List<EndOfDayBalance>()
                        {
                            new EndOfDayBalance(DateTime.UtcNow.Date, 50),
                            new EndOfDayBalance(DateTime.UtcNow.Date.AddDays(-1), 50)
                        })));

            return mock;
        }

        [Theory]
        [MemberData(nameof(TestData_AccountRequests))]
        public void AccountsBalanceReportService_GetEodBalanceReport(TestRecord testRecord)
        {
            var mockAccountService = GetMockAccountBalanceReportService();
            var target = new AccountsBalanceReportService(mockAccountService.Object);

            var result = target.GetEodBalanceReport(testRecord.InputRequest);

            result.Should().BeEquivalentTo(testRecord.ExpectedBalanceReport);
        }

        public static IEnumerable<object[]> TestData_AccountRequests()
        {
            var creditPerAccount = 200;
            var debitPerAccount = 100;
            var balancePerDay = 50;

            // Single Account, should give us our mock +200,-100, (50/50)
            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        new List<Account>() { GetBasisAccount() }),
                    expectedReport: new BalanceReport(
                        totalCredits: creditPerAccount,
                        totalDebits: debitPerAccount,
                        endOfDayBalances: new List<EndOfDayBalance>()
                        {
                            new EndOfDayBalance(DateTime.UtcNow.Date, balancePerDay),
                            new EndOfDayBalance(DateTime.UtcNow.Date.AddDays(-1), balancePerDay)
                        }))
            };


            // Two accounts, should double up total credits/debits but combine
            // eod balances (as mock will return same day for all accounts)
            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        new List<Account>() { GetBasisAccount(), GetBasisAccount() }),
                    expectedReport: new BalanceReport(
                        totalCredits: creditPerAccount * 2,
                        totalDebits: debitPerAccount * 2,
                        endOfDayBalances: new List<EndOfDayBalance>()
                        {
                            new EndOfDayBalance(DateTime.UtcNow.Date, balancePerDay * 2),
                            new EndOfDayBalance(DateTime.UtcNow.Date.AddDays(-1), balancePerDay * 2)
                        }))
            };


            // 13 accounts, you guessed it! 13x the amounts, eods combined
            var accounts = new List<Account>();
            for (int i = 0; i < 13; i++) accounts.Add(GetBasisAccount());

            yield return new object[] {
                new TestRecord(
                    inputRequest: GetAccountsBalanceRequest(
                        DateTime.UtcNow,
                        accounts),
                    expectedReport: new BalanceReport(
                        totalCredits: creditPerAccount * 13,
                        totalDebits: debitPerAccount * 13,
                        endOfDayBalances: new List<EndOfDayBalance>()
                        {
                            new EndOfDayBalance(DateTime.UtcNow.Date, balancePerDay * 13),
                            new EndOfDayBalance(DateTime.UtcNow.Date.AddDays(-1), balancePerDay * 13)
                        }))
            };
        }

        public class TestRecord
        {
            public AccountsBalanceRequest InputRequest { get; set; }
            public BalanceReport ExpectedBalanceReport { get; set; }

            public TestRecord(
                AccountsBalanceRequest inputRequest,
                BalanceReport expectedReport)
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
    }
}
