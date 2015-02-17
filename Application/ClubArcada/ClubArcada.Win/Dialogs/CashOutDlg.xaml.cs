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

        private double? OriginalCashout { get; set; }

        public CashOutDlg(CashResult cashResult)
        {
            InitializeComponent();

            CashResult = cashResult;
            OriginalCashout = CashResult.CashOut;
            CashResult.CashOut = null;

            InitializeComponent();
            DataContext = this;

            txtDate.Text = DateTime.Now.ToString(new CultureInfo("sk-SK"));

            Title = string.Format("Cash Out - {0}", cashResult.User.FullDislpayName);

            this.Loaded += delegate { txtCashOut.Focus(); };
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CashResult.EndTime = null;
            Sender.Start();
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CashResult.CashOut.HasValue)
            {
                var alert = new Dialogs.AlertDialog("Zadajte sumu cash out!");
                alert.ShowDialog();
                return;
            }

            try
            {
                var user = BusinessObjects.Data.UserData.GetById(BusinessObjects.eConnectionString.Online, CashResult.User.UserId);
                BusinessObjects.Data.UserData.Update(BusinessObjects.eConnectionString.Local, user);
            }
            catch (Exception exp)
            { }

            var confirmDialog = new Dialogs.ConfirmDialog(string.Format("Určite chcete cash out-núť hráča {0}, suma {1}€ ?", CashResult.User.FullName, CashResult.CashOut.Value));
            confirmDialog.ShowDialog();

            if (confirmDialog.DialogResult.HasValue && confirmDialog.DialogResult.Value)
            {
                var balance = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);

                double avaibleMoneyToCashIn = 0;
                double givedBack = 0;

                if (balance < 0 && CashResult.CashOut != 0 && CashResult.User.AutoReturnState != BusinessObjects.eAutoReturnState.NotSet)
                {
                    if (CashResult.User.AutoReturnState == BusinessObjects.eAutoReturnState.Neto)
                    {
                        if (CashResult.CashOut > CashResult.CashInTotal)
                        {
                            givedBack = CashResult.CashOut.Value - CashResult.CashInTotal;
                            avaibleMoneyToCashIn = CashResult.CashOut.Value - givedBack;
                            var confirmBalanceDialog = new Dialogs.AlertDialog(string.Format("POZOR! Hráč má balance {0}€. Cash out môže byť {1}€! Stiahnutá suma: {2}€! ", balance, avaibleMoneyToCashIn, givedBack));
                            confirmBalanceDialog.ShowDialog();
                            var transaction = new Transaction() { Amount = givedBack, CratedByUserId = App.User.UserId, DateUsed = null, TransactionType = (int)BusinessObjects.eTransactionType.Gived, UserId = CashResult.User.UserId, Description = "Stiahnuté z výhry - Cash Game", DateDeleted = null };
                            BusinessObjects.Data.TransactionData.Create(BusinessObjects.eConnectionString.Online, transaction);
                            var balance1 = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);
                            var mailBody = string.Format(Mailer.Constants.MailNewBorrowBody, CashResult.User.NickName, CashResult.User.FirstName, CashResult.User.LastName, "Cash Game", transaction.Amount, balance1, App.User.FullName);
                            ClubArcada.Mailer.Mailer.SendMail(ClubArcada.Mailer.Constants.MailNewReplySubject, mailBody);
                        }
                    }
                    else
                    {
                        if (CashResult.CashOut > balance)
                        {
                            givedBack = CashResult.CashOut.Value - balance;
                            avaibleMoneyToCashIn = CashResult.CashOut.Value - givedBack;
                            var confirmBalanceDialog = new Dialogs.AlertDialog(string.Format("POZOR! Hráč má balance {0}€. Cash out môže byť {1}€! Stiahnutá suma: {2}€! ", balance, avaibleMoneyToCashIn, givedBack));
                            confirmBalanceDialog.ShowDialog();
                            var transaction = new Transaction() { Amount = givedBack, CratedByUserId = App.User.UserId, DateUsed = null, TransactionType = (int)BusinessObjects.eTransactionType.Gived, UserId = CashResult.User.UserId, Description = "Stiahnuté z výhry - Cash Game", DateDeleted = null };
                            BusinessObjects.Data.TransactionData.Create(BusinessObjects.eConnectionString.Online, transaction);
                            var balance1 = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);
                            var mailBody = string.Format(Mailer.Constants.MailNewBorrowBody, CashResult.User.NickName, CashResult.User.FirstName, CashResult.User.LastName, "Cash Game", transaction.Amount, balance1, App.User.FullName);
                            ClubArcada.Mailer.Mailer.SendMail(ClubArcada.Mailer.Constants.MailNewReplySubject, mailBody);
                        }
                        else
                        {
                            givedBack = balance - CashResult.CashOut.Value;
                            avaibleMoneyToCashIn = 0;
                            var confirmBalanceDialog = new Dialogs.AlertDialog(string.Format("POZOR! Hráč má balance {0}€. Cash out môže byť {1}€! Stiahnutá suma: {2}€! ", balance, avaibleMoneyToCashIn, givedBack));
                            confirmBalanceDialog.ShowDialog();
                            var transaction = new Transaction() { Amount = givedBack, CratedByUserId = App.User.UserId, DateUsed = null, TransactionType = (int)BusinessObjects.eTransactionType.Gived, UserId = CashResult.User.UserId, Description = "Stiahnuté z výhry - Cash Game", DateDeleted = null };
                            BusinessObjects.Data.TransactionData.Create(BusinessObjects.eConnectionString.Online, transaction);
                            var balance1 = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);
                            var mailBody = string.Format(Mailer.Constants.MailNewBorrowBody, CashResult.User.NickName, CashResult.User.FirstName, CashResult.User.LastName, "Cash Game", transaction.Amount, balance1, App.User.FullName);
                            ClubArcada.Mailer.Mailer.SendMail(ClubArcada.Mailer.Constants.MailNewReplySubject, mailBody);
                        }
                    }
                }

                CashResult.EndTime = DateTime.Now;
                Sender.Stop();
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