using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class InvestmentRepository : EFRepository<Investment>, IInvestmentRepository
    {
        public InvestmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IList<Investment> ObterInvestimentosAtivos()
        {
            try
            {
                var activeInvestments = _context.Investments
                    .Where(c => c.ExpiryDate > DateTime.UtcNow);

                return activeInvestments.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
