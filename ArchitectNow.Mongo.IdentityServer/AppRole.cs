using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace ArchitectNow.Mongo.Identity
{
    public class AppRole : IdentityRole<BsonObjectId>
    {
        public AppRole()
        {
            Id = ObjectId.GenerateNewId();
        }
    }
    
    
}