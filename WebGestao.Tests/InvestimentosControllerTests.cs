using Core.Constants;
using Core.Entity;
using Core.Input.Investment;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebGestao.Controllers;

namespace WebGestao.Tests
{
    public class InvestimentosControllerTests
    {
        private readonly Mock<IInvestmentRepository> _investmentRepositoryMock;
        private readonly InvestimentosController _controller;

        public InvestimentosControllerTests()
        {
            _investmentRepositoryMock = new Mock<IInvestmentRepository>();
            _controller = new InvestimentosController(_investmentRepositoryMock.Object);
        }

        [Fact]
        public void AdicionarInvestimento_RetornaBadRequest_QuandoNomeVazio()
        {
            // Arrange
            var investmentInput = new InvestmentCreateInput
            {
                Name = "",
                Value = 1000,
                ExpiryDate = "30/12/2024"
            };

            // Act
            var result = _controller.AdicionarInvestimento(investmentInput) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("É obrigatório preencher o nome do produto/investimento!", result.Value);
        }

        [Fact]
        public void AlterarInvestimento_RetornaOk_QuandoSucesso()
        {
            // Arrange
            var investmentInput = new InvestmentInput
            {
                Id = 1,
                Name = "Updated Investment",
                Value = 2000,
                ExpiryDate = "30/12/2025"
            };

            var investment = new Investment
            {
                Id = 1,
                Name = "Test Investment",
                Value = 1000,
                ExpiryDate = DateTime.Parse("30/12/2024")
            };

            _investmentRepositoryMock.Setup(repo => repo.VerificarExistenciaEntidade(investmentInput.Id)).Returns(true);
            _investmentRepositoryMock.Setup(repo => repo.ObterPorId(investmentInput.Id)).Returns(investment);

            // Act
            var result = _controller.AlterarInvestimento(investmentInput) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Investimento alterado com sucesso", result.Value);
        }

        [Fact]
        public void ApagarInvestimento_RetornaOk_QuandoSucesso()
        {
            // Arrange
            var investmentId = 1;
            _investmentRepositoryMock.Setup(repo => repo.VerificarExistenciaEntidade(investmentId)).Returns(true);

            // Act
            var result = _controller.ApagarInvestimento(investmentId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Investimento deletado com sucesso", result.Value);
        }

        [Fact]
        public void ObterTodos_RetornaOk_ComListaDeInvestimentos()
        {
            // Arrange
            var investments = new List<Investment>
            {
                new Investment { Id = 1, Name = "Investment 1", Value = 1000, ExpiryDate = DateTime.Parse("30/12/2024") },
                new Investment { Id = 2, Name = "Investment 2", Value = 2000, ExpiryDate = DateTime.Parse("30/12/2025") }
            };

            _investmentRepositoryMock.Setup(repo => repo.ObterTodos()).Returns(investments);

            // Act
            var result = _controller.ObterTodos() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(investments, result.Value);
        }

        [Fact]
        public void ObterPorId_RetornaOk_ComInvestimento()
        {
            // Arrange
            var investmentId = 1;
            var investment = new Investment
            {
                Id = investmentId,
                Name = "Test Investment",
                Value = 1000,
                ExpiryDate = DateTime.Parse("30/12/2024")
            };

            _investmentRepositoryMock.Setup(repo => repo.VerificarExistenciaEntidade(investmentId)).Returns(true);
            _investmentRepositoryMock.Setup(repo => repo.ObterPorId(investmentId)).Returns(investment);

            // Act
            var result = _controller.ObterPorId(investmentId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(investment, result.Value);
        }

        [Fact]
        public void ObterPorId_RetornaNotFound_QuandoNaoEncontrado()
        {
            // Arrange
            var investmentId = 1;
            _investmentRepositoryMock.Setup(repo => repo.VerificarExistenciaEntidade(investmentId)).Returns(false);

            // Act
            var result = _controller.ObterPorId(investmentId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(ErrorMessages.InvestmentNotFound, result.Value);
        }
    }
}
