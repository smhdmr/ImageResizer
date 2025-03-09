namespace ImageResizer.Shared.Application.Services.Contracts;

public interface IEmailService
{
    Task SendEmail(string receiverEmail, string subject, string body);
}