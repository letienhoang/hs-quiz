using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Domain.SeedWork;
using Examination.Infrastructure.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositoty
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, IOptions<ExamSettings> examSettings, IMediator mediator) 
        : base(mongoClient, clientSessionHandle, Constants.Collections.User, examSettings, mediator)
        {
        }

        public Task<User> GetUserByIdAsync(string externalId) {
            var filter = Builders<User>.Filter.Eq(x => x.ExternalId, externalId);
            return Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}