using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Customer : EntityBase
    {

        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Password { get; set; }

        [Required]
        public required PermissionType PermissionType { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
