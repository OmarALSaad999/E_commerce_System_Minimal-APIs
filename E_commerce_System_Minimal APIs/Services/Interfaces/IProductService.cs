using E_commerce_System_Minimal_APIs.Requests;

namespace E_commerce_System_Minimal_APIs.Services.Interfaces
{
    public interface IProductService
    {

        Task<IResult> GetAllProducts(decimal? minPrice, decimal? maxPrice,
            string? categoryName, string? productName, string? productDescription);
        Task<IResult> CreateProduct(ProductRequest request);
        Task<IResult> GetProductById(int id);
        Task<IResult> UpdateProduct(int id, ProductRequest request);
        Task<IResult> GetAverage();
        Task<IResult> DeleteProduct(int id);

    }
}
