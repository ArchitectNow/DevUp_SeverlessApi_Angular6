using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using ArchitectNow.Mongo.Identity;

namespace ArchitectNow.Identity.IdentityServer {
    
    public class IdentityConfig : IIdentityConfig<AppUser, AppRole>
    {
        private readonly IdentityServerConfiguration _identityConfig;

        public IdentityConfig(
            IOptions<IdentityServerConfiguration> configOptions)
        {
            _identityConfig = configOptions.Value;
        }
        
        public List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    Enabled = true,
                    ClientId = "amraps-portal",
                    ClientUri = _identityConfig.BaseUrls.Portal,
                    ClientName = "Amraps Portal",
                    ClientSecrets = 
                    {
                        new Secret("8f5b733a04384fdf82b4a7fd07cc7e58e3591e6d91474990a24c976850ca4f55".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris =
                    {
                        $"{_identityConfig.BaseUrls.Portal}/signin-oidc",
                        $"{_identityConfig.BaseUrls.Portal}/assets/silent-refresh.html"
                    },
                    PostLogoutRedirectUris = { $"{_identityConfig.BaseUrls.Portal}" },
                    FrontChannelLogoutUri = $"{_identityConfig.BaseUrls.Portal}/signout-oidc",
                    AllowedCorsOrigins = { _identityConfig.BaseUrls.Portal },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "portal-api"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600,
                    AllowOfflineAccess = true
                }
            };
        }

        public List<ApiResource> GetApiResources()
        {
            var seed = new List<ApiResource>
            {
                new ApiResource("portal-api", "Amraps Portal Api")
            };
            return seed;
        }

        public List<IdentityResource> GetIdentityResources()
        {
            var seed = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

            return seed;
        }
        
        public List<AppUser> GetInitialUsers()
        {
            return new List<AppUser>
            {
                new AppUser
                {
                    UserName = "jeffs@mailinator.com",
                    LockoutEnabled = false,
                    EmailConfirmed = true,
                    NormalizedEmail = "JEFFS@MAILINATOR.COM",
                    NormalizedUserName = "JEFFS@MAILINATOR.COM",
                    PasswordHash = "AQAAAAEAACcQAAAAELFfuTmP1wvjrd5w9kXvdw3UnI4ynWXkJCtiDU7IPRCzfpwSiStJ3bClWca6Ontr0A==",
                    SecurityStamp = "7fd5475b-2666-4997-aa8b-3a482f0caa6e",
                    ConcurrencyStamp = "594e8a07-833a-41d9-a9e0-fff0ad50ae1a",
                    AccessFailedCount = 0,
                    
                    Claims = new List<AppUserClaim>
                    {
                        new AppUserClaim(new Claim(JwtClaimTypes.Name, "jeffs@mailinator.com")),
                        new AppUserClaim(new Claim(JwtClaimTypes.GivenName, "Jeff")),
                        new AppUserClaim(new Claim(JwtClaimTypes.FamilyName, "St. Germain")),
                        new AppUserClaim(new Claim(JwtClaimTypes.Email, "jeffs@mailinator.com"))
                    }
                }
            };
        }
        
        public List<AppRole> GetRoles()
        {
            return new List<AppRole>
            {
                new AppRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new AppRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            };
        }
        
    }
}