namespace Leitner
{
    public class LeitnerService
    {
        private readonly LeitnerRepository _leitnerRepository;

        public LeitnerService(LeitnerRepository leitnerRepository)
        {
            _leitnerRepository = leitnerRepository;
        }

        public Result Add(Question dto)
        {
            Result result = _leitnerRepository.Add(dto);

            return result;
        }
    }
}
