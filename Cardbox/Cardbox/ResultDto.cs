namespace Cardbox
{
    public class ResultDto
    {
        public ResultDto(bool success = true)
        {
            Success = success;
        }

        public bool Success { get; set; }
    }
}
