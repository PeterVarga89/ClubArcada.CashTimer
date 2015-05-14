using System;
using System.Windows;
using System.Windows.Controls;
using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.DataClasses;

namespace ClubArcada.Win.Dialogs
{
    public partial class CashInDlg : DialogBase
    {
        public CashResult CashResult { get; set; }
        public CashIn CashIn { get; set; }
        public Controls.PlayerLineCtrl Sender { get; set; }

        public double Borrowed { get; set; }

        public CashInDlg(CashResult cashResult)
        {
            CashIn = new BusinessObjects.DataClasses.CashIn();
            CashResult = cashResult;
            InitializeComponent();
            DataContext = this;

            Title = string.Format("Cash In - {0}", cashResult.User.FullDislpayName);
            this.Loaded += delegate { txtAmount.Focus(); };
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CashIn.Amount == 0)
            {
                var alertDlg = new Dialogs.AlertDialog("Cash In nemôže byť nula!");
                alertDlg.ShowDialog();
                return;
            }

            var confirmText = string.Format("Určite chcete vykonať cash in {1}€ pre hráča {0} ?", CashResult.User.FullName, CashIn.Amount);

            var confirmDlg1 = new Dialogs.ConfirmDialog(confirmText);
            confirmDlg1.ShowDialog();
            if (confirmDlg1.DialogResult.HasValue && confirmDlg1.DialogResult.Value)
            {
                if (Borrowed != 0)
                {
                    if (Borrowed > CashIn.Amount)
                    {
                        var alertDlg = new Dialogs.AlertDialog("Požičané nemôže byť väčšie ako Cash In!");
                        alertDlg.ShowDialog();
                        return;
                    }

                    var confirmDlg = new Dialogs.ConfirmDialog(string.Format("Určite chcete požičať peniaze? ({0}€)", Borrowed));
                    confirmDlg.ShowDialog();

                    if (!confirmDlg.DialogResult.HasValue || (confirmDlg.DialogResult.HasValue && confirmDlg.DialogResult.Value == false))
                    {
                        return;
                    }

                    BusinessObjects.Data.TransactionData.Create(new Transaction()
                    {
                        Amount = Borrowed * (-1),
                        Amount2 = Borrowed * (-1),
                        UserId = CashResult.User.UserId,
                        TransactionType = (int)eTransactionType.CashGame,
                        DateDeleted = null,
                        DateUsed = null,
                        Description = string.Empty,
                        CratedByUserId = App.User.UserId
                    });
                }

                CashIn.CashInId = Guid.NewGuid();
                CashIn.DateCreated = DateTime.Now;
                CashIn.CashResultId = CashResult.CashResultId;
                CashResult.CashIns.Add(CashIn);
                Sender.Refresh();
                this.DialogResult = true;
                this.Close();
            }
        }

        private void txtAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = string.Empty;
        }
    }
}