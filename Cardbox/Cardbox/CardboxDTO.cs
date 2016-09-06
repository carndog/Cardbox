using NodaTime;

namespace Cardbox
{
    public class CardboxDto
    {
        public string Question { get; set; }

        public QuestionType QuestionType { get; set; }

        public int CardboxNumber { get; set; }

        public LocalDate DateAdded { get; set; }
    }
}
