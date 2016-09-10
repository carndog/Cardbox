using Cardbox;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace CardboxTests
{
    [TestClass]
    public class CardboxDefintionRepositoryTests
    {
        private CardboxDefinitionRepository _cardboxDefinitionRepository;

        [TestInitialize]
        public void Init()
        {
            _cardboxDefinitionRepository = new CardboxDefinitionRepository(new StringBuilder());
        }

        [TestMethod]
        public void AddCardbox()
        {
            _cardboxDefinitionRepository.Add(new CardboxDefinitionDto
            {
                Number = 1,
                Duration = new TimeSpan(1, 1, 1)
            });

            _cardboxDefinitionRepository.Unsaved.Should().Equals($"{1},{new TimeSpan(1, 1, 1)}");
        }

        [TestMethod]
        public void CommitCardbox()
        {
            _cardboxDefinitionRepository.Add(new CardboxDefinitionDto
            {
                Number = 1,
                Duration = new TimeSpan(1, 1, 1)
            });

            _cardboxDefinitionRepository.Commit();

            CardboxDefinitionRepository.Instance.Should().Equals($"1,{new TimeSpan(1, 1, 1)}");
        }
    }
}
