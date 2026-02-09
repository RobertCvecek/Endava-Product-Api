using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Endava.Products.Data;
using Endava.Products.Data.Models;
using Endava.Products.Data.Requests;
using Endava.Products.Tests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Assert = Xunit.Assert;

namespace Endava.Products.Tests.Integration
{
    public class ProductsControllerTest(WebApplicationFactory<Program> factory)
        : BaseTest(factory, nameof(ProductsControllerTest))
    {
        [Fact]
        public async Task GetProducts_Returns400_WhenMinPriceGreaterThanMaxPrice()
        {
            // Act
            var response = await Client.GetAsync("/Products?MinPrice=100&MaxPrice=50");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_FilterByPrice_ReturnsFilteredProducts()
        {
            // Arrange
            var electronics = Seed(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Electronics",
                    Description = "Tech",
                }
            );

            Seed(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone",
                    Price = 1000,
                    Category = electronics,
                }
            );

            Seed(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Banana",
                    Price = 2,
                    Category = electronics,
                }
            );

            // Act
            var response = await Client.GetAsync("Products?MinPrice=500");

            // Assert
        }

        [Fact]
        public async Task UpdateProduct_Returns404_WhenProductDoesNotExist()
        {
            // Act
            var response = await Client.PutAsJsonAsync(
                $"/Products/{Guid.NewGuid()}",
                new UpdateProductRequest { Name = "New" }
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_Returns400_WhenCategoryDoesNotExist()
        {
            // Arrange
            var category = Seed(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Old",
                    Description = "Test",
                }
            );

            var product = Seed(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Price = 10,
                    CategoryId = category.Id,
                }
            );

            // Act
            var response = await Client.PutAsJsonAsync(
                $"/Products/{product.Id}",
                new UpdateProductRequest { CategoryId = Guid.NewGuid() }
            );

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_Returns204_AndUpdatesEntity()
        {
            // Arrange
            var category = Seed(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "NewCat",
                    Description = "Test",
                }
            );

            var product = Seed(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Old",
                    Price = 10,
                    CategoryId = category.Id,
                }
            );

            // Act
            var response = await Client.PutAsJsonAsync(
                $"/Products/{product.Id}",
                new UpdateProductRequest { Name = "Updated", Price = 20 }
            );

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Database(db =>
            {
                var dbProduct = db.Products.First(x => x.Id == product.Id);

                Assert.Equal("Updated", dbProduct.Name);
                Assert.Equal(20, dbProduct.Price);
            });
        }
    }
}
