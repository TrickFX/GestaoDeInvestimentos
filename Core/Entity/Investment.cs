using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Investment : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public double Value {  get; set; }

        [Required]
        public DateTime ExpiryDate {  get; set; }
    }
}
