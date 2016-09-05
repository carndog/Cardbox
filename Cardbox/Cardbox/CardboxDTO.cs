using NodaTime;

namespace Cardbox
{
    public class CardboxDto
    {
        public QuestionDto Question { get; set; }

        public int CardboxNumber { get; set; }

        public LocalDate DateAdded { get; set; }
    }
}
