using ClubArcada.BusinessObjects;
using System;
using System.ComponentModel;
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
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                StringBuilder bodyMessage = new StringBuilder();

                bodyMessage.Append("Expection: " + e.Exception.InnerException.Message).AppendLine().AppendLine();
                bodyMessage.Append("Message: " + e.Exception.Message).AppendLine().AppendLine();
                bodyMessage.Append("StackTrace: " + e.Exception.StackTrace).AppendLine();

                Mailer.Mailer.SendErrorMail("CashTimer Error", bodyMessage.ToString());
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Mailer.Mailer.SendErrorMail("CashTimer Error", ex.StackTrace);
            }
        }

        public static MainWindow ParentWindow { get; set; }

        public static BusinessObjects.DataClasses.User User { get; set; }

        public static DateTime LastUpdateDate { get; set; }

        public static Dialogs.KeyboardDlg KeyBoard { get; set; }

        public static void UpdateOnline(MainWindow main)
        {
            var worker = new BackgroundWorker();

            worker.DoWork += delegate
            {
                try
                {
                    BusinessObjects.Data.TournamentData.Insert(eConnectionString.Online, main.Tournament);
                    foreach (var t in main.Tables)
                        BusinessObjects.Data.TableData.Insert(eConnectionString.Online, t);

                    foreach (var cr in main.CashResults)
                    {
                        BusinessObjects.Data.CashResultData.InsertOrUpdate(eConnectionString.Online, cr);
                        foreach (var ci in cr.CashIns)
                            BusinessObjects.Data.CashResultData.InsertOrUpdate(eConnectionString.Online, ci);
                    }
                }
                catch (Exception e)
                {
                    if (e != null && e.Message != null)
                        Mailer.Mailer.SendErrorMail("Error", e.Message);
                }
            };

            worker.RunWorkerCompleted += delegate { App.LastUpdateDate = DateTime.Now; };
            worker.RunWorkerAsync();
        }
    }
}