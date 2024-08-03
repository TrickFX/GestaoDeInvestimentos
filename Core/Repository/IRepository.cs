using Core.Entity;

namespace Core.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Método responsável por cadastrar um registro de uma determinada entidade.
        /// </summary>
        /// <param name="entity"></param>
        void Cadastrar(T entity);

        /// <summary>
        /// Método responsável por realizar a alteração da entidade.
        /// </summary>
        /// <param name="entity"></param>
        void Alterar(T entity);

        /// <summary>
        /// Método responsável por deletar um registro de uma entidade.
        /// </summary>
        /// <param name="id"></param>
        void Deletar(int id);

        /// <summary>
        /// Método responsável por obter um registro de uma entidade
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T ObterPorId(int id);

        /// <summary>
        /// Método responsável por retornar uma lista com todos os registros de uma entidade
        /// </summary>
        /// <returns></returns>
        IList<T> ObterTodos();

        /// <summary>
        /// Método responsável por verificar a existência de uma entidade
        /// True = Existe
        /// False = Não existe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool VerificarExistenciaEntidade(int id);
    }
}
