using Backup;
using Cardbox;
using Cardbox.LexiconSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using System;

namespace CardboxTests
{
    [TestClass]
    public class CardboxTests
    {
        private readonly CardboxService _cardboxService = new CardboxService(
            new WordService
            (new TrieSearcher(
                new LazyLoadingTrie(
                    new AnagramTrieBuilder(
                        new ExecutingAssemblyFilePath().GetPath(), new TrieNode())))), new CardboxRepository());

        [TestMethod]
        public void AddCardbox()
        {
            _cardboxService.Add(new Backup.Cardbox()
            {
                CardboxDefinition = new CardboxDefinition
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
