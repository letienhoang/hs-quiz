using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Infrastructure.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositoty
{
    public class ExamResultRepository : BaseRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, IOptions<ExamSettings> examSettings, IMediator mediator) 
        : base(mongoClient, clientSessionHandle, Constants.Collections.ExamResult, examSettings, mediator)
        {
        }

        public async Task<ExamResult> GetDetailsAsync(string userId, string examId)
        {
            var filter = Builders<ExamResult>.Filter.Where(s => s.ExamId == examId && s.UserId == userId);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}