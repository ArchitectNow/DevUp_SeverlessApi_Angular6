using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ArchitectNow.Mongo.Identity;
using ArchitectNow.Mongo.Identity.Repositories;
using MongoDB.Bson;

namespace ArchitectNow.Identity.IdentityServer
{
    public class AmrapsProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMongoIdentityRepository _repository;
        
        public AmrapsProfileService(UserManager<AppUser> userManager, IMongoIdentityRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sUserId = _userManager.GetUserId(context.Subject);
            if(string.IsNullOrEmpty(sUserId))
                return Task.FromResult(0);
       
            var userCollection = _repository.GetDatabase()
                .GetCollection<AppUser>(AppIndentityConstants.Mongo.IdentityUserCollectionName);
            
            var user = userCollection.AsQueryable().SingleOrDefault(u => u.Id == BsonObjectId.Create(sUserId));
            if(user == null)
                return Task.FromResult(0);
            
            var roleNames = user.Roles.Select(r => r).ToList();
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.UserName, ClaimTypes.Name),
                new Claim(JwtClaimTypes.GivenName, user.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.GivenName)?.Value),
                new Claim(JwtClaimTypes.FamilyName, user.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.FamilyName)?.Value),
                new Claim(JwtClaimTypes.Email, user.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value)
            };
            claims.AddRange(roleNames.Select(name => new Claim(JwtClaimTypes.Role, name, ClaimTypes.Role)));
            context.IssuedClaims.AddRange(claims);
            
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var sUserId = _userManager.GetUserId(context.Subject);
            if (string.IsNullOrEmpty(sUserId))
                context.IsActive = false;

            context.IsActive = true;
            return Task.FromResult(0); 
        }
    }
}