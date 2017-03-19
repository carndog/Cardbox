namespace Cardbox
{
    public sealed class CardboxRepository
    {
        public ResultDto Add(Cardbox dto)
        {
            using (var context = new CardboxContext())
            {
                context.Definitions.Add(dto);
                context.SaveChanges();
            }

            return new ResultDto();
        }
    }
}
