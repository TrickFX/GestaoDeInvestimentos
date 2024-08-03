using Authentication.Input;
using Core.Entity;

namespace Authentication.Repository
{
    public interface ITokenRepository
    {
        /// <summary>
        /// Método responsável por obter o token
        /// </summary>
        /// <param name="customerInput"></param>
        /// <returns></returns>
        public string GetToken(CustomerLoginInput customerInput);

        /// <summary>
        /// Método responsável por realizar a geração do token
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public string GenerateTokenJWT(Customer customer);
    }
}
