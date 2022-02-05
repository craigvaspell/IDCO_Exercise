namespace Idco.Balances.Domain.Accounts
{
    public class Balances
    {
        public Balance Current { get; set; }
        public Balance Available { get; set; }

        public Balances(
            Balance current,
            Balance available)
        {
            Current = current;
            Available = available;
        }
    }
}