using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Services.IService
{
    public interface IProductService
    {
     
        Task<ResponseDto?> GetAllProductsAsync();
        
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductsAsync(ProductDto ProductDto);
        Task<ResponseDto?> UpdateProductsAsync(ProductDto ProductDto);
        Task<ResponseDto?> DeleteProductsAsync(int id);
    }
}
