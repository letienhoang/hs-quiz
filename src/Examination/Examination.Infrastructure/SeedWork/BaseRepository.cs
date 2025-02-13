using System.Linq.Expressions;
using Examination.Domain.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.SeedWork
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : Entity, IAggregateRoot
    {
        private readonly IMongoClient _mongoClient;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly string _collectionName;
        private readonly ExamSettings _examSettings;
        private readonly IMediator _mediator;
        public BaseRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, string collectionName, IOptions<ExamSettings> examSettings, IMediator mediator)
        {
            _mongoClient = mongoClient;
            _clientSessionHandle = clientSessionHandle;
            _collectionName = collectionName;
            _examSettings = examSettings.Value;
            _mediator = mediator;
            if (!_mongoClient.GetDatabase(_examSettings.DatabaseSettings.DatabaseName).ListCollectionNames().ToList().Contains(collectionName))
            {
                _mongoClient.GetDatabase(_examSettings.DatabaseSettings.DatabaseName).CreateCollection(collectionName);
            }
        }

        protected virtual IMongoCollection<T> Collection =>
            _mongoClient.GetDatabase(_examSettings.DatabaseSettings.DatabaseName).GetCollection<T>(_collectionName);

        public async Task AbortTransactionAsync(CancellationToken cancellationToken = default) {
            await _clientSessionHandle.AbortTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(T entity, CancellationToken cancellationToken = default) {
            await _clientSessionHandle.CommitTransactionAsync(cancellationToken);
            var domainEvents = entity.DomainEvents!.ToList();
            entity.ClearDomainEvents();
            foreach (var domainEvent in domainEvents) {
                await _mediator.Publish(domainEvent);
            }
        }

        public async Task DeleteAsync(string id) {
            await Collection.DeleteOneAsync(_clientSessionHandle, x => x.Id == id);
        }

        public async Task InsertAsync(T obj) {
            await Collection.InsertOneAsync(_clientSessionHandle, obj);
        }

        public void StartTransaction() {
            _clientSessionHandle.StartTransaction();
        }

        public Task UpdateAsync(T obj) {
            Expression<Func<T, string>> func = x => x.Id;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var value = (string)obj.GetType().GetProperty(func.Body.ToString().Split(".")[1])?.GetValue(obj, null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            var filter = Builders<T>.Filter.Eq(func!, value);
            return Collection.ReplaceOneAsync(_clientSessionHandle, filter, obj);
        }
    }
}