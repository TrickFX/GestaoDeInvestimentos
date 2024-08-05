using Core.Entity;

namespace Core.Repository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        int ObterEstoqueDisponivel(int idCustomer, int idInvestment);
        Transaction ObterProdutoPago(int idInvestment, int idCustomer);
        IList<Transaction> ObterTodosExtratosPorId(int customerId);
        IList<Transaction> ObterListaProdutosPagosPorId(int customerId);
        IList<Transaction> ObterListaProdutosPagos();
    }
}
