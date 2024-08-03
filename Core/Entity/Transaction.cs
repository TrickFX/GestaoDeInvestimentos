using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Transaction : EntityBase
    {
        [Required]
        public int InvestmentId { get; set; }

        [Required]
        public required Investment Investment { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public required Customer Customer { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public bool IsPurchase {get; set; }
    }
}
