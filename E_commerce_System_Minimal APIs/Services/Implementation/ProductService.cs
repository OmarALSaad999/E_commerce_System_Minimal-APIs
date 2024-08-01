using E_commerce_System_Minimal_APIs.Data;
using E_commerce_System_Minimal_APIs.Models;
using E_commerce_System_Minimal_APIs.Requests;
using E_commerce_System_Minimal_APIs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_System_Minimal_APIs.Services.Implementation
{

    public class ProductService : IProductService
    {


        private readonly DataContext _context;

        public ProductService(DataContext context) //Inject an instance of Data Context using  DI Container
        {
            _context = context;
        }
        public async Task<IResult> CreateProduct(ProductRequest request) //It is recommended to use another type class rather than Product class that we want to add to the DB,
                                                                         //as we may receive more or less info of Product properties via the request
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                //Here we are sure that the CategoryId exists in the Categories table
                //A filter has been implemented for that. ProductEndpointFilters().ValidateCreateRequest
                CategoryId = request.CategoryId,
                //WE can add a piece of code that searches the Category entity from DB having categoryId.
                //Then  initialize navigation property Category by the found reference
                Category = null,
            };

            await _context.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return Results.Created($"/products/{newProduct.ProductId}", newProduct);
        }

        public async Task<IResult> GetProductById(int id)
        {
            //In Entity Framework(EF) Core, the Include method is used to load related entities or collections of entities along with the main entities.
            //In our case, when fetching the product havin id, we will load as well the related property of type Category
            var product = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.ProductId == id);


            //var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id); // It works fine as well
            //await _context.Entry(product).Reference(x => x.Category).LoadAsync();

            //if you remove the Include method from your query, it will still work, but the Category navigation property of the Product entity will not be loaded eagerly.
            //This means that when you access product.Category later in your code, Entity Framework will issue a separate query to load the Category entity associated with the Product.
            //Another solution is to declare Category as virtual in Product class and enable lazy loading in DBContext
            //public virtual Category Category { get; set; }

            if (product is null)
            {
                return Results.NotFound("Product Not Found");
            }
            return Results.Ok(product);
        }

        public async Task<IResult> UpdateProduct(int id, ProductRequest request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (product is null)
            {
                return Results.NotFound("Product Not Found");
            }

            UpdateProduct(product, request.Name, request.Description, request.Price, request.CategoryId);
            await _context.SaveChangesAsync();
            return Results.Ok(product);
        }
        public void UpdateProduct(Product product, string name, string description, decimal price, int categoryId)
        {
            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.CategoryId = categoryId;
        }

        public async Task<IResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (product is null)
            {
                return Results.NotFound("Product Not Found.");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Results.Ok("Product is deleted.");
        }

        public async Task<IResult> GetAllProducts(decimal? minPrice, decimal? maxPrice,
                string? categoryName, string? productName, string? productDescription)
        {
            var query = _context.Products
                .Include(x => x.Category)
                .AsQueryable();
            if (minPrice is not null)
            {
                query = query.Where(x => x.Price >= minPrice);
            }
            if (maxPrice is not null)
            {
                query = query.Where(x => x.Price <= maxPrice);
            }
            if (categoryName is not null)
            {
                query = query.Where(x => x.Category.Name.Contains(categoryName));
            }
            if (productName is not null)
            {
                query = query.Where(x => x.Name.Contains(productName));
            }
            if (productDescription is not null)
            {
                query = query.Where(x => x.Description.Contains(productDescription));
            }
            return Results.Ok(await query.ToListAsync());
        }

        public async Task<IResult> GetAverage()
        {
            var count = await _context.Products.CountAsync();
            if (count == 0)
            {
                return Results.Ok(0);
            }
            var average = await _context.Products.AverageAsync(x => x.Price);
            return Results.Ok(average);
        }




    }
}