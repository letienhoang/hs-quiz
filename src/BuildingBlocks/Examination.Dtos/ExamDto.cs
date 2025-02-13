using Examination.Dtos.Enums;

namespace Examination.Dtos
{
    public class ExamDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string ShortDesc { get; set; }
        public int NumberOfQuestions { get; set; }
        public TimeSpan? Duration { get; set; }
        public Level Level { get; set; }
        public DateTime DateCreated { get; set; }
    }
}