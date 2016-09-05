namespace Cardbox
{
    public class CardboxService
    {
        private readonly IAnagrammer _anagrammer;
        private readonly ICardboxRepository _cardboxRepository;

        public CardboxService(IAnagrammer anagrammer,
             ICardboxRepository cardboxRepository)
        {
            _anagrammer = anagrammer;
            _cardboxRepository = cardboxRepository;
        }

        public AnswerDto Evaluate(QuestionDto dto)
        {
            AnswerDto answer = _anagrammer.Anagram(dto);

            return answer;
        }

        public CardboxDto Add(CardboxDto dto)
        {
            _cardboxRepository.Add(dto);

            return dto;
        }
    }
}
