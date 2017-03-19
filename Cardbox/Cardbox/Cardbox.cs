using NodaTime;

namespace Cardbox
{
    public class Cardbox
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public QuestionType QuestionType { get; set; }

        public CardboxDefinition CardboxDefinition { get; set; }

        public LocalDate DateAdded { get; set; }
    }
}
