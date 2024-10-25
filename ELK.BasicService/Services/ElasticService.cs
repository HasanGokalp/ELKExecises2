using ELK.BasicService.Const;
using ELK.BasicService.Entities;
using Microsoft.Extensions.Options;
using Nest;

namespace ELK.BasicService.Services
{
    public class ElasticService : IElasticService
    {
        private readonly ElasticSettings _elasticSettings;
        private readonly ElasticClient _elasticClient;

        public ElasticService(IOptions<ElasticSettings> options)
        {
            _elasticSettings = options.Value;

            var settings = new ConnectionSettings(new Uri(_elasticSettings.Url))
                .DefaultIndex(_elasticSettings.DefaultIndex);
                //.BasicAuthentication(_elasticSettings.Username, _elasticSettings.Password);

            _elasticClient = new ElasticClient(settings);
        }

        public async Task<bool> AddOrUpdate(Log log)
        {
            //var response = await _elasticClient.IndexDocumentAsync(log);

            var response = await _elasticClient.IndexAsync(log, idx => idx.Index(_elasticSettings.DefaultIndex).OpType(Elasticsearch.Net.OpType.Index));

            return response.IsValid;
        }

        public async Task<bool> AddOrUpdateBulkAsync(IEnumerable<Log> logs, string indexname)
        {
            //var response = _elasticClient.IndexMany(logs, indexname);

            var response = await _elasticClient.BulkAsync(b => b.Index(_elasticSettings.DefaultIndex).UpdateMany(logs, (ud, u) => ud.Doc(u).DocAsUpsert(true)));

            return response.IsValid;
        }

        public Task CreateIndexIfNotExistsAsync<T>(string indexName) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task CreateIndexIfNotExistsAsync(string indexName)
        {
            if (! _elasticClient.Indices.Exists(indexName).Exists)
            {
                await _elasticClient.Indices.CreateAsync(indexName);
            }
        }

        public async Task<Log> Get(string key)
        {
            var response = await _elasticClient.GetAsync<Log>(key, g => g.Index(_elasticSettings.DefaultIndex));

            return await Task.FromResult(response.Source);
        }

        public async Task<IEnumerable<Log>> GetAll()
        {
            var response = await _elasticClient.SearchAsync<Log>(s => s.Index(_elasticSettings.DefaultIndex));

            return response.IsValid ? response.Documents.ToList() : default;
        }
    }
}
