using Backup;

namespace Cardbox
{
    public class CardboxService
    {
        private readonly WordService _anagrammer;
        private readonly CardboxRepository _cardboxRepository;

        public CardboxService(WordService anagrammer,
             CardboxRepository cardboxRepository)
        {
            _anagrammer = anagrammer;
            _cardboxRepository = cardboxRepository;
        }

        public AnswerDto Evaluate(string query)
        {
            AnswerDto answer = _anagrammer.Anagram(query);

            return answer;
        }

        public ResultDto Add(Backup.Cardbox dto)
        {
            ResultDto resultDto = _cardboxRepository.Add(dto);

            return resultDto;
        }
    }
}
