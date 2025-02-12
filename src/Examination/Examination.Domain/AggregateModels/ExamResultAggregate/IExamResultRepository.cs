using Examination.Domain.SeedWork;

namespace Examination.Domain.AggregateModels.ExamResultAggregate
{
    public interface IExamResultRepository : IRepositoryBase<ExamResult>
    {
        Task<ExamResult> GetDetailsAsync(string userId, string examId);
    }
}