using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClubArcada.Win.Dialogs
{
    public partial class BonusDialog : DialogBase
    {
        public Transaction Transaction { get; set; }
        public User User { get; set; }
        public MainWindow MainWindow { get; set; }

        public BonusDialog(User user, MainWindow parent)
        {
            DataContext = this;
            InitializeComponent();
            MainWindow = parent;
            this.Title = "Bonus - " + user.FullDislpayName;

            User = user;

            Transaction = new BusinessObjects.DataClasses.Transaction()
            {
                CratedByUserId = App.User.UserId,
                Description = "Cash Game Bonus",
                TransactionType = (int)eTransactionType.Bonus,
                UserId = user.UserId
            };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Action save = () =>
            {
                var worker = new BackgroundWorker();
                worker.DoWork += delegate
                {
                    BusinessObjects.Data.TransactionData.Create(BusinessObjects.eConnectionString.Online, Transaction);
                    var balance = BusinessObjects.Data.UserData.GetUserBalance(User.UserId);
                    var mailBody = string.Format(Mailer.Constants.MailNewBonusBody, User.NickName, User.FirstName, User.LastName, "Cash Game", Transaction.Amount, balance);
                    ClubArcada.Mailer.Mailer.SendMail(Mailer.Constants.MailNewBonusSubject, mailBody);

                    if (Transaction.DateUsed.HasValue)
                    {
                        var cashresult = MainWindow.CashResults.SingleOrDefault(m => m.UserId == User.UserId);
                        cashresult.CashIns.Add(new CashIn()
                        {
                            Amount = (double)Transaction.Amount,
                            CashInId = Guid.NewGuid(),
                            CashResultId = cashresult.CashResultId,
                            DateCreated = DateTime.Now,
                            DateDeleted = null,
                            CreatedByUserId = App.User.UserId
                        });
                    }
                };

                worker.RunWorkerCompleted += delegate
                {
                    this.Close();
                };

                worker.RunWorkerAsync();
            };

            if ((cbxIsUsing.SelectedItem as ComboBoxItem).Tag.ToString() == "1")
            {
                Transaction.DateUsed = DateTime.Now;
                save();
            }
            else
            {
                save();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnPoker_Click(object sender, RoutedEventArgs e)
        {
            txtWin.Text = "40€";
            Transaction.Amount = 40;
        }

        private void btnSF_Click(object sender, RoutedEventArgs e)
        {
            txtWin.Text = "100€";
            Transaction.Amount = 100;
        }

        private void btnRF_Click(object sender, RoutedEventArgs e)
        {
            txtWin.Text = "200€";
            Transaction.Amount = 200;
        }
    }
}