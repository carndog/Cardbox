﻿using Backup;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;

namespace CardboxTests
{
    [TestClass]
    public class CardboxDefintionRepositoryTests
    {
        [TestMethod]
        public void GivenCardbox_WhenCommit_ThenDataIsCorrect()
        {
            CardboxDefinition cardboxDefinition = new CardboxDefinition
            {
                Number = 1,
                Duration = new TimeSpan(1, 1, 1)
            };

            CardboxDefinitionRepository.Instance.Add(cardboxDefinition);

            string cardboxDefinitionPath = ConfigurationManager.AppSettings["cardboxDefinition"];

            File.Exists(cardboxDefinitionPath).Should().BeTrue();
            File.ReadAllText(cardboxDefinitionPath).Trim().Should().Be($"1,{new TimeSpan(1, 1, 1)}");
        }
    }
}
