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
    public class ProductsController(IProductsRepository productsRepository) : ControllerBase
    {
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
    }
}
