using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace ClubArcada.Win
{
    public partial class App : Application
    {
        public App()
            : base()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            StringBuilder bodyMessage = new StringBuilder();

            bodyMessage.Append("Expection: " + e.Exception.InnerException.Message).AppendLine().AppendLine();
            bodyMessage.Append("Message: " + e.Exception.Message).AppendLine().AppendLine();
            bodyMessage.Append("StackTrace: " + e.Exception.StackTrace).AppendLine();

            Mailer.Mailer.SendErrorMail("CashTimer Error", bodyMessage.ToString());
            e.Handled = true;
        }
    }
}