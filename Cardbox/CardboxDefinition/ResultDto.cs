namespace CardboxDefinition
{
    public class ResultDto
    {
        public ResultDto(bool success = true)
        {
            Success = success;
        }

        public string Data { get; set; } = string.Empty;

        public bool Success { get; set; }
    }
}
