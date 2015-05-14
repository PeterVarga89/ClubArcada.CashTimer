using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace ClubArcada.Mailer
{
    public class Mailer
    {
        public static void SendMail(string subject, string bodyMessage, bool isError = false)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            SetMessageDetails(ref message);

            if(isError)
            {
                message.To.Clear();
                message.To.Add(Constants.MailToCC);
            }

            message.Subject = subject;
            message.Body = bodyMessage + Constants.MailSignature;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Constants.MailSmtpClient);
            smtp.Port = Constants.MailPort;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.MailUserName, Constants.MailPassword);
            Send(smtp, message);
        }

        public static void SendMail(string subject, MemoryStream attachment, string attachmentName, string bodyMessage)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            SetMessageDetails(ref message);

            message.Subject = subject;
            message.Body = bodyMessage + Constants.MailSignature;
            message.Attachments.Add(new Attachment(attachment, attachmentName));

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Constants.MailSmtpClient);
            smtp.Port = Constants.MailPort;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.MailUserName, Constants.MailPassword);
            Send(smtp, message);
        }

        public static void SendMail(string subject, Stream attachment, string attachmentName, string bodyMessage)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            SetMessageDetails(ref message);

            message.Subject = subject;
            message.Body = bodyMessage + Constants.MailSignature;
            message.Attachments.Add(new Attachment(attachment, attachmentName));

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Constants.MailSmtpClient);
            smtp.Port = Constants.MailPort;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.MailUserName, Constants.MailPassword);
            Send(smtp, message);
        }

        private static void SetMessageDetails(ref System.Net.Mail.MailMessage message)
        {
            message.To.Add(Constants.MailTo);
            message.CC.Add(Constants.MailToCC);

            message.Sender = new MailAddress(Constants.MailSender);
            message.From = new MailAddress(Constants.MailFrom);
        }

        public static void SendErrorMail(string subject, string bodyMessage)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            SetMessageDetails(ref message);

            message.To.Clear();
            message.To.Add(Constants.MailToCC);

            message.Subject = subject;
            message.Body = bodyMessage + Constants.MailSignature;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Constants.MailSmtpClient);
            smtp.Port = Constants.MailPort;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.MailUserName, Constants.MailPassword);
            Send(smtp, message);
        }

        private static void Send(SmtpClient client, MailMessage message)
        {
            try
            {
                client.SendCompleted += client_SendCompleted;
                client.Send(message);
            }
            catch (Exception e)
            {
            }
        }

        private static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
        }

        private bool CheckConnection(String URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }
}