namespace Idco.Balances.Domain.Accounts
{
    public class Identifiers
    {
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public string SecondaryIdentification { get; set; }

        public Identifiers(
            string sortCode,
            string accountNumber,
            string secondaryIdentification)
        {
            SortCode = sortCode;
            AccountNumber = accountNumber;
            SecondaryIdentification = secondaryIdentification;
        }
    }
}