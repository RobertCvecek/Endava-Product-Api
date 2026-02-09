using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Endava.Products.Data.Dtos;
using Endava.Products.Data.Interfaces;
using Endava.Products.Data.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Endava.Products.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(
        IProductsRepository productsRepository,
        ICategoriesRepository categoriesRepository
    ) : ControllerBase
    {
        /// <summary>
        /// Returns filtered products
        /// </summary>
        [HttpGet(Name = "fetchProduct")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PlaceOrder([FromQuery] ProductsFilterRequest filter)
        {
            if (filter.MinPrice > filter.MaxPrice)
            {
                return BadRequest("MinPrice cannot be greater than MaxPrice.");
            }

            var products = await productsRepository.Fetch(filter);
            return Ok(products);
        }

        /// <summary>
        /// Updates product name and/or price
        /// </summary>
        [HttpPut("{id:guid}", Name = "updateProduct")]
        public async Task<IActionResult> UpdateProduct(
            [FromRoute] Guid id,
            [FromBody] UpdateProductRequest dto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await productsRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            if (dto.Name != null)
            {
                product.Name = dto.Name;
            }

            if (dto.Price.HasValue)
            {
                product.Price = dto.Price.Value;
            }

            if (dto.CategoryId != null)
            {
                var category = await categoriesRepository.GetById(dto.CategoryId.Value);
                if (category is null)
                {
                    return BadRequest("Category with given ID does not exist");
                }

                product.Category = category;
            }

            await productsRepository.Update(product);

            return NoContent();
        }
    }
}
