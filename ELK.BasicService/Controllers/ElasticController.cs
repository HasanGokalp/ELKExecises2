using ELK.BasicService.Entities;
using ELK.BasicService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELK.BasicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticController : Controller
    {
        private readonly IElasticService _elasticService;
        public ElasticController(IElasticService elasticService)
        {
            _elasticService = elasticService;
        }

        [HttpPost("CreateIndex")]
        public async Task<IActionResult> Index(string str)
        {
            await _elasticService.CreateIndexIfNotExistsAsync(str);
            return Ok($"look at the localhost:9200/{str}");
        }

        [HttpPost("AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] Log log)
        {
            await _elasticService.AddOrUpdate(log);
            return Ok($"look at the localhost:9200/");
        }

        [HttpPost("AddOrUpdateBulk")]
        public async Task<IActionResult> AddOrUpdateBulk([FromBody] IEnumerable<Log> logs, string indexname)
        {
            await _elasticService.AddOrUpdateBulkAsync(logs, indexname);
            return Ok($"look at the localhost:9200/{indexname}");
        }

        [HttpPost("Get")]
        public async Task<IActionResult> Get(string key)
        {
            var log = await _elasticService.Get(key);
            return Ok(log);
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _elasticService.GetAll();
            return Ok(logs);
        }
    }
}
