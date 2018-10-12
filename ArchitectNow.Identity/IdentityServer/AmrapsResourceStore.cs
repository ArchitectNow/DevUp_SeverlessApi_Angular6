using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectNow.Mongo.Identity.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace ArchitectNow.Identity.IdentityServer
{
    public class AmrapsResourceStore : IResourceStore
    {
        private readonly IMongoIdentityRepository _repository;

        public AmrapsResourceStore(IMongoIdentityRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var results = await _repository.FindAsync<IdentityResource>(a => scopeNames.Contains(a.Name));
            return results.ToList();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var results = await _repository.FindAsync<ApiResource>(a => a.Scopes.Any(s => scopeNames.Contains(s.Name)));
            return results.ToList();
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            return _repository.SingleAsync<ApiResource>(a => a.Name == name);
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiTask = _repository.AllAsync<ApiResource>();
            var identityTask = _repository.AllAsync<IdentityResource>();
            Task.WaitAll(apiTask, identityTask);
            var result = new Resources(await identityTask, await apiTask);
            return result;
        }
    }
}