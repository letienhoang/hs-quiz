using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Dtos;
using MediatR;
using MongoDB.Driver;

namespace Examination.Application.Queries.V1.GetHomeExamList
{
    public class GetHomeExamListQueryHandler : IRequestHandler<GetHomeExamListQuery, IEnumerable<ExamDto>>
    {
        private readonly IExamRepository _examRepository;
        private readonly IClientSessionHandle _clientSessionHandler;
        private readonly IMapper _mapper;
        public GetHomeExamListQueryHandler(IExamRepository examRepository, IClientSessionHandle clientSessionHandler, IMapper mapper)
        {
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _clientSessionHandler = clientSessionHandler ?? throw new ArgumentNullException(nameof(clientSessionHandler));
            _mapper = mapper;
        }
        public async Task<IEnumerable<ExamDto>> Handle(GetHomeExamListQuery request, CancellationToken cancellationToken) {
            var exams = await _examRepository.GetExamListAsync();
            return _mapper.Map<IEnumerable<ExamDto>>(exams);
        }
    }
}