using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using ArchitectNow.DataModels;

namespace ArchitectNow.Mongo.Repositories
{
    public class WodsRepository : MongoRepository<WodsRepository, Wod>, IWodsRepository
    {
        public WodsRepository(IMongoDbContext dbContext) : base(dbContext)
        {
            
        }

        public override string CollectionName => "wods";
    }
}