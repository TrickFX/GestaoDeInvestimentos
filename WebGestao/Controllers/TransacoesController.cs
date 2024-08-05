using Core.Enums;
using Core.Input.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Entity;
using Core.Repository;
using System.Security.Claims;

namespace WebGestao.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IInvestmentRepository _investmentRepository;

        public TransacoesController(IInvestmentRepository investmentRepository, ITransactionRepository transactionRepository)
        {
            _investmentRepository = investmentRepository;
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Endpoint responsável por realizar a compra de produtos/investimentos (cliente)
        /// </summary>
        /// <param name="investInput"></param>
        /// <returns></returns>
        [HttpPost("ComprarInvestimento/")]
        [Authorize(Roles = nameof(PermissionType.User))]
        public IActionResult ComprarInvestimento([FromBody] BuyInvestInput investInput)
        {
            try
            {
                var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var product = _investmentRepository.ObterPorId(investInput.InvestmentId);

                if (product == null)
                    return BadRequest("O investimento informado não existe.");

                if (investInput.Amount == 0)
                    return BadRequest("É necessário preencher uma quantia.");

                var soldTransactions = _transactionRepository.ObterEstoqueDisponivel(int.Parse(customerId), investInput.InvestmentId);
                var estoqueDisponivel = soldTransactions + investInput.Amount;

                var newTransaction = new Transaction()
                {
                    CustomerId = int.Parse(customerId),
                    InvestmentId = investInput.InvestmentId,
                    TransactionDate = DateTime.UtcNow,
                    Amount = estoqueDisponivel,
                    IsPurchase = true
                };

                _transactionRepository.Cadastrar(newTransaction);

                return Ok("Sucesso!");
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível realizar a compra do produto/investimento");
            }
        }

        /// <summary>
        /// Endpoint responsável por realizar a venda de investimentos/produtos (cliente)
        /// </summary>
        /// <param name="sellInvestInput"></param>
        /// <returns></returns>
        [HttpPost("VenderInvestimento")]
        [Authorize(Roles = nameof(PermissionType.User))]
        public IActionResult VenderInvestimento([FromBody] BuyInvestInput sellInvestInput)
        {
            try
            {
                var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var product = _investmentRepository.ObterPorId(sellInvestInput.InvestmentId);

                var transaction = _transactionRepository.ObterProdutoPago(sellInvestInput.InvestmentId, int.Parse(customerId));
                var soldTransactions = _transactionRepository.ObterEstoqueDisponivel(int.Parse(customerId), sellInvestInput.InvestmentId);

                if (sellInvestInput.Amount == 0)
                    return BadRequest("É necessário preencher uma quantidade.");

                if (product == null)
                    return BadRequest("O investimento informado não existe.");

                if (soldTransactions == 0)
                    return BadRequest("Não há uma quantia para vender.");

                if (transaction == null)
                    return BadRequest("Nenhuma compra encontrada para esse investimento.");

                if (sellInvestInput.Amount > soldTransactions)
                    return BadRequest("Quantidade de venda excede a quantia disponível.");

                double estoqueRestante = soldTransactions - sellInvestInput.Amount;

                var newTransaction = new Transaction()
                {
                    CustomerId = int.Parse(customerId),
                    InvestmentId = sellInvestInput.InvestmentId,
                    TransactionDate = DateTime.Now,
                    Amount = estoqueRestante,
                    IsPurchase = false
                };

                _transactionRepository.Cadastrar(newTransaction);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Ocorreu um erro ao tentar vender o investimento");
            }
        }

        /// <summary>
        /// Endpoint responsável por obter os produtos ativos.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ConsultarProdutosDisponiveis/")]
        [Authorize(Roles = nameof(PermissionType.User))]
        public IActionResult ConsultarProdutosDisponiveis()
        {
            try
            {
                List<Investment> productList = new List<Investment>();
                var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var transactions = _transactionRepository.ObterListaProdutosPagosPorId(int.Parse(customerId));

                foreach (var transaction in transactions)
                {
                    var product = _investmentRepository.ObterPorId(transaction.InvestmentId);
                    productList.Add(product);
                }

                return Ok(productList);
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível consultar os produtos disponíveis.");
            }
        }

        /// <summary>
        /// Endpoint responsável por retornar uma lista de extratos de um cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ConsultarExtratos/")]
        [Authorize(Roles = nameof(PermissionType.User))]
        public IActionResult ConsultarExtratos()
        {
            try
            {
                var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                return Ok(_transactionRepository.ObterTodosExtratosPorId(int.Parse(customerId)));
            }
            catch (Exception)
            {
                return BadRequest("Ocorreu um erro ao consultar os extratos");
            }
        }
    }
}
