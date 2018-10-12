using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ArchitectNow.ApiFunctions.Shared;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;


namespace ArchitectNow.ApiFunctions.Auth
{
    public static class TokenSecurity
    {
        public static async Task<ClaimsPrincipal> ValidateTokenAsync(this HttpRequestHeaders headers)
        {
            if (ValidateHeaders(headers, out var authHeader)) 
                return null;

            var issuer = Application.GetConfigValue<string>("issuer");
            var discoClient = await DiscoveryClient.GetAsync(issuer);
            
            return ValidateToken(discoClient, authHeader);
        }

        private static bool ValidateHeaders(HttpRequestHeaders headers, out AuthenticationHeaderValue authHeader)
        {
            authHeader = null;
            if (headers?.Authorization == null)
                return true;

            authHeader = headers.Authorization;
            return authHeader?.Scheme != "Bearer";
        }

        private static ClaimsPrincipal ValidateToken(DiscoveryResponse discoClient, AuthenticationHeaderValue authHeader)
        {
            var audience = Application.GetConfigValue<string>("audience");
            var keys = GetPublicKey(discoClient);

            var parameters = new TokenValidationParameters
            {
                ValidIssuer = discoClient.Issuer,
                ValidAudience = audience,
                IssuerSigningKeys = keys,

                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            try
            {
                var user = handler.ValidateToken(authHeader.Parameter, parameters, out var _);
                return user;
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                // This exception is thrown if the signature key of the JWT could not be found.
                // This could be the case when the issuer changed its signing keys.
                return null;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }

        private static List<SecurityKey> GetPublicKey(DiscoveryResponse discoClient)
        {    
            var keys = new List<SecurityKey>();
            foreach (var webKey in discoClient.KeySet.Keys)
            {
                var e = Base64Url.Decode(webKey.E);
                var n = Base64Url.Decode(webKey.N);

                var key = new RsaSecurityKey(new RSAParameters {Exponent = e, Modulus = n})
                {
                    KeyId = webKey.Kid
                };

                keys.Add(key);
            }

            return keys;
        }

        /// <summary>
        /// Authorize the current request for bearer tokens
        /// </summary>
        /// <param name="req">HttpRequestMessage request message</param>
        /// <param name="roles">Comma separated list of roles.</param>
        /// <returns>true or false if authorized</returns>
        public static async Task<bool> Authorize(this HttpRequestMessage req, string roles = null, ILogger log = null)
        {
            var principal = await req.Headers.ValidateTokenAsync();
            // If we don't get a claims principal back we do not have a valid token.
            if (principal == null)
            {
                log.LogInformation("No principal found");
                return false;
            }
            
            log.LogInformation("Token is valid");
            // If we don't have any roles to check and we have a valid token then okay.
            if (string.IsNullOrWhiteSpace(roles))
            {
                log.LogInformation("No roles specified");
                return true;
            }
               
            log.LogInformation("Validating roles");
            var rArr = roles.Split(',');
            var inRole = rArr.Any(r => principal.IsInRole(r.Trim()));
            log.LogInformation(inRole ? "One or more role found" : "No roles found. Not authorized");
            return inRole;
        }
        
        private static ClaimsPrincipal ValidateToken(this HttpRequestHeaders headers)
        {
            if (ValidateHeaders(headers, out var authHeader)) 
                return null;

            var issuer = Application.GetConfigValue<string>("issuer");
            var discoClient = DiscoveryClient.GetAsync(issuer).Result;
            
            return ValidateToken(discoClient, authHeader);
        }
    }
}

