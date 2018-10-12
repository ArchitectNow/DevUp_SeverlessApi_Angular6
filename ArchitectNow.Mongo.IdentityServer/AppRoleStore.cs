using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ArchitectNow.Mongo.Identity.Repositories;

namespace ArchitectNow.Mongo.Identity
{
    public class AppRoleStore<TRole> : IRoleStore<TRole> where TRole : AppRole
    {
        private readonly IMongoIdentityRepository _repository;
        private readonly IMongoCollection<TRole> _rolesCollection;


        public AppRoleStore(IMongoIdentityRepository repository)
        {
            _repository = repository;
            _rolesCollection = _repository.GetDatabase()
                .GetCollection<TRole>(AppIndentityConstants.Mongo.IdentityRoleCollectionName);
        }
        
        public void Dispose()
        {
            //Not needed.
        }

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await _rolesCollection.InsertOneAsync(role, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            var result = await _rolesCollection.ReplaceOneAsync(r => r.Id == role.Id, role, cancellationToken: cancellationToken);
            return result.IsAcknowledged ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            var result = await _rolesCollection.DeleteOneAsync(r => r.Id == role.Id, cancellationToken);
            return result.IsAcknowledged ? IdentityResult.Success : IdentityResult.Failed();
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName.ToUpperInvariant();
            return Task.FromResult(0);
        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return _rolesCollection.Find(r => r.Id == roleId).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return _rolesCollection.Find(r => r.NormalizedName == normalizedRoleName.ToUpperInvariant())
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}