using Core.Entity;
using Core.Input.Costumer;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebGestao.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public ClienteController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        /// <summary>
        /// Endpoint responsável por realizar o cadastro de clientes e operadores.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Observações: 
        /// - Tipo de permissão 0 = Cliente
        /// - Tipo de permissão 1 = Operador
        /// </remarks>
        [HttpPost("AdicionarCliente/")]
        [AllowAnonymous]
        public IActionResult AdicionarCliente([FromBody] CostumerCreateInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Name) ||
                    string.IsNullOrEmpty(input.LastName) ||
                    string.IsNullOrEmpty(input.Email) ||
                    string.IsNullOrEmpty(input.Password))
                {
                    return BadRequest("É necessário o preenchimento de todos os campos.");
                }

                if (input.PermissionType > 1)
                    return BadRequest("O tipo de permissão informado é inválido!");

                var newCostumer = new Customer()
                {
                    FirstName = input.Name,
                    LastName = input.LastName,
                    Email = input.Email,
                    Password = input.Password,
                    PermissionType = (Core.Enums.PermissionType)input.PermissionType

                };

                _customerRepository.Cadastrar(newCostumer);

                return Created();
            }
            catch (Exception e)
            {
                return BadRequest("Ocorreu um erro ao cadastrar um cliente.");
            }
        }

        /// <summary>
        /// Endpoint responsável por retonar todos os clientes/operadores cadastrado.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ObterTodos()
        {
            try
            {
                return Ok(_customerRepository.ObterTodos());
            }
            catch (Exception e)
            {
                return BadRequest("Ocorreu um erro ao pesquisar os clientes/operadores");
            }
        }
    }
}
