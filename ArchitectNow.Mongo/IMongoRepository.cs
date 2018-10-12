using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ArchitectNow.Mongo
{
    public interface IMongoRepository<TRepositoryType, TType>
    {
        TRepositoryType Where(Expression<Func<TType, bool>> expression);
        TRepositoryType Take(int count);
        TRepositoryType Skip(int count);
        TRepositoryType OrderBy<TKey>(Expression<Func<TType, TKey>> expression);
        TRepositoryType OrderByDescending<TKey>(Expression<Func<TType, TKey>> expression);
        TType Single();
        TType Single(Expression<Func<TType, bool>> expression);
        Task<TType> SingleAsync();
        Task<TType> SingleAsync(Expression<Func<TType, bool>> expression);
        TType SingleOrDefault();
        TType SingleOrDefault(Expression<Func<TType, bool>> expression);
        Task<TType> SingleOrDefaultAsync();
        Task<TType> SingleOrDefaultAsync(Expression<Func<TType, bool>> expression);
        TType First();
        TType First(Expression<Func<TType, bool>> expression);
        Task<TType> FirstAsync();
        Task<TType> FirstAsync(Expression<Func<TType, bool>> expression);
        TType FirstOrDefault();
        TType FirstOrDefault(Expression<Func<TType, bool>> expression);
        Task<TType> FirsteOrDefaultAsync();
        Task<TType> FirsteOrDefaultAsync(Expression<Func<TType, bool>> expression);
        List<TType> ToList();
        Task<List<TType>> ToListAsync();
        long Count();
        Task<long> CountAsync();
        DeleteResult Delete(Expression<Func<TType, bool>> expression);
        void Add(TType item);
        void Add(IEnumerable<TType> items);
        bool CollectionExists();
    }
}