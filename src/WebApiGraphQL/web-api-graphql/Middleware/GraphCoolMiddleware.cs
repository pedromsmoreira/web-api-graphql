namespace WebApiGraphQL.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using GraphQL;
    using GraphQL.Http;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Http;
    using Queries;
    using Repositories;

    public class GraphCoolMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IBookRepository bookRepository;

        public GraphCoolMiddleware(RequestDelegate next, IBookRepository bookRepository)
        {
            this.next = next;
            this.bookRepository = bookRepository;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sent = false;

            if (httpContext.Request.Path.StartsWithSegments("/graph"))
            {
                using (var streamReader = new StreamReader(httpContext.Request.Body))
                {
                    var query = await streamReader.ReadToEndAsync();

                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        // TODO: Investigate a way to have a Generic Query :) 
                        var schema = new Schema { Query = new BooksQuery(this.bookRepository) };

                        var result = await new DocumentExecuter()
                            .ExecuteAsync(options =>
                            {
                                options.Schema = schema;
                                options.Query = query;
                            })
                            .ConfigureAwait(false);

                        this.CheckForErrors(result);
                        
                        await this.WriteResult(httpContext, result);
                        sent = true;
                    }
                }
            }

            if (!sent)
            {
                await this.next(httpContext);
            }
        }

        private async Task WriteResult(HttpContext httpContext, ExecutionResult result)
        {
            var json = new DocumentWriter(indent: true).Write(result);
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(json);
        }

        private void CheckForErrors(ExecutionResult result)
        {
            if (result.Errors?.Count > 0)
            {
                var errors = new List<Exception>();
                foreach (var error in result.Errors)
                {
                    var ex = new Exception(error.Message);
                    if (error.InnerException != null)
                    {
                        ex = new Exception(error.Message, error.InnerException);
                    }
                    errors.Add(ex);
                }
                throw new AggregateException(errors);
            }
        }
    }
}