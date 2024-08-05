using Core.Entity;

namespace Core.Repository
{
    public interface IInvestmentRepository : IRepository<Investment>
    {
        IList<Investment> ObterInvestimentosAtivos();
    }
}
