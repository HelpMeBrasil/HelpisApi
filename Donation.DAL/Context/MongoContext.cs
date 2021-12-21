using Donation.Domain.Contract;
using Donation.Domain.Helpers;
using Donation.Infrastructure.Contract;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Donation.Infrastructure.Repository
{
    public class MongoContext<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> collection;

        public MongoContext(IMongoDatabase database)
        {
            collection = database.GetCollection<TDocument>(MongoHelper.GetCollectionName(typeof(TDocument)));
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public virtual TDocument FindById(string id)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, new ObjectId(id));

            return collection.Find(filter).SingleOrDefault();
        }

        public virtual async Task<TDocument> FindByIdAsync(string id)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, new ObjectId(id));

            IAsyncCursor<TDocument> result = await collection.FindAsync(filter);

            return result.SingleOrDefault();
        }

        public virtual void InsertOne(TDocument document)
        {
            collection.InsertOne(document);
        }

        public virtual async Task InsertOneAsync(TDocument document)
        {
            await collection.InsertOneAsync(document);
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            collection.InsertMany(documents);
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);

            await collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            collection.FindOneAndDelete(filterExpression);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await collection.FindOneAndDeleteAsync(filterExpression);
        }

        public void DeleteById(string id)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, new ObjectId(id));

            collection.FindOneAndDelete(filter);
        }

        public async Task DeleteByIdAsync(string id)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, new ObjectId(id));

            await collection.FindOneAndDeleteAsync(filter);
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            collection.DeleteMany(filterExpression);
        }

        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await collection.DeleteManyAsync(filterExpression);
        }
    }
}