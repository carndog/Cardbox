namespace Cardbox
{
    public class CardboxService
    {
        private readonly WordService _anagrammer;
        private readonly ICardboxRepository _cardboxRepository;

        public CardboxService(WordService anagrammer,
             ICardboxRepository cardboxRepository)
        {
            _anagrammer = anagrammer;
            _cardboxRepository = cardboxRepository;
        }

        public AnswerDto Evaluate(string query)
        {
            AnswerDto answer = _anagrammer.Anagram(query);

            return answer;
        }

        public ResultDto Add(CardboxDto dto)
        {
            ResultDto resultDto = _cardboxRepository.Add(dto);

            return resultDto;
        }
    }
}
