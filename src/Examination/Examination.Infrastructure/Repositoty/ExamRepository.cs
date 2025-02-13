using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Infrastructure.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Examination.Infrastructure.Repositoty
{
    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
        public ExamRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, string collectionName, IOptions<ExamSettings> examSettings, IMediator mediator) 
        : base(mongoClient, clientSessionHandle, collectionName, examSettings, mediator)
        {
        }

        public async Task<Exam> GetExamByIdAsync(string id) {
            var filter = Builders<Exam>.Filter.Eq(x => x.Id, id);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Exam>> GetExamListAsync() => await Collection.AsQueryable().ToListAsync();
    }
}