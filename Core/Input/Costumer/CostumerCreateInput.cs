namespace Core.Input.Costumer
{
    public class CostumerCreateInput
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public int PermissionType { get; set; }
        public required string Password { get; set; }
    }
}
