using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ArchitectNow.Mongo.Identity
{
    public class AppUser : IdentityUser<BsonObjectId>
    {
        public AppUser()
        {
            Id = ObjectId.GenerateNewId();
            Roles = new List<string>();
            Logins = new List<AppUserLogin>();
            Claims = new List<AppUserClaim>();
            Tokens = new List<AppUserToken>();
        }
        
        public List<string> Roles { get; set; }
        public List<AppUserLogin> Logins { get; set; }
        public List<AppUserClaim> Claims { get; set; }
        public List<AppUserToken> Tokens { get; set; }
        
        [BsonExtraElements]
        public Dictionary<string, object> ExtraElements { get; set; }

        public bool HasPassword()
        {
            return !string.IsNullOrWhiteSpace(PasswordHash);
        }
        
        public string GetTokenValue(string provider, string name)
        {
            return GetToken(provider, name)?.Value;
        }
        
        public void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
        }
        
        public void SetToken(string provider, string name, string value)
        {
            var existingToken = GetToken(provider, name);
            if (existingToken != null)
            {
                existingToken.Value = value;
                return;
            }

            Tokens.Add(new AppUserToken
            {
                LoginProvider = provider,
                Name = name,
                Value = value
            });
        }
        
        private AppUserToken GetToken(string provider, string name)
        {
            return Tokens.FirstOrDefault(t => t.LoginProvider == provider && t.Name == name);
        }
    }
}