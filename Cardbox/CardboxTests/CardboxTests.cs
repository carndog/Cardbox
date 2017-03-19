using Cardbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using System;

namespace CardboxTests
{
    [TestClass]
    public class CardboxTests
    {
        private readonly CardboxService _cardboxService = new CardboxService(new CardboxRepository());

        [TestMethod]
        public void AddCardbox()
        {
            _cardboxService.Add(new Cardbox.Cardbox()
            {
                CardboxDefinition = new Cardbox.CardboxDefinition
                {
                    Duration = new TimeSpan(20, 0, 0),
                    Number = 2
                },
                DateAdded = SystemClock.Instance.Now.InUtc().LocalDateTime.Date,
                Question = "ISX",
                QuestionType = QuestionType.Anagram
            });
        }
    }
}
