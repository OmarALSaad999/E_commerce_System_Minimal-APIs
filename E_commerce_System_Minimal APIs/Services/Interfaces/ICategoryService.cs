using E_commerce_System_Minimal_APIs.Requests;

namespace E_commerce_System_Minimal_APIs.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IResult> GetAllCategories();
        Task<IResult> GetCategoryById(int id);
        Task<IResult> UpdateCategory(int id, CategoryRequest request);
        Task<IResult> CreateCategory(CategoryRequest request);
        Task<IResult> GetCategoriesWithProductNumbers();
        Task<IResult> DeleteCategory(int id);
    }
}
