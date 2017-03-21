namespace Leitner
{
    public class Result
    {
        public Result(bool success = true)
        {
            Success = success;
        }

        public string Data { get; set; } = string.Empty;

        public bool Success { get; set; }
    }
}
