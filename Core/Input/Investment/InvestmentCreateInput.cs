namespace Core.Input.Investment
{
    public class InvestmentCreateInput
    {
        public required string Name { get; set; }
        public required double Value { get; set; }
        public required DateTime ExpiryDate { get; set; }
    }
}
