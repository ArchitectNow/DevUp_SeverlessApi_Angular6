using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ArchitectNow.Mongo
{
   
        public abstract class MongoRepository<TRepositoryType, TType> : IMongoRepository<TRepositoryType, TType>
        where TType : class, new()
        where TRepositoryType: MongoRepository<TRepositoryType, TType>
    {
        private readonly IMongoDbContext _dbContext;
        private IMongoQueryable<TType> _query;
        
        public abstract string CollectionName { get; }
        
        protected MongoRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
            Initialize();
        }

        protected void Initialize()
        {
            _query = _dbContext.GetCollection<TType>(CollectionName).AsQueryable();
        }

        public TRepositoryType Where(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            _query = _query.Where(expression);
            return GetSelf();
        }

        public TRepositoryType Take(int count)
        {
            _query = _query.Take(count);
            return GetSelf();
        }

        public TRepositoryType Skip(int count)
        {
            _query = _query.Skip(count);
            return GetSelf();
        }

        public TRepositoryType OrderBy<TKey>(Expression<Func<TType, TKey>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            _query = _query.OrderBy(expression);
            return GetSelf();
        }

        public TRepositoryType OrderByDescending<TKey>(Expression<Func<TType, TKey>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            _query = _query.OrderByDescending(expression);
            return GetSelf();
        }

        public TType Single()
        {
            return _query.Single();
        }

        public TType Single(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).Single();
        }
        
        public Task<TType> SingleAsync()
        {
            return _query.SingleAsync();
        }

        public Task<TType> SingleAsync(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).SingleAsync();
        }

        public TType SingleOrDefault()
        {
            return _query.SingleOrDefault();
        }
        
        public TType SingleOrDefault(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).SingleOrDefault();
        }

        public Task<TType> SingleOrDefaultAsync()
        {
            return _query.SingleOrDefaultAsync();
        }
        
        public Task<TType> SingleOrDefaultAsync(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).SingleOrDefaultAsync();
        }

        public TType First()
        {
            return _query.First();
        }
        
        public TType First(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).First();
        }

        public Task<TType> FirstAsync()
        {
            return _query.FirstAsync();
        }
        
        public Task<TType> FirstAsync(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).FirstAsync();
        }

        public TType FirstOrDefault()
        {
            return _query.FirstOrDefault();
        }
        
        public TType FirstOrDefault(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).FirstOrDefault();
        }

        public Task<TType> FirsteOrDefaultAsync()
        {
            return _query.FirstOrDefaultAsync();
        }
        
        public Task<TType> FirsteOrDefaultAsync(Expression<Func<TType, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return _query.Where(expression).FirstOrDefaultAsync();
        }

        public List<TType> ToList()
        {
            return _query.ToList();
        }

        public Task<List<TType>> ToListAsync()
        {
            return _query.ToListAsync();
        }

        public long Count()
        {
            var filter = _query.ToBsonDocument();
            return _dbContext.GetCollection<TType>(CollectionName).Count(filter);
        }

        public Task<long> CountAsync()
        {
            var filter = _query.ToBsonDocument();
            return _dbContext.GetCollection<TType>(CollectionName).CountAsync(filter);
        }

        public DeleteResult Delete(Expression<Func<TType, bool>> expression)
        {
            var result = _dbContext.GetCollection<TType>(CollectionName).DeleteMany(expression);
            return result;
        }

        public void Add(TType item)
        {
            _dbContext.GetCollection<TType>(CollectionName).InsertOne(item);
        }

        public void Add(IEnumerable<TType> items)
        {
            _dbContext.GetCollection<TType>(CollectionName).InsertMany(items);
        }

        public bool CollectionExists()
        {
            var collection = _dbContext.GetCollection<TType>(CollectionName);
            var filter = new BsonDocument();
            var totalCount = collection.Count(filter);
            return (totalCount > 0) ? true : false;
        }

        private TRepositoryType GetSelf()
        {
            return (TRepositoryType) this;
        }
    }
}