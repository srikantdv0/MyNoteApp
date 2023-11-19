using System;
namespace Notes.Services
{
	public interface ISendEmail
	{
		Task<bool> SendEmailAsync(string ReciverName, string ReviverEmail,
                                  string Subject, string Body);
	}
}

