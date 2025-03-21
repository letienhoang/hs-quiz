using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.SeedWork;
using Examination.Infrastructure.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositoty
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, IOptions<ExamSettings> examSettings, IMediator mediator) 
        : base(mongoClient, clientSessionHandle, Constants.Collections.Question, examSettings, mediator)
        {
        }
    }
}