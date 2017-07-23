namespace WebApiGraphQL.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Middleware;

    public static class GraphCoolMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GraphCoolMiddleware>();
        }
    }
}