
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EduAcess.Services
{
	public static class EmailService
	{
		private const string SmtpHost = "smtp.gmail.com";
		private const int SmtpPort = 587;

		// Use your Gmail address here.
		private const string SenderEmail = "kwazi9939@gmail.com";

		// Replace this with a NEW Gmail App Password.
		// Do not reuse the password exposed in the earlier chat.
		private const string SenderAppPassword = "yfjf psyl obqy cpxv";

		public static async Task SendAsync(
			string toEmail,
			string subject,
			string htmlBody)
		{
			if (string.IsNullOrWhiteSpace(toEmail))
				throw new ArgumentException(
					"Recipient email is required.", nameof(toEmail));

			if (string.IsNullOrWhiteSpace(subject))
				throw new ArgumentException(
					"Email subject is required.", nameof(subject));

			if (string.IsNullOrWhiteSpace(htmlBody))
				throw new ArgumentException(
					"Email body is required.", nameof(htmlBody));

			using (var message = new MailMessage())
			{
				message.From = new MailAddress(
					SenderEmail, "EduAccess");

				message.To.Add(new MailAddress(toEmail));
				message.Subject = subject;
				message.Body = htmlBody;
				message.IsBodyHtml = true;

				using (var smtp = new SmtpClient(SmtpHost, SmtpPort))
				{
					smtp.EnableSsl = true;
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = new NetworkCredential(
						SenderEmail, SenderAppPassword);
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.Timeout = 30000;

					try
					{
						await smtp.SendMailAsync(message);
					}
					catch (Exception ex)
					{
						Trace.TraceError(
							"EduAccess email failed. Recipient: {0}. Error: {1}",
							toEmail, ex);

						throw;
					}
				}
			}
		}
	}
}
