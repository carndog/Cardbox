using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using System;
using Leitner;

namespace CardboxTests
{
    [TestClass]
    public class LeitnerTests
    {
        private readonly LeitnerService _leitnerService = new LeitnerService(new LeitnerRepository());

        [TestMethod]
        public void AddCardBox()
        {
            _leitnerService.Add(new Question()
            {
                Cardbox = new Cardbox
                {
                    Duration = new TimeSpan(20, 0, 0),
                    Number = 2
                },
                DateAdded = SystemClock.Instance.Now.InUtc().LocalDateTime.Date,
                Text = "ISX",
                QuestionType = QuestionType.Anagram
            });
        }
    }
}
