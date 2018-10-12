using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using ArchitectNow.Mongo.Identity.Repositories;

namespace ArchitectNow.Identity.IdentityServer
{
    public class AmrapsClientStore : IClientStore
    {
        private readonly IMongoIdentityRepository _repository;
        
        public AmrapsClientStore(IMongoIdentityRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _repository.SingleAsync<Client>(c => c.ClientId == clientId);
            return client ?? null;
        }
    }
}