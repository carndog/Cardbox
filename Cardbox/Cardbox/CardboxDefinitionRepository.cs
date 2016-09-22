using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Cardbox
{
    public class CardboxDefinitionRepository
    {
        private static readonly string FilePath;

        static CardboxDefinitionRepository()
        {
            FilePath = ConfigurationManager.AppSettings["cardboxDefinition"];

            string directoryName = Path.GetDirectoryName(FilePath);
            if (directoryName != null && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        private static readonly Lazy<CardboxDefinitionRepository> CardboxDefinitionDb = new Lazy<CardboxDefinitionRepository>(() =>
        {
            if (File.Exists(FilePath))
            {
                var str = new StringBuilder(File.ReadAllText(FilePath));
                return new CardboxDefinitionRepository(str);
            }
            else
            {
                return new CardboxDefinitionRepository(new StringBuilder(string.Empty));
            }
        });

        private StringBuilder Unsaved { get; }

        public static CardboxDefinitionRepository Instance => CardboxDefinitionDb.Value;

        private CardboxDefinitionRepository() { }

        private CardboxDefinitionRepository(StringBuilder unsaved)
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

            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(Unsaved);
            }

            return new ResultDto();
        }
    }
}
