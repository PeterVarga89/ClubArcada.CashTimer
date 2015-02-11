using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.DataClasses;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            BusinessObjects.Data.TransactionData.Create(eConnectionString.Online, Transaction);

            var balance = BusinessObjects.Data.UserData.GetUserBalance(User.UserId);
            var mailBody = string.Format(Mailer.Constants.MailNewBorrowBody, User.NickName, User.FirstName, User.LastName, "Cash Game", Transaction.Amount, balance, App.User.FullName);
            ClubArcada.Mailer.Mailer.SendMail(subject, mailBody);

            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox.Text != string.Empty)
            {
                var userList = BusinessObjects.Data.UserData.GetListBySearchString(eConnectionString.Local, RemoveDiacritics(textBox.Text));
                lbxUsers.ItemsSource = userList;
                lbxUsers.DisplayMemberPath = "FullDislpayName";
            }
            else
            {
                lbxUsers.ItemsSource = null;
            }
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
    }
}