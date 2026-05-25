namespace E_Commerce.API.Middleware
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "An unexpected error occurred.";
        public string? Details { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}
