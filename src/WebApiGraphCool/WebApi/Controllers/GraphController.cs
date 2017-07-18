namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using GraphQL;
    using GraphQL.Http;
    using GraphQL.Types;
    using Newtonsoft.Json.Linq;
    using Queries;
    using Repositories;

    public class GraphController : ApiController
    {
        [HttpPost]
        [Route("~/api/graph")]
        public async Task<IHttpActionResult> Post([FromBody] string query)
        {
            var result = await new DocumentExecuter().ExecuteAsync((options) =>
            {
                options.Schema = new Schema { Query = new BooksQuery() };
                options.Query = query;
            });

            this.ErrorValidation(result);

            var json = new DocumentWriter(indent: true).Write(result);

            return this.Ok(JObject.Parse(json));
        }

        private void ErrorValidation(ExecutionResult result)
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