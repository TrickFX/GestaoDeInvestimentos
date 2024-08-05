using Authentication.Input;
using Authentication.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebGestao.Controllers
{
    /// <summary>
    /// Responsável por gerar o token de autenticação
    /// </summary>
    [ApiController]
    [Route("/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenRepository _tokenRepository;

        public AuthenticationController(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Endpoint responsável por realizar a geração do Token (Bearer)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("GetToken/")]
        public IActionResult GetToken([FromBody] CustomerLoginInput input)
        {
            try
            {
                var token = _tokenRepository.GetToken(input);

                if (!String.IsNullOrEmpty(token))
                {
                    return Ok(token);
                }

                return Unauthorized("E-mail ou senha incorreto.");
            }
            catch (Exception ex)
            {

                return Unauthorized("Não foi possível realizar a geração do token.");
            }
        }
    }
}
