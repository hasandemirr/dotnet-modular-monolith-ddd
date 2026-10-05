namespace ModularMonolith.Application.Abstractions.Messaging;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct = default);

}