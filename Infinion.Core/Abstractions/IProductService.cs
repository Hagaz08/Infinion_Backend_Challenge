using Infinion.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinion.Core.Abstractions
{
    public interface IProductService
    {
        Task<Result<CreateProductResponseDto>> CreateProductAsync(string userId, CreateProductDto productDto);
        Task<Result<ProductDto>> GetProductByIdAsync(string productId);
        Task<Result<PaginatorDto<IEnumerable<ProductDto>>>> GetAllProductsAync(PaginationFilter paginationFilter);
        Task<Result> DeleteProductAsync(string productId);
        Task<Result<UpdateProductDto>> UpdateProductAsync(string productId, UpdateProductDto updateProductDto);
    }
}

