﻿using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Cardbox
{
    public sealed class CardboxData : ICardboxRepository
    {
        public static readonly Lazy<CardboxData> CardboxDb = new Lazy<CardboxData>(() =>
        {
            string filePath = ConfigurationManager.AppSettings["cardbox"];
            using (StreamReader reader = new StreamReader(filePath))
            {
                string cardboxData = reader.ReadToEnd();
                StringBuilder str = new StringBuilder(cardboxData);
                return new CardboxData(str);
            };
        });

        private readonly StringBuilder _unsaved;

        public static CardboxData Instance => CardboxDb.Value;

        private CardboxData(StringBuilder unsaved)
        {
            _unsaved = unsaved;
        }

        public ResultDto Add(CardboxDto dto)
        {
            string newRow = $"{dto.DateAdded},{dto.CardboxNumber},{dto.Question.QuestionType},{dto.Question.Question}";
            _unsaved.AppendLine(newRow);

            return new ResultDto();
        }
    }
}
