using System;

namespace Cardbox
{
    public class CardboxDb : ICardboxRepository
    {
        private readonly string _filePath;

        public CardboxDb()
        {
            _filePath = String.Empty;
        }


        public void Add(CardboxDto dto)
        {

        }
    }
}
