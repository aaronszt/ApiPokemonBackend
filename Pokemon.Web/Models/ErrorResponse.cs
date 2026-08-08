namespace Pokemon.Web.Models;

public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
    public IEnumerable<string>? Errors { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
}

