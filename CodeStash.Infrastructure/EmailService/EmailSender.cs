using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CodeStash.Infrastructure.EmailService;
public class EmailSender(IHttpClientFactory httpClientFactory, ILogger<EmailSender> logger) : IEmailSender
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EmailService");
    private readonly ILogger<EmailSender> _logger = logger;

    public async Task SendEmailAsync(EmailRequest emailRequest)
    {
        _logger.LogInformation("Sending email to {To}", emailRequest.To);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = JsonContent.Create(emailRequest),
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to send email to {To}: {StatusCode}", emailRequest.To, response.StatusCode);

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error content: {ErrorContent}", errorContent);

            throw new InvalidOperationException(
                $"Failed to send email. Status code: {response.StatusCode}, Error: {errorContent}");
        }

        _logger.LogInformation("Email sent successfully to {To}", emailRequest.To);
    }
}
