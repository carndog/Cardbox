using Backup;

namespace Cardbox
{
    public class CardboxDefinitionService
    {
        private readonly CardboxDefinitionRepository _cardboxDefinitionRepository;

        public CardboxDefinitionService(CardboxDefinitionRepository cardboxRepository)
        {
            _cardboxDefinitionRepository = cardboxRepository;
        }

        public ResultDto Add(CardboxDefinition dto)
        {
            ResultDto resultDto = _cardboxDefinitionRepository.Add(dto);

            return resultDto;
        }
    }
}
