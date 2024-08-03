using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class TransactionRepository : EFRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
