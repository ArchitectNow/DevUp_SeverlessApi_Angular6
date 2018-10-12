using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectNow.DataModels;

namespace ArchitectNow.Mongo.Repositories
{
    public interface IWodsRepository : IMongoRepository<WodsRepository, Wod>
    {
        
    }
}