using Backup;
using Cardbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CardboxTests
{
    [TestClass]
    public class CardboxDefinitionServiceTests
    {
        private readonly CardboxDefinitionService _cardboxDefinitionService = new CardboxDefinitionService(new CardboxDefinitionRepository());

        [TestMethod]
        public void AddCardboxDefinition()
        {
            _cardboxDefinitionService.Add(new CardboxDefinition
            {
                Duration = new TimeSpan(20, 0, 0),
                Number = 1
            });
        }
    }
}
