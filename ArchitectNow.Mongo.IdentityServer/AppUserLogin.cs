using Microsoft.AspNetCore.Identity;

namespace ArchitectNow.Mongo.Identity
{
    public class AppUserLogin 
    {
        public AppUserLogin(string loginProvider, string providerKey, string providerDisplayName)
        {
            LoginProvider = loginProvider;
            ProviderDisplayName = providerDisplayName;
            ProviderKey = providerKey;
        }

        public AppUserLogin(UserLoginInfo login)
        {
            LoginProvider = login.LoginProvider;
            ProviderDisplayName = login.ProviderDisplayName;
            ProviderKey = login.ProviderKey;
        }
        
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual string ProviderDisplayName { get; set; }
        
        public UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }
    }
}