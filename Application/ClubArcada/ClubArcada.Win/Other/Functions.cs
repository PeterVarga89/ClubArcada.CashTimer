using System;
using System.Net;
using System.Net.Mail;

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

            message.Sender = new MailAddress("internal@clubarcada.hu");
            message.From = new MailAddress("internal@clubarcada.hu");
            message.Priority = MailPriority.High;

            client.Send(message);
        }

        private bool CheckIsConnection(String URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}