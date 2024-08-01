using E_commerce_System_Minimal_APIs.Data;
using E_commerce_System_Minimal_APIs.Requests;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_System_Minimal_APIs.Filters
{

    public class ProductEndpointFilters
    {

        public async ValueTask<object?> ValidateUpdateRequest(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.GetArgument<ProductRequest>(2);
            return await ValidateRequest(request, context, next);
        }
        public async ValueTask<object?> ValidateCreateRequest(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.GetArgument<ProductRequest>(1);
            return await ValidateRequest(request, context, next);
        }
        private async ValueTask<object?> ValidateRequest(ProductRequest request, EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (request.Price < 0 || string.IsNullOrEmpty(request.Name)
                || string.IsNullOrEmpty(request.Description))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"Price", new [] { "Price should be greater than 0"} },
                    {"Name", new [] {"Name should not be empty"} },
                    {"Description", new [] {"Description Should Not be empty" } }
                });
            }
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();
            var categoryExists = await dbContext.Categories.FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId);
            if (categoryExists is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"Category", new [] { "Category With this id does not exist"} },
                });
            }
            return await next(context);
        }
    }
}