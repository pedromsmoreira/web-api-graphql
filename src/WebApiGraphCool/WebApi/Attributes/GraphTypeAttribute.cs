namespace WebApi.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using GraphQL;
    using GraphQL.Types;

    public class GraphTypeAttribute : FilterAttribute, IActionFilter
    {
        private readonly string parameterName;
        private readonly Type parameterType;

        public GraphTypeAttribute(Type type, string parameterName = null)
        {
            this.parameterName = parameterName;
            this.parameterType = type;
        }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            string query = Convert.ToString(actionContext.ActionArguments.ToArray().First().Value);
            var result = await new DocumentExecuter().ExecuteAsync((options) =>
            {
                options.Schema = new Schema() { Query = (IObjectGraphType)Activator.CreateInstance(parameterType) };
                options.Query = query;
            });
            actionContext.ActionArguments[parameterName ?? actionContext.ActionArguments.ToArray().First().Key] = result;
            return await continuation();
        }

        private void _checkForErrors(ExecutionResult result)
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