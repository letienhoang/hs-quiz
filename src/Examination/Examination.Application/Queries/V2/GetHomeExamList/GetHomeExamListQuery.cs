using Examination.Dtos;
using MediatR;

namespace Examination.Application.Queries.V2.GetHomeExamList
{
    public class GetHomeExamListQuery : IRequest<IEnumerable<ExamDto>>
    {
        
    }
}