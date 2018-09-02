using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Platform.API.Repository
{
    public class MongoRepository<T> : MongoDBContextBase, IMongoRepository<T> where T : ModelBase
    {
        public IMongoCollection<T> Collection { get; private set; }

        public MongoRepository(IOptions<MongoDBConfiguration> settings)
            : base(settings)
        {
            Collection = Database.GetCollection<T>(typeof(T).Name);
        }

        public virtual async Task<T> Get(FilterDefinition<T> filter)
        {
            T document = await Collection
                .FindAsync<T>(filter)
                .Result
                .FirstOrDefaultAsync();

            return document;
        }



        public virtual async Task<T> Get(string id)
        {
            T document = await Collection
                .FindAsync<T>(Builders<T>.Filter.Eq<string>(x => x.Id, id))
                .Result
                .FirstOrDefaultAsync();

            return document;
        }

        public virtual async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await Collection.AsQueryable<T>().Where(filter).FirstOrDefaultAsync();
        }

        public virtual IMongoQueryable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return Collection.AsQueryable<T>().Where(filter);
        }

        public virtual Task<List<T>> GetList(Expression<Func<T, bool>> filter)
        {
            return Collection.AsQueryable<T>().Where(filter).ToListAsync();
        }

        public virtual async Task<List<T>> GetAll(FilterDefinition<T> filter = null, bool getAll = false)
        {
            if (filter == null)
                filter = FilterDefinition<T>.Empty;

            if (!getAll)
            {
                filter = filter & Builders<T>.Filter.Eq<bool>(x => x.IsDeleted, false);
                filter = filter & Builders<T>.Filter.Eq<bool>(x => x.IsActive, true);
            }
            List<T> documentList = await Collection
                .FindAsync<T>(filter)
                .Result
                .ToListAsync();

            if (documentList == null)
                return new List<T>();

            return documentList;
        }

        public virtual async Task<T> Insert(T newItem)
        {
            newItem.CreateDate = DateTime.Now;
            await Collection.InsertOneAsync(newItem);

            return newItem;
        }

        public virtual async Task InsertMany(IEnumerable<T> newItems)
        {
            await Collection.InsertManyAsync(newItems);
        }

        public virtual async Task<bool> Update(T document)
        {
            document.UpdateDate = DateTime.Now;
            ReplaceOneResult result = await Collection.ReplaceOneAsync(
                x => x.Id == document.Id,
                document
            );

            return result.ModifiedCount > 0;
        }

        public virtual async Task<bool> Delete(FilterDefinition<T> filter)
        {
            UpdateResult result = await Collection.UpdateOneAsync(
                filter,
                Builders<T>.Update.Set<bool>(x => x.IsDeleted, true)
            );

            return result.ModifiedCount > 0;
        }

        public virtual async Task<bool> HardDelete(FilterDefinition<T> filter)
        {
            var cancellationToken = new System.Threading.CancellationToken();
            DeleteResult result = await Collection.DeleteOneAsync(filter, cancellationToken);

            return result.DeletedCount > 0;
        }
    }
}
