using System;
using System.Configuration;
using System.IO;
using System.Text;
using Backup;

namespace Cardbox
{
    public sealed class CardboxRepository
    {
        public static readonly Lazy<CardboxRepository> CardboxDb = new Lazy<CardboxRepository>(() =>
        {
            string filePath = ConfigurationManager.AppSettings["cardbox"];

            using (var reader = new StreamReader(filePath))
            {
                string cardboxData = reader.ReadToEnd();
                var str = new StringBuilder(cardboxData);
                return new CardboxRepository(str);
            };
        });

        public StringBuilder Unsaved { get; }

        public static CardboxRepository Instance => CardboxDb.Value;

        private CardboxRepository(StringBuilder unsaved)
        {
            Unsaved = unsaved;
        }

        public ResultDto Add(CardboxDto dto)
        {
            string newRow = $"{dto.DateAdded},{dto.CardboxNumber},{dto.QuestionType},{dto.Question}";
            Unsaved.AppendLine(newRow);

            return new ResultDto();
        }

        public ResultDto Commit()
        {
            throw new NotImplementedException();
        }
    }
}
