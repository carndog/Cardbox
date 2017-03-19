namespace Cardbox
{
    public class CardboxService
    {
        private readonly CardboxRepository _cardboxRepository;

        public CardboxService(CardboxRepository cardboxRepository)
        {
            _cardboxRepository = cardboxRepository;
        }

        public ResultDto Add(Cardbox dto)
        {
            ResultDto resultDto = _cardboxRepository.Add(dto);

            return resultDto;
        }
    }
}
