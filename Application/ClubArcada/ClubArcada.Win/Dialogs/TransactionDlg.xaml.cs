using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.DataClasses;
using System.Windows;
using System.Windows.Controls;

namespace ClubArcada.Win.Dialogs
{
    public partial class TransactionDlg : DialogBase
    {
        public User User { get; set; }

        public Transaction Transaction { get; set; }

        public TransactionDlg()
        {
            InitializeComponent();
            this.DataContext = this;
            Transaction = new Transaction() { CratedByUserId = App.User.UserId, Description = string.Empty };
            this.Loaded += delegate { txtSearch.Focus(); };
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (User == null)
            {
                var alertDlg = new Dialogs.AlertDialog("Vyberte si hráča!");
                alertDlg.ShowDialog();
                return;
            }

            if (Transaction.Amount == 0)
            {
                var alertDlg = new Dialogs.AlertDialog("Suma nemôže byť nula!");
                alertDlg.ShowDialog();
                return;
            }

            if (cbType.SelectedItem == null)
            {
                var alertDlg = new Dialogs.AlertDialog("Vyberte si typ!");
                alertDlg.ShowDialog();
                return;
            }

            Transaction.UserId = User.UserId;

            string type = (cbType.SelectedItem as ComboBoxItem).Content.ToString();

            var subject = string.Empty;

            BusinessObjects.Data.TransactionData.HandleRefactoring(Transaction.UserId, Transaction.Amount, App.User.UserId, "Vrátené ručne - [generované aplikáciou]");

            if (type == "Požičané")
            {
                Transaction.TransactionType = 0;
                Transaction.Amount = Transaction.Amount * (-1);
                subject = ClubArcada.Mailer.Constants.MailNewBorrowSubject;
            }
            else
            {
                Transaction.TransactionType = 1;
                subject = ClubArcada.Mailer.Constants.MailNewReplySubject;
            }
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lbxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lbxUsers = sender as ListBox;
            if (lbxUsers.SelectedItem != null)
            {
                var selectedUser = lbxUsers.SelectedItem as User;
                txtSearch.Text = selectedUser.FullDislpayName;
                User = selectedUser;
                lbxUsers.ItemsSource = null;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox.Text != string.Empty)
            {
                var userList = BusinessObjects.Data.UserData.GetListBySearchString(eConnectionString.Local, textBox.Text);
                lbxUsers.ItemsSource = userList;
                lbxUsers.DisplayMemberPath = "FullDislpayName";
            }
            else
            {
                lbxUsers.ItemsSource = null;
            }
        }
    }
}