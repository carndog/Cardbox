using NodaTime;
using System;

namespace Leitner
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public QuestionType QuestionType { get; set; }

        public Cardbox Cardbox { get; set; }

        public LocalDate DateAdded
        {
            get
            {
                return LocalDateTime.FromDateTime(DateAddedValue).Date;
            }
            set
            {
                DateAddedValue = new DateTime(value.Year, value.Month, value.Day);
            }
        }

        public DateTime DateAddedValue { get; set; }
    }
}
