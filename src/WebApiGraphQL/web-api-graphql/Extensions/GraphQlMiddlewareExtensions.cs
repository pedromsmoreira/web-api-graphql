namespace WebApiGraphQL.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;
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

        public static IApplicationBuilder UseGraphiQL(this IApplicationBuilder app, GraphiQlOptionsV2 optionsV2)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (optionsV2 == null)
            {
                throw new ArgumentNullException(nameof(optionsV2));
            }
            return app.UseMiddleware<GraphiQlMiddlewareV2>(Options.Create(optionsV2));
        }
    }
}