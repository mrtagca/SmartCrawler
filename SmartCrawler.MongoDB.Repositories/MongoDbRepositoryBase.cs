using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartCrawler.MongoDB.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCrawler.MongoDB.Repositories
{
    public abstract class MongoDbRepositoryBase<T> : IRepository<T, string> where T : MongoDbEntity, new()
    {
        protected readonly IMongoCollection<T> Collection;
        private readonly MongoDbSettings settings;

        protected MongoDbRepositoryBase(IOptions<MongoDbSettings> options)
        {
            this.settings = options.Value;
            var client = new MongoClient(this.settings.ConnectionString);
            var db = client.GetDatabase(this.settings.Database);
            this.Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? Collection.AsQueryable()
                : Collection.AsQueryable().Where(predicate);
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual T Add(T entity)
        {
            try
            {
                var options = new InsertOneOptions { BypassDocumentValidation = false };
                Collection.InsertOne(entity, options);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await Collection.InsertOneAsync(entity, options);
            return entity;
        }

        public virtual bool AddRange(IEnumerable<T> entities)
        {
            try
            {
                var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
                BulkWriteResult<T> bulkWriteResult = Collection.BulkWrite((IEnumerable<WriteModel<T>>)entities, options);
                return (bulkWriteResult).IsAcknowledged;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
            return (await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
        }

        public virtual async Task<T> UpdateAsync(string id, T entity)
        {
            return await Collection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }

        public virtual async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            return await Collection.FindOneAndReplaceAsync(predicate, entity);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            return await Collection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
        }

        public virtual async Task<T> DeleteAsync(string id)
        {
            return await Collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return await Collection.FindOneAndDeleteAsync(filter);
        }
    }
}
