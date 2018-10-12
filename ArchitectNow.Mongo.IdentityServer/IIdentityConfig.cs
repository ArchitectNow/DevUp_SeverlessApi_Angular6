using System.Collections.Generic;
using IdentityServer4.Models;

namespace ArchitectNow.Mongo.Identity
{
    public interface IIdentityConfig<TUserType, TRoleType>
        where TUserType : AppUser 
        where TRoleType : AppRole
    {
        List<Client> GetClients();
        List<ApiResource> GetApiResources();
        List<IdentityResource> GetIdentityResources();
        List<TUserType> GetInitialUsers();
        List<TRoleType> GetRoles();
    }
}