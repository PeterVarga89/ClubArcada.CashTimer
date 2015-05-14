using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using ClubArcada.BusinessObjects;

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

                bodyMessage.Append("Expection: " + e.Exception.Message).AppendLine().AppendLine();
                bodyMessage.Append("Message: " + e.Exception.TargetSite).AppendLine().AppendLine();
                bodyMessage.Append("StackTrace: " + e.Exception.StackTrace).AppendLine();

                Mailer.Mailer.SendMail("CashTimer Error", bodyMessage.ToString(),true);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Mailer.Mailer.SendMail("CashTimer Error", ex.Message,true);
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
                    BusinessObjects.Data.TournamentData.Insert(eConnectionString.Online, main.Tournament.Copy());
                    foreach (var t in main.Tables)
                        BusinessObjects.Data.TableData.Insert(eConnectionString.Online, t.Copy());

                    foreach (var cr in main.CashResults.Copy())
                    {
                        BusinessObjects.Data.CashResultData.InsertOrUpdate(eConnectionString.Online, cr.Copy());

                        if (cr.CashIns != null)
                        {
                            foreach (var ci in cr.CashIns)
                            {
                                BusinessObjects.Data.CashResultData.InsertOrUpdate(eConnectionString.Online, ci.Copy());
                            }
                        }
                    }

                    LastUpdateDate = DateTime.Now;
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