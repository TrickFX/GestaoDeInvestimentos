using Core.Constants;
using Core.Entity;
using Core.Input.Investment;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebGestao.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class InvestimentosController : ControllerBase
    {
        private readonly IInvestmentRepository _investmentRepository;

        public InvestimentosController(IInvestmentRepository investmentRepository)
        {
            _investmentRepository = investmentRepository;
        }

        /// <summary>
        /// Endpoint responsável por realizar a inclusão de novos investimentos
        /// </summary>
        /// <param name="investmentInput"></param>
        /// <returns></returns>
        /// <response code="201">Sucesso no cadastro de um investimento</response>
        [HttpPost("AdicionarInvestimento/")]
        public IActionResult AdicionarInvestimento([FromBody] InvestmentCreateInput investmentInput)
        {
            try
            {
                var investment = new Investment()
                {
                    Name = investmentInput.Name,
                    Value = investmentInput.Value,
                    ExpiryDate = investmentInput.ExpiryDate
                };

                _investmentRepository.Cadastrar(investment);
                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint responsável por realizar a alteração de um investimento
        /// </summary>
        /// <param name="investmentInput"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso na alteração de um investimento</response>
        [HttpPut("AlterarInvestimento/")]
        public IActionResult AlterarInvestimento([FromBody] InvestmentInput investmentInput)
        {
            try
            {
                if (_investmentRepository.VerificarExistenciaEntidade(investmentInput.Id))
                {
                    var investimento = _investmentRepository.ObterPorId(investmentInput.Id);

                    investimento.Name = investmentInput.Name;
                    investimento.Value = investmentInput.Value;
                    investimento.ExpiryDate = investmentInput.ExpiryDate;

                    _investmentRepository.Alterar(investimento);
                    return Ok();
                }

                return NotFound(ErrorMessages.InvestmentNotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint responsável por deletar um investimento específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao deletar um investimento</response>
        [HttpDelete("ApagarInvestimento/{id:int}")]
        public IActionResult ApagarInvestimento([FromRoute] int id)
        {
            try
            {
                if (_investmentRepository.VerificarExistenciaEntidade(id))
                {
                    _investmentRepository.Deletar(id);
                    return Ok();
                }

                return NotFound(ErrorMessages.InvestmentNotFound);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        /// <summary>
        /// Endpoint responsável por listar todos os investimentos disponíveis.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ObterTodos/")]
        public IActionResult ObterTodos()
        {
            try
            {
                return Ok(_investmentRepository.ObterTodos());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Endpoint responsável por realizar a busca de um investimento específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ObterPorId/{id:int}")]
        public IActionResult ObterPorId([FromRoute] int id)
        {
            try
            {
                if (_investmentRepository.VerificarExistenciaEntidade(id))
                {
                    return Ok(_investmentRepository.ObterPorId(id));
                }

                return NotFound(ErrorMessages.InvestmentNotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
