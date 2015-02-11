using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Globalization;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class CashOutDlg : DialogBase
    {
        public Controls.PlayerLineCtrl Sender { get; set; }

        public CashResult CashResult { get; set; }

        public CashOutDlg(CashResult cashResult)
        {
            InitializeComponent();

            CashResult = cashResult;
            InitializeComponent();
            DataContext = this;

            txtDate.Text = DateTime.Now.ToString(new CultureInfo("sk-SK"));

            Title = string.Format("Cash Out - {0}", cashResult.User.FullDislpayName);

            txtCashOut.Focus();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CashResult.EndTime = null;
            Sender.Timer.Start();
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var confirmDialog = new Dialogs.ConfirmDialog(string.Format("Určite chcete cash out-núť hráča {0}, suma {1}€ ?", CashResult.User.FullName, CashResult.CashOut.Value));
            confirmDialog.ShowDialog();

            if (confirmDialog.DialogResult.HasValue && confirmDialog.DialogResult.Value)
            {
                var balance = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);

                double avaibleMoneyToCashIn = 0;
                double givedBack = 0;

                if (balance < 0)
                {
                    if (CashResult.CashOut > Math.Abs(balance))
                    {
                        avaibleMoneyToCashIn = CashResult.CashOut.Value - Math.Abs(balance);
                        givedBack = Math.Abs(balance);

                        var confirmBalanceDialog = new Dialogs.AlertDialog(string.Format(
                            "POZOR! Hráč má balance {0}€. Cash out môže byť {1}€! Stiahnutá suma: {2}€! ",
                            balance,
                            avaibleMoneyToCashIn,
                            givedBack));

                        confirmBalanceDialog.ShowDialog();

                        var transaction = new Transaction()
                        {
                            Amount = givedBack,
                            CratedByUserId = App.User.UserId,
                            DateUsed = null,
                            TransactionType = (int)BusinessObjects.eTransactionType.Gived,
                            UserId = CashResult.User.UserId,
                            Description = "Stiahnuté z výhry - Cash Game",
                            DateDeleted = null
                        };

                        BusinessObjects.Data.TransactionData.Create(BusinessObjects.eConnectionString.Online, transaction);

                        var balance1 = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);
                        var mailBody = string.Format(Mailer.Constants.MailNewBorrowBody, CashResult.User.NickName, CashResult.User.FirstName, CashResult.User.LastName, "Cash Game", transaction.Amount, balance1, App.User.FullName);
                        ClubArcada.Mailer.Mailer.SendMail(ClubArcada.Mailer.Constants.MailNewReplySubject, mailBody);
                    }
                    else
                    {
                        avaibleMoneyToCashIn = 0;
                        givedBack = CashResult.CashOut.Value;

                        var confirmBalanceDialog = new Dialogs.AlertDialog(string.Format(
                            "POZOR! Hráč má balance {0}€. Cash out môže byť {1}€! Stiahnutá suma: {2}€! ",
                            balance,
                            avaibleMoneyToCashIn,
                            givedBack));

                        confirmBalanceDialog.ShowDialog();

                        var transaction = new Transaction()
                        {
                            Amount = givedBack,
                            CratedByUserId = App.User.UserId,
                            DateUsed = null,
                            TransactionType = (int)BusinessObjects.eTransactionType.Gived,
                            UserId = CashResult.User.UserId,
                            Description = "Stiahnuté z výhry - Cash Game",
                            DateDeleted = null
                        };

                        BusinessObjects.Data.TransactionData.Create(BusinessObjects.eConnectionString.Online, transaction);

                        var balance1 = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);
                        var mailBody = string.Format(Mailer.Constants.MailNewBorrowBody, CashResult.User.NickName, CashResult.User.FirstName, CashResult.User.LastName, "Cash Game", transaction.Amount, balance1, App.User.FullName);
                        ClubArcada.Mailer.Mailer.SendMail(ClubArcada.Mailer.Constants.MailNewReplySubject, mailBody);
                    }
                }

                CashResult.EndTime = DateTime.Now;
                Sender.Timer.Stop();
                Sender.Refresh();
                App.ParentWindow.PlayingPlayerIds.Remove(CashResult.UserId);
                this.Close();
            }
        }

        private void txtCashOut_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCashOut.Text = string.Empty;
        }
    }
}