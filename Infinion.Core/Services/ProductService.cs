using Infinion.Core.Abstractions;
using Infinion.Core.DTOs;
using Infinion.Core.Utilities;
using Infinion.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infinion.Core.Services
{

    public class ProductService : IProductService
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public ProductService(IRepository repository, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<Result<CreateProductResponseDto>> CreateProductAsync(string userId, CreateProductDto productDto)
        {
            var result = new Result<CreateProductResponseDto>();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var newProduct = new Product
                {
                    Name = productDto.Name,
                    Category = productDto.Category,
                    Brand = productDto.Brand,
                    PhotoURL = productDto.PhotoURL,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    AddedBy = $"{user.FirstName} {user.LastName}"
                };
                await _repository.Add<Product>(newProduct);
                await _unitOfWork.SaveChangesAsync();
                var productResponse = new CreateProductResponseDto()
                {
                    Name = newProduct.Name,
                    Category = newProduct.Category,
                    Brand = newProduct.Brand,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    AddedBy = newProduct.AddedBy,
                };
                result.IsSuccess = true;
                result.Message = "Product Added Successfully";
                result.Content = productResponse;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(string productId)
        {
            var result = new Result<ProductDto>();
            try
            {

                var product = _repository.GetAll<Product>().FirstOrDefault(x => x.Id == productId);

                if (product == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Product not found";
                    return result;
                }

                var productToReturn = new ProductDto
                {
                    Id = productId,
                    Name = product.Name,
                    Category = product.Category,
                    Brand = product.Brand,
                    Description = product.Description,
                    Price = product.Price,
                    AddedBy = product.AddedBy,
                    PhotoURL = product.PhotoURL,
                };

                result.IsSuccess = true;
                result.Message = "Product retrieved successfully";
                result.Content = productToReturn;

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<Result<PaginatorDto<IEnumerable<ProductDto>>>> GetAllProductsAync(PaginationFilter paginationFilter)
        {
            var result = new Result<PaginatorDto<IEnumerable<ProductDto>>>();
            try
            {

                var products = _repository.GetAll<Product>();

                if (products.Count() < 1)
                {
                    result.IsSuccess = false;
                    result.Message = "Product(s) not found";
                    return result;
                }

                var productListToReturn = await products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Brand = p.Brand,
                    Description = p.Description,
                    Price = p.Price,
                    AddedBy = p.AddedBy,
                    PhotoURL = p.PhotoURL,

                }).Paginate(paginationFilter);

                result.IsSuccess = true;
                result.Message = "Products retrieved successfully";
                result.Content = productListToReturn;



            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<Result> DeleteProductAsync(string productId)
        {
            var result = new Result();
            try
            {

                var product = _repository.GetAll<Product>().First(p => p.Id == productId);
                if (product == null)
                {
                    result.IsSuccess = false;
                    result.Message = "product not found";
                    return result;
                }


                _repository.Remove<Product>(product);
                await _unitOfWork.SaveChangesAsync();

                result.IsSuccess = true;
                result.Message = "Product deleted successfully";

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<Result<UpdateProductDto>> UpdateProductAsync( string productId, UpdateProductDto updateProductDto)
        {
            var result = new Result<UpdateProductDto>();
            try
            {
                var product = _repository.GetAll<Product>().FirstOrDefault(p=>p.Id==productId);

                if (product == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Product not found";
                    return result;
                }

                product.Name = updateProductDto.Name ?? product.Name;
                product.Description = updateProductDto.Description ?? product.Description;
                product.Price = updateProductDto.Price ?? product.Price;
                product.Brand = updateProductDto.Brand ?? product.Brand;
                product.Category= updateProductDto.Category ?? product.Category;
                product.PhotoURL=updateProductDto.PhotoURL ?? product.PhotoURL;

                 _repository.Update<Product>(product);
                await _unitOfWork.SaveChangesAsync();

                var updatedProduct = new UpdateProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Brand = product.Brand,
                    Category = product.Category,
                    PhotoURL = product.PhotoURL,
                    
                };

                result.IsSuccess = true;
                result.Message = "Product updated successfully";
                result.Content = updatedProduct;


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

    }
}
