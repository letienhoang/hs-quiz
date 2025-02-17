using Examination.Domain.AggregateModels.ExamResultAggregate;
using MediatR;

namespace Examination.Application.Commands.V1.StartExam
{
    public class StartExamCommandHandler : IRequestHandler<StartExamCommand, bool>
    {
        private readonly IExamResultRepository _examResultRepository;
        public StartExamCommandHandler(IExamResultRepository examResultRepository)
        {
            _examResultRepository = examResultRepository;
        }
        public async Task<bool> Handle(StartExamCommand request, CancellationToken cancellationToken)
        {
            var examResult = await _examResultRepository.GetDetailsAsync(request.UserId, request.ExamId);
            _examResultRepository.StartTransaction();
            try 
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                if (examResult == null)
                {
                    examResult = ExamResult.CreateNewResult(request.UserId, request.ExamId);
                    examResult.StartExam(request.FirstName, request.LastName);
                    await _examResultRepository.InsertAsync(examResult);
                }
                else
                {
                    examResult.ExamStartDate = DateTime.Now;
                    examResult.Finished = false;
                    examResult.StartExam(request.FirstName, request.LastName);
                }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                await _examResultRepository.CommitTransactionAsync(examResult, cancellationToken);
                return true;
            }
            catch
            {
                await _examResultRepository.AbortTransactionAsync(cancellationToken);
                return false;
            }
        }
    }
}