﻿using Core.Constants;
using Core.Entity;
using Core.Enums;
using Core.Input.Investment;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        /// <remarks>
        /// Observação:
        /// O campo expiryDate é preenchido no formato dd/MM/yyyy.
        /// </remarks>
        [HttpPost("AdicionarInvestimento/")]
        [Authorize(Roles = nameof(PermissionType.Operator))]
        public IActionResult AdicionarInvestimento([FromBody] InvestmentCreateInput investmentInput)
        {
            try
            {
                if (String.IsNullOrEmpty(investmentInput.Name))
                    return BadRequest("É obrigatório preencher o nome do produto/investimento!");

                if (investmentInput.Value == 0)
                    return BadRequest("O valor do produto/investimento não pode ser zero ou nulo!");

                var investment = new Investment()
                {
                    Name = investmentInput.Name,
                    Value = investmentInput.Value,
                    ExpiryDate = ConverterParaData(investmentInput.ExpiryDate)
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
        /// <remarks>
        /// Observação:
        /// O campo expiryDate é preenchido no formato dd/MM/yyyy.
        /// </remarks>
        [HttpPut("AlterarInvestimento/")]
        [Authorize(Roles = nameof(PermissionType.Operator))]
        public IActionResult AlterarInvestimento([FromBody] InvestmentInput investmentInput)
        {
            try
            {
                if (String.IsNullOrEmpty(investmentInput.Name))
                    return BadRequest("É obrigatório preencher o nome do produto/investimento!");

                if (investmentInput.Value == 0)
                    return BadRequest("O valor do produto/investimento não pode ser zero ou nulo!");

                if (_investmentRepository.VerificarExistenciaEntidade(investmentInput.Id))
                {
                    var investimento = _investmentRepository.ObterPorId(investmentInput.Id);

                    investimento.Name = investmentInput.Name;
                    investimento.Value = investmentInput.Value;
                    investimento.ExpiryDate = ConverterParaData(investmentInput.ExpiryDate);

                    _investmentRepository.Alterar(investimento);
                    return Ok("Investimento alterado com sucesso");
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
        [Authorize(Roles = nameof(PermissionType.Operator))]
        public IActionResult ApagarInvestimento([FromRoute] int id)
        {
            try
            {
                if (_investmentRepository.VerificarExistenciaEntidade(id))
                {
                    _investmentRepository.Deletar(id);
                    return Ok("Investimento deletado com sucesso");
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        private DateTime ConverterParaData(string date)
        {
            DateTime dateTime;

            try
            {
                DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                return dateTime;
            }
            catch (Exception)
            {
                throw new Exception("A data informada é inválida!");
            }
        }

    }
}
