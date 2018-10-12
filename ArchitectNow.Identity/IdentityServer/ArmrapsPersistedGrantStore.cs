using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectNow.Mongo.Identity.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace ArchitectNow.Identity.IdentityServer
{
    public class AmrapsPersistedGrantStore : IPersistedGrantStore
    {
        private readonly IMongoIdentityRepository _repository;

        public AmrapsPersistedGrantStore(IMongoIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var result = await _repository.FindAsync<PersistedGrant>(i => i.SubjectId == subjectId);
            return result.ToList();
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            var result = _repository.Single<PersistedGrant>(i => i.Key == key);
            return Task.FromResult(result);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            _repository.Delete<PersistedGrant>(i => i.SubjectId == subjectId && i.ClientId == clientId);
            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            _repository.Delete<PersistedGrant>(i => i.SubjectId == subjectId && i.ClientId == clientId && i.Type == type);
            return Task.FromResult(0);
        }

        public Task RemoveAsync(string key)
        {
            _repository.Delete<PersistedGrant>(i => i.Key == key);
            return Task.FromResult(0);
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            return _repository.AddAsync(grant);
        }
    }
}