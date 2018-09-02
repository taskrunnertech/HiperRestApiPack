using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Platform.API.Repository
{

    public class ModelBase
    {
        /// <summary></summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>Insert date</summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;

        /// <summary>Update date</summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        /// <summary></summary>
        public DateTime? UpdateDate { get; set; } = DateTime.Now;

        /// <summary>If false means document is deleted</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>Shows the activation of the record</summary>
        public bool IsActive { get; set; } = true;
    }

    public interface IMongoRepository<T> where T : ModelBase
    {
        IMongoCollection<T> Collection { get; }
        Task<bool> Delete(FilterDefinition<T> filter);
        Task<T> Get(FilterDefinition<T> filter);
        Task<T> Get(string id);
        Task<T> Get(Expression<Func<T, bool>> filter);
        IMongoQueryable<T> GetAll(Expression<Func<T, bool>> filter);
        Task<List<T>> GetList(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAll(FilterDefinition<T> filter = null, bool getAll = false);
        Task<bool> HardDelete(FilterDefinition<T> filter);
        Task<T> Insert(T newItem);
        Task InsertMany(IEnumerable<T> newItems);
        Task<bool> Update(T document);
    }
}
