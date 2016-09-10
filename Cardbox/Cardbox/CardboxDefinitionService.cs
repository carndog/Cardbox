namespace Cardbox
{
    public class CardboxDefinitionService
    {
        private readonly CardboxDefinitionRepository _cardboxDefinitionRepository;

        public CardboxDefinitionService(CardboxDefinitionRepository cardboxRepository)
        {
            _cardboxDefinitionRepository = cardboxRepository;
        }

        public ResultDto Add(CardboxDefinitionDto dto)
        {
            ResultDto resultDto = _cardboxDefinitionRepository.Add(dto);

            return resultDto;
        }
    }
}
