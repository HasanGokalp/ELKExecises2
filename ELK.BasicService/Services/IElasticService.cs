using ELK.BasicService.Entities;

namespace ELK.BasicService.Services
{
    public interface IElasticService
    {
        Task CreateIndexIfNotExistsAsync<T>(string indexName) where T : class;

        Task CreateIndexIfNotExistsAsync(string indexName);

        Task<bool> AddOrUpdate(Log log);

        Task<bool> AddOrUpdateBulkAsync(IEnumerable<Log> logs, string indexname);

        Task<Log> Get(string key);

        Task<IEnumerable<Log>> GetAll();
    }
}
