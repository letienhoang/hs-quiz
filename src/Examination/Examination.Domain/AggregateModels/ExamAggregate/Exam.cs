using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.SeedWork;
using Examination.Dtos.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Examination.Domain.AggregateModels.ExamAggregate
{
    public class Exam : Entity, IAggregateRoot
    {
        public Exam(string name, string shortDesc, string content, int numberOfQuestions, TimeSpan duration,
            IEnumerable<Question> questions, Level level, string ownerUserId, int numberOfQuestionCorrectForPass,
            bool isTimeRestricted)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            if (questions == null && !questions.Any())
                throw new ArgumentNullException($"{nameof(questions)} can not be null.");
#pragma warning restore CS8604 // Possible null reference argument.

            if (questions.Count() != numberOfQuestions)
                throw new ArgumentNullException($"{nameof(numberOfQuestions)} is invalid.");

            if (numberOfQuestionCorrectForPass > numberOfQuestions)
                throw new ArgumentNullException($"{nameof(numberOfQuestionCorrectForPass)} is invalid.");


            (Name, ShortDesc, Content, NumberOfQuestions,
                    Duration, Questions, Level, DateCreated, OwnerUserId, NumberOfQuestionCorrectForPass,
                    IsTimeRestricted)
                = (name, shortDesc, content, numberOfQuestions, duration, questions, level, DateTime.UtcNow,
                    ownerUserId,
                    numberOfQuestionCorrectForPass, isTimeRestricted);
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Exam(string name, string shortDesc, string content, int numberOfQuestions, Level level, DateTime dateCreated, string ownerUserId, int numberOfQuestionCorrectForPass, bool isTimeRestricted)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            this.Name = name;
            this.ShortDesc = shortDesc;
            this.Content = content;
            this.NumberOfQuestions = numberOfQuestions;
            this.Level = level;
            this.DateCreated = dateCreated;
            this.OwnerUserId = ownerUserId;
            this.NumberOfQuestionCorrectForPass = numberOfQuestionCorrectForPass;
            this.IsTimeRestricted = isTimeRestricted;

        }
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("shortDesc")]
        public string ShortDesc { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("numberOfQuestions")]
        public int NumberOfQuestions { get; set; }

        [BsonElement("duration")]
        public TimeSpan? Duration { get; set; }

        [BsonElement("questions")]
        public IEnumerable<Question> Questions { get; set; }

        [BsonElement("level")]
        public Level Level { get; set; }

        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; }

        [BsonElement("ownerUserId")]
        public string OwnerUserId { get; set; }

        [BsonElement("numberOfQuestionCorrectForPass")]
        public int NumberOfQuestionCorrectForPass { get; set; }

        [BsonElement("isTimeRestricted")]
        public bool IsTimeRestricted { get; set; }

    }
}