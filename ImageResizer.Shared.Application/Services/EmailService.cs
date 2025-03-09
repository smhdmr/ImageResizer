using System.Net;
using System.Net.Mail;
using ImageResizer.Shared.Application.Services.Contracts;
using log4net;

namespace ImageResizer.Shared.Application.Services;

public class EmailService: IEmailService
{
    private readonly ILog logger = null!;
    private readonly SmtpClient smtpClient;
    private readonly string senderEmail;
    
    public EmailService(ILog logger, string host, int port, string username, string password)
    {
        smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };
        senderEmail = username;
    }
    
    public async Task SendEmail(string receiverEmail, string subject, string body)
    {
        try
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(senderEmail),
                To = { receiverEmail },
                Subject = subject,
                Body = body
            };
            await smtpClient.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}