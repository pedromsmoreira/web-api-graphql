namespace WebApi.Controllers
{
    using System.Web.Http;
    using Attributes;
    using GraphQL;
    using GraphQL.Http;
    using Newtonsoft.Json.Linq;
    using Queries;

    public class GraphController : ApiController
    {
        [HttpPost]
        [Route("~/api/graph")]
        [GraphType(typeof(BooksQuery))]
        public IHttpActionResult Post([FromBody] ExecutionResult result)
        {
            var json = new DocumentWriter(indent: true).Write(result);

            return this.Ok(JObject.Parse(json));
        }
    }
}