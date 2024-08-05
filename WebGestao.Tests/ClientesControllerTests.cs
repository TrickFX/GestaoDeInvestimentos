using Core.Entity;
using Core.Input.Costumer;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using WebGestao.Controllers;
using Xunit;

namespace WebGestao.Tests
{
    public class ClientesControllerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly ClienteController _controller;

        public ClientesControllerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _controller = new ClienteController(_customerRepositoryMock.Object);
        }

        [Fact]
        public void AdicionarCliente_RetornaBadRequest_QuandoCamposFaltando()
        {
            // Arrange
            var input = new CostumerCreateInput
            {
                Name = "",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                PermissionType = 0
            };

            // Act
            var result = _controller.AdicionarCliente(input);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("É necessário o preenchimento de todos os campos.", badRequestResult.Value);
        }

        [Fact]
        public void AdicionarCliente_RetornaBadRequest_QuandoPermissaoInvalida()
        {
            // Arrange
            var input = new CostumerCreateInput
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                PermissionType = 2
            };

            // Act
            var result = _controller.AdicionarCliente(input);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("O tipo de permissão informado é inválido!", badRequestResult.Value);
        }

        [Fact]
        public void ObterTodos_RetornaOk_ComListaDeClientes()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = "1a2s3d4f", PermissionType = 0 },
                new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", Password = "1a2s3d4f", PermissionType = 0 }
            };

            _customerRepositoryMock.Setup(repo => repo.ObterTodos()).Returns(customers);

            // Act
            var result = _controller.ObterTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(customers, okResult.Value);
        }

        [Fact]
        public void AdicionarCliente_RetornaBadRequest_QuandoErro()
        {
            // Arrange
            var input = new CostumerCreateInput
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                PermissionType = 0
            };

            _customerRepositoryMock.Setup(repo => repo.Cadastrar(It.IsAny<Customer>())).Throws(new Exception("Ocorreu um erro ao cadastrar um cliente."));

            // Act
            var result = _controller.AdicionarCliente(input);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Ocorreu um erro ao cadastrar um cliente.", badRequestResult.Value);
        }

        [Fact]
        public void ObterTodos_RetornaBadRequest_QuandoErro()
        {
            // Arrange
            _customerRepositoryMock.Setup(repo => repo.ObterTodos()).Throws(new Exception("Ocorreu um erro ao pesquisar os clientes/operadores"));

            // Act
            var result = _controller.ObterTodos();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Ocorreu um erro ao pesquisar os clientes/operadores", badRequestResult.Value);
        }
    }
}
