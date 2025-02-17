using AutoMapper;
using DnsClient.Internal;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Examination.Application.Queries.V1.GetHomeExamList
{
    public class GetHomeExamListQueryHandler : IRequestHandler<GetHomeExamListQuery, IEnumerable<ExamDto>>
    {
        private readonly IExamRepository _examRepository;
        private readonly IClientSessionHandle _clientSessionHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<GetHomeExamListQueryHandler> _logger;
        public GetHomeExamListQueryHandler(IExamRepository examRepository, IClientSessionHandle clientSessionHandler, IMapper mapper, ILogger<GetHomeExamListQueryHandler> logger)
        {
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _clientSessionHandler = clientSessionHandler ?? throw new ArgumentNullException(nameof(clientSessionHandler));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ExamDto>> Handle(GetHomeExamListQuery request, CancellationToken cancellationToken) {
            _logger.LogInformation("GetHomeExamListQueryHandler called");
            var exams = await _examRepository.GetExamListAsync();
            _logger.LogInformation("GetHomeExamListQueryHandler returned");
            return _mapper.Map<IEnumerable<ExamDto>>(exams);
        }
    }
}