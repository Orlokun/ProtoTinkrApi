using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TinkrCommon.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        
        private readonly IMongoCollection<T> dbCollection;

        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;
        
        public MongoRepository(IMongoDatabase mongoDatabase, string collectionName)
        {
            dbCollection = mongoDatabase.GetCollection<T>(collectionName);
        }
        
        public async Task<IReadOnlyCollection<T>> GetAsyncAll()
        {
            return await dbCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<T> GetAsyncById(Guid id)
        {
            var filter = _filterBuilder.Eq(item => item.Id, id);
            return await dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAsyncAll(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsyncById(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(item => item.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

    }
}