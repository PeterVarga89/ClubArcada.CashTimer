using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using ClubArcada.BusinessObjects.DataClasses;

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
                CashResult.User = user;
            }
            catch (Exception exp)
            { }

            var confirmDialog = new Dialogs.ConfirmDialog(string.Format("Určite chcete cash out-núť hráča {0}, suma {1}€ ?", CashResult.User.FullName, CashResult.CashOut.Value));
            confirmDialog.ShowDialog();

            if (confirmDialog.DialogResult.HasValue && confirmDialog.DialogResult.Value)
            {
                var balance = BusinessObjects.Data.UserData.GetUserBalance(CashResult.User.UserId);

                double avaibleMoneyToCashOut = 0;
                double givedBack = 0;

                if (balance < 0 && CashResult.CashOut != 0 && CashResult.User.AutoReturnState != BusinessObjects.eAutoReturnState.NotSet)
                {
                    if (CashResult.User.AutoReturnState == BusinessObjects.eAutoReturnState.Neto)
                    {
                        if (CashResult.CashOut > CashResult.CashInTotal)
                        {
                            var b = Math.Abs(balance);
                            var nettoWin = CashResult.CashOut.Value - CashResult.CashInTotal;

                            if (nettoWin >= Math.Abs(b))
                            {
                                givedBack = Math.Abs(b);
                                avaibleMoneyToCashOut = nettoWin - givedBack;
                            }
                            else
                            {
                                givedBack = Math.Abs(b) - nettoWin;
                                avaibleMoneyToCashOut = 0;
                            }

                            CreateTransaction(b, givedBack, avaibleMoneyToCashOut);
                        }
                    }
                    else
                    {
                        if (CashResult.CashOut > Math.Abs(balance))
                        {
                            givedBack = Math.Abs(balance);
                            avaibleMoneyToCashOut = CashResult.CashOut.Value - givedBack;

                            CreateTransaction(balance, givedBack, avaibleMoneyToCashOut);
                        }
                        else
                        {
                            givedBack = CashResult.CashOut.Value;
                            avaibleMoneyToCashOut = 0;
                            CreateTransaction(balance, givedBack, avaibleMoneyToCashOut);
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

        private void CreateTransaction(double balance, double givedBack, double avaibleMoneyToCashOut)
        {
            var confirmBalanceDialog = new Dialogs.AlertDialog(string.Format("POZOR! Hráč má balance {0}€. Cash out môže byť {1}€! Stiahnutá suma: {2}€! ", balance, avaibleMoneyToCashOut, givedBack));
            confirmBalanceDialog.ShowDialog();

            BackgroundWorker bw = new BackgroundWorker();

            var loggedInUserId = App.User.UserId;

            bw.DoWork += delegate
            {
                BusinessObjects.Data.TransactionData.HandleRefactoring(CashResult.UserId, givedBack, loggedInUserId);
            };

            bw.RunWorkerCompleted += delegate
            {
                IsBusy = false;
            };

            IsBusy = true;
            bw.RunWorkerAsync();
        }

        private void txtCashOut_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCashOut.Text = string.Empty;
        }
    }
}