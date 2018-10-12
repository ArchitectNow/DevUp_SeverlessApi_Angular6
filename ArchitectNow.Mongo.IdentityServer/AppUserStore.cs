using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using ArchitectNow.Mongo.Identity.Repositories;

namespace ArchitectNow.Mongo.Identity
{
    public class AppUserStore<TUser> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserRoleStore<TUser>,
        IUserLoginStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserEmailStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserAuthenticationTokenStore<TUser>
        where TUser : AppUser
    {
        private readonly IMongoIdentityRepository _repository;
        private readonly IMongoCollection<TUser> _userCollection;
        
        
        public AppUserStore(IMongoIdentityRepository repository)
        {
            _repository = repository;
            _userCollection = _repository.GetDatabase()
                .GetCollection<TUser>(AppIndentityConstants.Mongo.IdentityUserCollectionName);
        }
        
        public void Dispose()
        {
            // Not needed.
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName.ToUpperInvariant();
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            await _userCollection.InsertOneAsync(user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            
            await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            await _userCollection.DeleteOneAsync(u => u.Id == user.Id, null, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var result = await _userCollection.FindAsync(u => u.Id == userId, null, cancellationToken);
            return result.SingleOrDefault();
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var result = await _userCollection.FindAsync(
                u => u.NormalizedUserName == normalizedUserName.ToUpperInvariant(), null, cancellationToken);
            return result.SingleOrDefault();
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.HasPassword());
        }

        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Add(roleName.ToUpperInvariant());
            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Remove(roleName.ToUpperInvariant());
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<string>)user.Roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var hasRole = user.Roles.Contains(roleName.ToUpperInvariant());
            return Task.FromResult(hasRole);
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var result = await _userCollection.Find(u => u.Roles.Contains(roleName.ToUpperInvariant()))
                .ToListAsync(cancellationToken);
            return result;
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.Logins.Add(new AppUserLogin(login));
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.Logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<UserLoginInfo>)user.Logins.Select(l => l.ToUserLoginInfo()));
        }

        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return _userCollection
                .Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return _userCollection.Find(u => u.NormalizedEmail == normalizedEmail.ToUpperInvariant())
                .SingleOrDefaultAsync(cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail.ToUpperInvariant();
            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            var results = user.Claims.Select(c => c.ToSecurityClaim()).ToList();
            return Task.FromResult((IList<Claim>)results);
        }

        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.Claims.AddRange(claims.Select(c => new AppUserClaim(c)));
            return Task.FromResult(0);
        }

        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            
            var claimExists = user.Claims.Any(c => c.Type == claim.Type && c.Value == newClaim.Value);
            if (!claimExists)
                return Task.FromResult(0);

            user.Claims.RemoveAll(c => c.Type == claim.Type && c.Value == claim.Value);
            user.Claims.Add(new AppUserClaim(newClaim));
            
            return Task.FromResult(0);
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
                user.Claims.RemoveAll(c => c.Type == claim.Type && c.Value == claim.Value);
            return Task.FromResult(0);
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var results = await _userCollection
                .Find(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                .ToListAsync(cancellationToken);
            return results;
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnd);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = user.AccessFailedCount + 1;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            user.SetToken(loginProvider, name, value);
            return Task.FromResult(0);
        }

        public Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            user.RemoveToken(loginProvider, name);
            return Task.FromResult(0);
        }

        public Task<string> GetTokenAsync(TUser user, string provider, string name, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.GetTokenValue(provider, name));
        }
    }
}