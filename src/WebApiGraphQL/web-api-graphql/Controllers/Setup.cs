using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiGraphQL.Controllers
{
    using Microsoft.Extensions.Caching.Distributed;

    [Route("api/[controller]")]
    public class Setup : Controller
    {
        private readonly IDistributedCache DistributedCache;

        public Setup(IDistributedCache distributedCache)
        {
            this.DistributedCache = distributedCache;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            
        }
    }
}