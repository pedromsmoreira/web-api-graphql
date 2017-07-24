namespace WebApiGraphQL.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Options;
    using Middleware;
    using Middleware.GraphiQl;
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

        public static IApplicationBuilder UseGraphiQl(this IApplicationBuilder builder, GraphiQlOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.UseMiddleware<GraphiQlMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseGraphiQl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GraphiQlMiddleware>();
        }
    }
}