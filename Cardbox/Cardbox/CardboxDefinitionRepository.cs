using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Cardbox
{
    public class CardboxDefinitionRepository
    {
        static CardboxDefinitionRepository()
        {
            string filePath = ConfigurationManager.AppSettings["cardboxDefinition"];

            EnsureDirectoryExists(filePath);

        }

        public static readonly Lazy<CardboxDefinitionRepository> CardboxDefinitionDb = new Lazy<CardboxDefinitionRepository>(() =>
        {
            string filePath = ConfigurationManager.AppSettings["cardboxDefinition"];

            using (var reader = new StreamReader(filePath))
            {
                string cardboxData = reader.ReadToEnd();
                var str = new StringBuilder(cardboxData);
                return new CardboxDefinitionRepository(str);
            };
        });

        private static void EnsureDirectoryExists(string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        public StringBuilder Unsaved { get; }

        public static CardboxDefinitionRepository Instance => CardboxDefinitionDb.Value;

        public CardboxDefinitionRepository(StringBuilder unsaved)
        {
            Unsaved = unsaved;
        }

        public ResultDto Add(CardboxDefinitionDto dto)
        {
            string newRow = $"{dto.Number},{dto.Duration}";
            Unsaved.AppendLine(newRow);

            return new ResultDto();
        }

        public ResultDto Commit()
        {
            string path = ConfigurationManager.AppSettings["cardboxDefinition"];
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.Write(Unsaved);
            }

            return new ResultDto();
        }
    }
}
