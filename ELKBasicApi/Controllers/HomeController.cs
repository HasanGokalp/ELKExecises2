using Elastic.Clients.Elasticsearch.IndexManagement;
using Microsoft.AspNetCore.Mvc;

namespace ELKBasicApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("OKEY");
        }
    }
}
