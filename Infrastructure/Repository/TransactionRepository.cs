using Core.Entity;
using Core.Repository;
using System;
using System.Linq;

namespace Infrastructure.Repository
{
    public class TransactionRepository : EFRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public int ObterEstoqueDisponivel(int idCustomer, int idInvestment)
        {
            try
            {
                var soldTransactions = _context.Transactions
                    .Where(c => c.InvestmentId == idInvestment && c.CustomerId == idCustomer)
                    .OrderByDescending(c => c.TransactionDate)
                    .FirstOrDefault();

                if (soldTransactions == null)
                {
                    soldTransactions = new Transaction();
                    soldTransactions.Amount = 0;
                }
                   
                return Convert.ToInt32(soldTransactions.Amount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Transaction> ObterListaProdutosPagos()
        {
            try
            {
                var lastTransactions = _context.Transactions
                    .Where(c => c.Amount > 0)
                    .GroupBy(c => c.InvestmentId)
                    .Select(g => g.OrderByDescending(t => t.TransactionDate).FirstOrDefault())
                    .ToList();


                return lastTransactions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Transaction> ObterListaProdutosPagosPorId(int customerId)
        {
            try
            {
                var lastTransactions = _context.Transactions
                    .Where(c => c.CustomerId == customerId && c.Amount > 0)
                    .GroupBy(c => c.InvestmentId)
                    .Select(g => g.OrderByDescending(t => t.TransactionDate).FirstOrDefault())
                    .ToList();


                return lastTransactions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Transaction ObterProdutoPago(int idInvestment, int idCustomer)
        {
            try
            {
                var transaction = _context.Transactions
                    .Where(c => c.InvestmentId == idInvestment && c.IsPurchase == true && c.CustomerId == idCustomer)
                    .OrderByDescending(c => c.TransactionDate)
                    .FirstOrDefault();

                return transaction;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Transaction> ObterTodosExtratosPorId(int customerId)
        {
            try
            {
                return _context.Set<Transaction>().Where(entity => entity.CustomerId == customerId).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
