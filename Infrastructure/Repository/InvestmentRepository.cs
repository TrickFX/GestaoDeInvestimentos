using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class InvestmentRepository : EFRepository<Investment>, IInvestmentRepository
    {
        public InvestmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
