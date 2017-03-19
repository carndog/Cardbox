namespace CardboxDefinition
{
    public class CardboxDefinitionRepository
    {
        public ResultDto Add(CardboxDefinition dto)
        {
            using (var context = new CardboxDefinitionContext())
            {
                context.Definitions.Add(dto);
                context.SaveChanges();
            }

            return new ResultDto();
        }
    }
}
