using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class CashInDlg : Window
    {
        public CashResult CashResult { get; set; }
        public CashIn CashIn { get; set; }
        public Controls.PlayerLineCtrl Sender { get; set; }

        public CashInDlg(CashResult cashResult)
        {
            CashIn = new BusinessObjects.DataClasses.CashIn();
            CashResult = cashResult;
            InitializeComponent();
            DataContext = this;

            Title = string.Format("Cash In - {0}", cashResult.User.FullDislpayName);

            txtAmount.Focus();
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(CashIn.Amount < 30)
            {
                Dialogs.AlertDialog aDlg = new AlertDialog("Minimum 30€ !");
                aDlg.ShowDialog();
                return;
            }

            CashIn.CashInId = Guid.NewGuid();
            CashIn.DateCreated = DateTime.Now;
            CashIn.CashResultId = CashResult.CashResultId;
            CashResult.CashIns.Add(CashIn);
            Sender.Refresh();
            this.DialogResult = true;
            this.Close();
        }

        private void txtAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = string.Empty;
        }
    }
}
