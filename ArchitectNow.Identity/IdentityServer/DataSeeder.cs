using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ArchitectNow.Mongo.Identity;
using ArchitectNow.Mongo.Identity.Repositories;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace ArchitectNow.Identity.IdentityServer
{
    public static class DataSeeder
    {
        public static IdentityBuilder SeedUserData(this IdentityBuilder builder)
        {
            var provider = builder.Services.BuildServiceProvider();
            var repository = provider.GetService<IMongoIdentityRepository>();
            
            var userCollection = repository.GetDatabase()
                .GetCollection<AppUser>(AppIndentityConstants.Mongo.IdentityUserCollectionName);
            
            var user = userCollection.AsQueryable().Where(u => u.NormalizedUserName == "JEFF_AMRAPS").SingleOrDefault();
            if (user != null)
                return builder;
            
            var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "jeff_amraps",
                    AccessFailedCount = 0,
                    ConcurrencyStamp = "9e3fc90b-9a70-4178-a43b-d3ae05379018",
                    Email = "jeff_amraps@amraps.local",
                    EmailConfirmed = true, 
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    NormalizedEmail = "JEFF_AMRAPS@AMRAPS.LOCAL",
                    NormalizedUserName = "JEFF_AMRAPS",
                    PasswordHash = "AQAAAAEAACcQAAAAEE9T7Th+W50ABeTaBn726dhcaPS9T+tMx7YQ0EZ54IOayR9sea6d9lN/B4B3l7/6FA==",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false, 
                    SecurityStamp = "983d0920-8d30-45ec-be5f-494f1cefc56c",
                    TwoFactorEnabled = false,
                    Roles = new List<string> { "Administrator" },
                    Claims = new List<AppUserClaim>
                    { 
                        new AppUserClaim(new Claim(JwtClaimTypes.Subject, "amraps-portal")),
                        new AppUserClaim(new Claim(JwtClaimTypes.Name, "jeff_amraps", ClaimTypes.Name)),
                        new AppUserClaim(new Claim(JwtClaimTypes.GivenName, "Jeff")),
                        new AppUserClaim(new Claim(JwtClaimTypes.FamilyName, "St. Germain")),
                        new AppUserClaim(new Claim(JwtClaimTypes.Email, "jeff_amraps@amraps.local"))
                    }
                }
            };
            userCollection.InsertMany(users);
            return builder;
        }
    }
}