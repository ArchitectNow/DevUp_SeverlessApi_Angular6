using ArchitectNow.DataModels;
using ArchitectNow.Mongo.Repositories;

namespace ArchitectNow.ApiFunctions.Shared
{
    public class Repositories 
    {
        public Repositories()
        {
            
        }

        private IWodsRepository _wodsRepository;
        public IWodsRepository WodsRepository => _wodsRepository 
                                                 ?? (_wodsRepository = new WodsRepository(Application.DataContext));
    }
}