using notification_service.Notifications.DTOS;
using notification_service.Notifications.Services;
using System.Net.Mail;
using System.Net;

namespace notification_service.Notifications.EmailSerivce
{
    public class Emails : INotifications
    {
        public string TypeService => "Email";

        public async Task<bool> SendRegisterAccount(RequestSendMessage request)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("hotelluxurytrungduc@gmail.com", "ykbg blmo tqxy hrld");
                    smtp.EnableSsl = true;

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("hotelluxurytrungduc@gmail.com", "Hotel Management");
                        message.To.Add(request.To);
                        message.Subject = request.Subject;
                        message.Body = request.Body;
                        message.IsBodyHtml = true;

                        await smtp.SendMailAsync(message);
                    }
                }

                Console.WriteLine("Email sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}
