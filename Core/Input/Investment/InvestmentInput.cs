namespace Core.Input.Investment
{
    public class InvestmentInput
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required double Value { get; set; }
        public required string ExpiryDate { get; set; }
    }
}
