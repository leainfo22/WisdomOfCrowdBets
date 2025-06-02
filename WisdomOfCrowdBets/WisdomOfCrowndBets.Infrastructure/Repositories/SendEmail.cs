using NPOI.XWPF.UserModel;
using System.Net.Mail;
using System.Net;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.DTO.Team;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowndBets.Infrastructure.Repositories
{
    public class SendEmail: ISendEmail
    {
        public async Task SentBetEmailNotification(string message, Email emailData) 
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(emailData.fromEmail);
                    mailMessage.To.Add(emailData.toEmail);
                    mailMessage.Subject = emailData.emailSubject;
                    mailMessage.Body = String.Format(emailData.emailBody,message);
                    mailMessage.IsBodyHtml = true;

                    using (SmtpClient smtpClient = new SmtpClient(emailData.smtpServer, emailData.smtpPort))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new NetworkCredential(emailData.fromEmail, "tsuvodeoujydkeml");
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                        await smtpClient.SendMailAsync(mailMessage);
                        Console.WriteLine("Email sent successfully via Gmail!");
                    }
                }
            }
            catch (SmtpException ex) // Catch specific SmtpException for SMTP-related errors
            {
                Console.WriteLine($"SMTP Error sending email: {ex.StatusCode} - {ex.Message}");
                // You might want to log more details here
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email via Gmail: {ex.Message}");
            }
        }

    }
}
