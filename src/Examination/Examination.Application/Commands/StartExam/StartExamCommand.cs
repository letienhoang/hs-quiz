using MediatR;

namespace Examination.Application.Commands.StartExam
{
    public class StartExamCommand : IRequest<bool>
    {
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ExamId { get; set; }
        public DateTime StartDate { get; set; }
    }
}