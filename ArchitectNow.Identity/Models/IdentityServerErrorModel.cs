using IdentityServer4.Models;

namespace ArchitectNow.Identity.Models
{
    public class IdentityServerErrorModel
    {
        public ErrorMessage IdentityServerError { get; set; }
        public bool IsDevelopment { get; set; }
    }
}
