using Infinion.Core.Abstractions;
using Infinion.Core.DTOs;
using Infinion.Domain.Constants;
using Infinion.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Infinion_Sadiq.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(IProductService productService, UserManager<AppUser> userManager)
        {
            _productService = productService;
            _userManager=userManager;
        }
        [HttpPost]
        [Authorize(Roles =RoleConstant.Admin)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var result = await _productService.CreateProductAsync(userId, productDTO);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("get_product_by_Id/{productId}")]
        public async Task<IActionResult> GetProductById(string productId)
        {
            
            var result = await _productService.GetProductByIdAsync(productId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("get_all_products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] PaginationFilter? paginationFilter = null)
        {
            paginationFilter ??= new PaginationFilter();
            var result = await _productService.GetAllProductsAync(paginationFilter);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("delete_product/{productId}")]
        [Authorize(Roles =RoleConstant.Admin)]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
           
            var result = await _productService.DeleteProductAsync(productId);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPatch("update_product/{productId}")]
        [Authorize(Roles =RoleConstant.Admin)]
        public async Task<IActionResult> UpdateProduct(string productId, UpdateProductDto updateProductDto)
        {

            var result = await _productService.UpdateProductAsync(productId, updateProductDto);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
