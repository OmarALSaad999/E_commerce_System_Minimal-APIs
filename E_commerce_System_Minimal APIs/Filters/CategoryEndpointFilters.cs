using E_commerce_System_Minimal_APIs.Requests;

namespace E_commerce_System_Minimal_APIs.Filters
{

    public class CategoryEndpointFilters
    {

        public static async ValueTask<object?> ValidateUpdateRequest(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.GetArgument<CategoryRequest>(2);
            return await ValidateRequest(request, context, next);
        }
        public static async ValueTask<object?> ValidateCreateRequest(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.GetArgument<CategoryRequest>(1);
            return await ValidateRequest(request, context, next);
        }
        private static async ValueTask<object?> ValidateRequest(CategoryRequest request, EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"Name", new [] {"Name should not be empty"} },
                });
            }
            return await next(context);
        }
    }
}
