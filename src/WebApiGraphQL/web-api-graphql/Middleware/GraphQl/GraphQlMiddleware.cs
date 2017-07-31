namespace WebApiGraphQL.Middleware.GraphQl
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using GraphQL;
    using GraphQL.Http;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public sealed class GraphQlMiddleware
    {
        private readonly string Path;
        private readonly RequestDelegate Next;
        private readonly ISchema Schema;

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

            // Duplicated code in GraphiQlMiddleware too! Optimize later.
            this.Next = next ?? throw new ArgumentNullException(nameof(next));
            var graphqlOptions = options.Value;

            this.Path = string.IsNullOrEmpty(graphqlOptions?.GraphQlPath)
                ? GraphQlOptions.DefaultPath
                : graphqlOptions.GraphQlPath;

            this.Schema = graphqlOptions?.Schema;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!this.IsGraphQlRequest(context))
            {
                await this.Next(context).ConfigureAwait(false);
                return;
            }

            await this.ExecuteAsync(context);
        }

        private bool IsGraphQlRequest(HttpContext context)
        {
            var validHttpMethod = string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
            var validRequestPath = context.Request.Path.StartsWithSegments(this.Path);

            return validHttpMethod && validRequestPath;
        }

        private async Task WriteResponseAsync(HttpContext context, ExecutionResult executionResult)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (executionResult.Errors?.Count ?? 0) == 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;

            var graphqlResponse = new DocumentWriter().Write(executionResult);

            await context.Response.WriteAsync(graphqlResponse);
        }

        private async Task ExecuteAsync(HttpContext context)
        {
            string query;

            using (var streamReader = new StreamReader(context.Request.Body))
            {
                query = await streamReader.ReadToEndAsync().ConfigureAwait(true);
            }

            var request = this.TryDeserializeGraphiQlRequest(query);

            var executionResult = await this.FilterRequest(request, query);

            await this.WriteResponseAsync(context, executionResult);
        }

        // This method is here only because of graphiql explorer!!! Should be done in another way, but I'm too lazy for it.
        // This works for now!!! :) :P
        // To avoid this we should create GraphQLController  and make GraphiQL Explorer call the controller endpoint
        private GraphiQlRequest TryDeserializeGraphiQlRequest(string query)
        {
            GraphiQlRequest request = null;

            try
            {
                request = JsonConvert.DeserializeObject<GraphiQlRequest>(query);
            }
            catch (Exception)
            {
                // muahahahhaha does nothing! Surprise :)
            }

            return request;
        }

        private async Task<ExecutionResult> FilterRequest(GraphiQlRequest request, string query)
        {
            if (request != null)
            {
                return await new DocumentExecuter().ExecuteAsync(options =>
                {
                    options.Schema = this.Schema;
                    options.Query = request.Query;
                    options.OperationName = request.OperationName;
                    options.Inputs = request.Variables.ToInputs();
                });
            }

            return await new DocumentExecuter().ExecuteAsync(options =>
            {
                options.Schema = this.Schema;
                options.Query = query;
            });
        }
    }
}