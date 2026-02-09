using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Endava.Products.Api.Controllers;
using Endava.Products.Data.Dtos;
using Endava.Products.Data.Interfaces;
using Endava.Products.Data.Models;
using Endava.Products.Data.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Endava.Products.Tests.Unit
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductsRepository> _productsRepo = new();
        private readonly Mock<ICategoriesRepository> _categoriesRepo = new();

        private ProductsController CreateController() =>
            new(_productsRepo.Object, _categoriesRepo.Object);

        [Fact]
        public async Task GetProducts_ReturnsBadRequest_WhenMinPriceGreaterThanMaxPrice()
        {
            // Arrange
            var controller = CreateController();
            var filter = new ProductsFilterRequest { MinPrice = 100, MaxPrice = 50 };

            // Act
            var result = await controller.PlaceOrder(filter);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("MinPrice cannot be greater than MaxPrice.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetProducts_ReturnsOk_WithProducts()
        {
            // Arrange
            var controller = CreateController();

            var products = new List<ProductDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone",
                    Price = 1000,
                },
            };

            _productsRepo
                .Setup(r => r.Fetch(It.IsAny<ProductsFilterRequest>()))
                .ReturnsAsync(products);

            // Act
            var result = await controller.PlaceOrder(new ProductsFilterRequest());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(products, okResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var controller = CreateController();
            var id = Guid.NewGuid();

            _productsRepo.Setup(r => r.GetById(id)).ReturnsAsync((Product?)null);

            // Act
            var result = await controller.UpdateProduct(id, new UpdateProductRequest());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenCategoryDoesNotExist()
        {
            // Arrange
            var controller = CreateController();
            var product = new Product { Id = Guid.NewGuid() };

            _productsRepo.Setup(r => r.GetById(product.Id)).ReturnsAsync(product);
            _categoriesRepo.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

            var dto = new UpdateProductRequest { CategoryId = Guid.NewGuid() };

            // Act
            var result = await controller.UpdateProduct(product.Id, dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Category with given ID does not exist", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var controller = CreateController();
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Old",
                Price = 10,
            };

            var category = new Category { Id = Guid.NewGuid(), Name = "Cat" };

            _productsRepo.Setup(r => r.GetById(product.Id)).ReturnsAsync(product);
            _categoriesRepo.Setup(r => r.GetById(category.Id)).ReturnsAsync(category);

            var dto = new UpdateProductRequest
            {
                Name = "New",
                Price = 20,
                CategoryId = category.Id,
            };

            // Act
            var result = await controller.UpdateProduct(product.Id, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);

            Assert.Equal("New", product.Name);
            Assert.Equal(20, product.Price);

            _productsRepo.Verify(r => r.Update(product), Times.Once);
        }
    }
}
