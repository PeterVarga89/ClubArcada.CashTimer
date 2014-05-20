using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.Win.Other
{
    internal class Functions
    {
        public static void SendMail(string subject, string bodyMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("petervarga891@gmail.com", "vape6931"),
                EnableSsl = true
            };

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            message.Subject = "Cash Timer - " + subject;
            message.Body = bodyMessage;

            message.To.Add("petervarga891@gmail.com");

            message.Sender = new MailAddress("internal@renoval.hu");
            message.From = new MailAddress("internal@renoval.hu");
            message.Priority = MailPriority.High;

            client.Send(message);
        }
    }
}
