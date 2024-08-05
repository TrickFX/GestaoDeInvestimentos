using Core.Entity;
using Core.Repository;
using Core.Enums;

namespace Infrastructure.Repository
{
    public class CustomerRepository : EFRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IList<Customer> ObterTodosOperadores()
        {
            try
            {
                var listaOperadores = _context.Customers
                    .Where(c => c.PermissionType == PermissionType.Operator);

                return listaOperadores.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
