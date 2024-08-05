using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EFRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Cadastrar(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Alterar(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Deletar(int id)
        {
            _dbSet.Remove(ObterPorId(id));
            _context.SaveChanges();
        }

        public T ObterPorId(int id)
        {
            return _dbSet.FirstOrDefault(entity => entity.Id == id);
        }

        public IList<T> ObterTodos()
        {
            return _context.Set<T>().ToList();
        }

        public bool VerificarExistenciaEntidade(int id)
        {
            if (ObterPorId(id) != null)
            {
                return true;
            }

            return false;
        }

    }
}
