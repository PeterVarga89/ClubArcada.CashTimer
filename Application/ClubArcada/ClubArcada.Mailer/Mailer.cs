using System.IO;
using System.Net.Mail;

namespace ClubArcada.Mailer
{
    public class Mailer
    {
        public static void SendMail(string subject, string bodyMessage)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            SetMessageDetails(ref message);

            message.Subject = subject;
            message.Body = bodyMessage + Constants.MailSignature;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Constants.MailSmtpClient);
            smtp.Port = Constants.MailPort;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.MailUserName, Constants.MailPassword);
            smtp.Send(message);
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
            smtp.Send(message);
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
            smtp.Send(message);
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

            message.To.Add(Constants.MailToCC);

            message.Subject = subject;
            message.Body = bodyMessage + Constants.MailSignature;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Constants.MailSmtpClient);
            smtp.Port = Constants.MailPort;
            smtp.Credentials = new System.Net.NetworkCredential(Constants.MailUserName, Constants.MailPassword);
            smtp.Send(message);
        }
    }
}