using E_commerce_System_Minimal_APIs.Data;
using E_commerce_System_Minimal_APIs.Models;
using E_commerce_System_Minimal_APIs.Requests;
using E_commerce_System_Minimal_APIs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_System_Minimal_APIs.Services.Implementation
{

    public class CategoryService : ICategoryService
    {

        private readonly DataContext _context;

        public CategoryService(DataContext context) //DI container will inject an instance of DataContext
        {
            _context = context;
        }

        public async Task<IResult> GetAllCategories()
        {
            return Results.Ok(await _context.Categories.ToListAsync());
        }

        public async Task<IResult> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category is null)
            {
                return Results.NotFound("Category not found");
            }
            return Results.Ok(category);
        }
        public async Task<IResult> CreateCategory(CategoryRequest request)
        {
            var newCategory = new Category
            {
                Name = request.Name, //Using the filter we have validated that the received Name is not null. CategoryEndpointFilters.ValidateCreateRequest
            };
            await _context.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return Results.Created($"/categories/{newCategory.CategoryId}", newCategory);
        }
        public async Task<IResult> UpdateCategory(int id, CategoryRequest request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category is null)
            {
                return Results.NotFound("Category not found");
            }
            UpdateCategory(category, request.Name);//Using the filter we have validated that the received Name is not null. CategoryEndpointFilters.ValidateUpdateRequest
            await _context.SaveChangesAsync();
            return Results.Ok(category);
        }
        public void UpdateCategory(Category category, string name)
        {
            category.Name = name;
        }
        public async Task<IResult> DeleteCategory(int id)
        {
            var cat = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (cat is null)
            {
                return Results.NotFound("Category does not exist");
            }
            _context.Categories.Remove(cat);
            await _context.SaveChangesAsync();
            return Results.Ok("Category and its related products have been deleted.");
        }

        public async Task<IResult> GetCategoriesWithProductNumbers()
        {
            var categories = await _context.Categories
                .Select(x => new
                {
                    Id = x.CategoryId,
                    Name = x.Name,
                    productCount = _context.Products.Count(p => p.CategoryId == x.CategoryId)
                })
                .ToListAsync();
            return Results.Ok(categories);
        }
    }
}