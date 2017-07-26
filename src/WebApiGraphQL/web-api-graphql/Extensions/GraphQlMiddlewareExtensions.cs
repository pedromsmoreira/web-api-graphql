namespace WebApiGraphQL.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;
    using Middleware.GraphQl;

    public static class GraphQlMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GraphQlMiddleware>();
        }

        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder, GraphQlOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.UseMiddleware<GraphQlMiddleware>(Options.Create(options));
        }
    }
}