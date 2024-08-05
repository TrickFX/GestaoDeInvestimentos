using Core.Entity;
using Core.Enums;
using Core.Input.Transaction;
using Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using WebGestao.Controllers;
using Xunit;

namespace WebGestao.Tests
{
    public class TransacoesControllerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IInvestmentRepository> _investmentRepositoryMock;
        private readonly TransacoesController _controller;

        public TransacoesControllerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _investmentRepositoryMock = new Mock<IInvestmentRepository>();
            _controller = new TransacoesController(_investmentRepositoryMock.Object, _transactionRepositoryMock.Object);

            // Simula um usuário autenticado
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, nameof(PermissionType.User))
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public void ComprarInvestimento_RetornaOk_QuandoSucesso()
        {
            // Arrange
            var investInput = new BuyInvestInput
            {
                InvestmentId = 1,
                Amount = 1000
            };

            var investment = new Investment
            {
                Id = 1,
                Name = "Test Investment",
                Value = 1000,
                ExpiryDate = DateTime.Parse("30/12/2024")
            };

            _investmentRepositoryMock.Setup(repo => repo.ObterPorId(investInput.InvestmentId)).Returns(investment);
            _transactionRepositoryMock.Setup(repo => repo.ObterEstoqueDisponivel(It.IsAny<int>(), investInput.InvestmentId)).Returns(0);

            // Act
            var result = _controller.ComprarInvestimento(investInput);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Sucesso!", okResult.Value);
        }

        [Fact]
        public void ComprarInvestimento_RetornaBadRequest_QuandoProdutoNaoExiste()
        {
            // Arrange
            var investInput = new BuyInvestInput
            {
                InvestmentId = 1,
                Amount = 1000
            };

            _investmentRepositoryMock.Setup(repo => repo.ObterPorId(investInput.InvestmentId)).Returns((Investment)null);

            // Act
            var result = _controller.ComprarInvestimento(investInput);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("O investimento informado não existe.", badRequestResult.Value);
        }

        [Fact]
        public void VenderInvestimento_RetornaOk_QuandoSucesso()
        {
            // Arrange
            var sellInvestInput = new BuyInvestInput
            {
                InvestmentId = 1,
                Amount = 1000
            };

            var investment = new Investment
            {
                Id = 1,
                Name = "Test Investment",
                Value = 1000,
                ExpiryDate = DateTime.Parse("30/12/2024")
            };

            var transaction = new Transaction
            {
                CustomerId = 1,
                InvestmentId = 1,
                TransactionDate = DateTime.Now,
                Amount = 1000,
                IsPurchase = true
            };

            _investmentRepositoryMock.Setup(repo => repo.ObterPorId(sellInvestInput.InvestmentId)).Returns(investment);
            _transactionRepositoryMock.Setup(repo => repo.ObterProdutoPago(sellInvestInput.InvestmentId, 1)).Returns(transaction);
            _transactionRepositoryMock.Setup(repo => repo.ObterEstoqueDisponivel(1, sellInvestInput.InvestmentId)).Returns(1000);

            // Act
            var result = _controller.VenderInvestimento(sellInvestInput);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void VenderInvestimento_RetornaBadRequest_QuandoProdutoNaoExiste()
        {
            // Arrange
            var sellInvestInput = new BuyInvestInput
            {
                InvestmentId = 1,
                Amount = 1000
            };

            _investmentRepositoryMock.Setup(repo => repo.ObterPorId(sellInvestInput.InvestmentId)).Returns((Investment)null);

            // Act
            var result = _controller.VenderInvestimento(sellInvestInput);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("O investimento informado não existe.", badRequestResult.Value);
        }

        [Fact]
        public void ConsultarProdutosDisponiveis_RetornaOk_ComListaDeProdutos()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, InvestmentId = 1, Amount = 1000, IsPurchase = true },
                new Transaction { Id = 2, InvestmentId = 2, Amount = 2000, IsPurchase = true }
            };

            _transactionRepositoryMock.Setup(repo => repo.ObterListaProdutosPagosPorId(It.IsAny<int>())).Returns(transactions);

            // Act
            var result = _controller.ConsultarProdutosDisponiveis();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(transactions, okResult.Value);
        }

        [Fact]
        public void ConsultarExtratos_RetornaOk_ComListaDeExtratos()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, InvestmentId = 1, Amount = 1000, IsPurchase = true },
                new Transaction { Id = 2, InvestmentId = 2, Amount = 2000, IsPurchase = true }
            };

            _transactionRepositoryMock.Setup(repo => repo.ObterTodosExtratosPorId(It.IsAny<int>())).Returns(transactions);

            // Act
            var result = _controller.ConsultarExtratos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(transactions, okResult.Value);
        }
    }
}
