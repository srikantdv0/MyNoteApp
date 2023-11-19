using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace Notes.Services
{
	public class SendEmail : ISendEmail
	{
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public SendEmail(IConfiguration configuration, ILogger<SendEmail> logger)
		{
            _configuration = configuration;
            _logger = logger;
		}

        public async Task<bool> SendEmailAsync(string ReciverName, string ReviverEmail,
                                  string Subject, string Body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["Email:SenderName"], _configuration["Email:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress(ReciverName, ReviverEmail));
            emailMessage.Subject = Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = Body};

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_configuration["Email:SMTP"], Int32.Parse(_configuration["Email:SMTPPort"]), true);
                    client.AuthenticationMechanisms.Remove(_configuration["Email:AuthenticationMechanism"]);
                    client.Authenticate(_configuration["Email:AuthenticateEmail"], _configuration["Email:AuthenticatePassword"]);
                    await client.SendAsync(emailMessage);
                    return true;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}

