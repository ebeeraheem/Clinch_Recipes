namespace CodeStash.Infrastructure.EmailService;

public class EmailRequest
{
    public required string To { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public bool IsHtmlBody { get; set; }
}
