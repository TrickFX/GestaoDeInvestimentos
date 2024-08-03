using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
