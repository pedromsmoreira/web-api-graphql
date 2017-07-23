namespace WebApiGraphQL.Middleware.GraphQl
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using GraphQL;
    using GraphQL.Http;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public sealed class GraphQlMiddleware
    {
        private readonly string path;
        private readonly RequestDelegate next;
        private readonly ISchema schema;

        public GraphQlMiddleware(RequestDelegate next, IOptions<GraphQlOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Value?.Schema == null)
            {
                throw new ArgumentException("Schema is null!");
            }

            this.next = next ?? throw new ArgumentNullException(nameof(next));
            var graphqlOptions = options.Value;

            this.path = string.IsNullOrEmpty(graphqlOptions?.GraphQlPath)
                ? GraphQlOptions.DefaultPath
                : graphqlOptions.GraphQlPath;

            this.schema = graphqlOptions?.Schema;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var validHttpMethod = string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
            var validRequestPath = context.Request.Path.Equals(this.path);

            if (validHttpMethod && validRequestPath)
            {
                var executionResult = await ExecuteAsync(context.Request).ConfigureAwait(true);
                await WriteResponseAsync(context.Response, executionResult).ConfigureAwait(true);
                return;
            }

            await this.next(context).ConfigureAwait(true);
        }

        private static Task WriteResponseAsync(HttpResponse response, ExecutionResult executionResult)
        {
            response.ContentType = "application/json";
            response.StatusCode = (executionResult.Errors?.Count ?? 0) == 0 ? 200 : 400;

            var graphqlResponse = new DocumentWriter().Write(executionResult);

            return response.WriteAsync(graphqlResponse);
        }

        private async Task<ExecutionResult> ExecuteAsync(HttpRequest request)
        {
            string query;

            using (var streamReader = new StreamReader(request.Body))
            {
                query = await streamReader.ReadToEndAsync().ConfigureAwait(true);
            }

            return await new DocumentExecuter().ExecuteAsync(options =>
            {
                options.Schema = this.schema;
                options.Query = query;
            }).ConfigureAwait(true);
        }
    }

    
}