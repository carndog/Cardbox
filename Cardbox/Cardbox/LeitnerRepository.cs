using Leitner.Persistence;

namespace Leitner
{
    public sealed class LeitnerRepository
    {
        public Result Add(Question dto)
        {
            using (var context = new LeitnerContext())
            {
                context.Questions.Add(dto);
                context.SaveChanges();
            }

            return new Result();
        }
    }
}
