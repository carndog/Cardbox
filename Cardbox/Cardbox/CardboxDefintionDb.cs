using System;

namespace Cardbox
{
    public class CardboxDefintionDb : ICardboxDefinitionRepository
    {
        private readonly string _filePath;

        public CardboxDefintionDb()
        {
            _filePath = String.Empty;
        }

        public void Add(CardboxDefinitionDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
