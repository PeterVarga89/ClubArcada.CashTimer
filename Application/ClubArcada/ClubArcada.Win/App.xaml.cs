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

            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var mainwindow = new MainWindow();
            var dlgLogin = new Dialogs.LoginDlg();
            dlgLogin.ShowDialog();
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

        public static MainWindow ParentWindow
        {
            get;
            set;
        }

        public static BusinessObjects.DataClasses.User User { get; set; }
    }
}