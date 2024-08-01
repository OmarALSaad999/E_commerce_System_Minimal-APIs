using E_commerce_System_Minimal_APIs.Data;
using E_commerce_System_Minimal_APIs.Filters;
using E_commerce_System_Minimal_APIs.Requests;
using E_commerce_System_Minimal_APIs.Services.Implementation;
using E_commerce_System_Minimal_APIs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



//////////////////////////////////// Products //////////////////////////////////////
var products = app.MapGroup("/products").WithTags("Products");


//Create a new product
products.MapPost("/", async (IProductService productService, ProductRequest request)
    => await productService.CreateProduct(request))
    .AddEndpointFilter(new ProductEndpointFilters().ValidateCreateRequest);

//Get a product via Id
products.MapGet("/{id}", async (IProductService productService, int id)
    => await productService.GetProductById(id));

//Update a product via Id
products.MapPut("/{id}", async (IProductService productService, int id, ProductRequest request)
    => await productService.UpdateProduct(id, request))
    .AddEndpointFilter(new ProductEndpointFilters().ValidateUpdateRequest);

//Delete a product
products.MapDelete("/{id}", async (IProductService productService, int id)
    => await productService.DeleteProduct(id));

//Get the average proce of all products

products.MapGet("/average", async (IProductService productService)
    => await productService.GetAverage());

//Filter products based on many values
products.MapGet("/", async (IProductService productService, decimal? minPrice, decimal? maxPrice,
            string? categoryName, string? productName, string? productDescription)
            => await productService.GetAllProducts(minPrice, maxPrice, categoryName, productName, productDescription));


//////////////////////////////// Categories ///////////////////////////////////////////
var categories = app.MapGroup("/categories").WithTags("Categories");
//Get all categories
categories.MapGet("/", async (ICategoryService categoryService)
    => await categoryService.GetAllCategories());

//Get category by Id
categories.MapGet("/{id}", async (ICategoryService categoryService, int id)
    => await categoryService.GetCategoryById(id));
//Create a category
categories.MapPost("/", async (ICategoryService categoryService, CategoryRequest request)
    => await categoryService.CreateCategory(request))
    .AddEndpointFilter(CategoryEndpointFilters.ValidateCreateRequest);
//Update a category
categories.MapPut("/{id}", async (ICategoryService categoryService, int id, CategoryRequest request)
    => await categoryService.UpdateCategory(id, request))
    .AddEndpointFilter(CategoryEndpointFilters.ValidateUpdateRequest);
//Delete a category
categories.MapDelete("/{id}", async (ICategoryService categoryService, int id)
    => await categoryService.DeleteCategory(id));

//Get the total number of products in each category
categories.MapGet("/countOfProducts", async (ICategoryService categoryService)
    => await categoryService.GetCategoriesWithProductNumbers());

app.Run();