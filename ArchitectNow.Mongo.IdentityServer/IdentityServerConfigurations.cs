namespace ArchitectNow.Mongo.Identity
{
    public class IdentityServerConfiguration
    {
        public string AuthorityUrl { get; set; }
        public BaseUrls BaseUrls { get; set; }
    }

    public class BaseUrls
    {
        public string Api { get; set; }
        public string Portal { get; set; }
    }
    
}