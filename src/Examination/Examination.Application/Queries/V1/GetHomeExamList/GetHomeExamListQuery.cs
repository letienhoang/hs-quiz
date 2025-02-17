using Examination.Dtos;
using MediatR;

namespace Examination.Application.Queries.V1.GetHomeExamList
{
    public class GetHomeExamListQuery : IRequest<IEnumerable<ExamDto>>
    {
        
    }
}