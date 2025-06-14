namespace CodeStash.Infrastructure.EmailService;

public interface IEmailSender
{
    Task SendEmailAsync(EmailRequest emailRequest);
}