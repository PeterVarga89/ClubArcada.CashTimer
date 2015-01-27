using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClubArcada.Win.Dialogs
{
    public partial class NewPlayerDlg : Window
    {
        public CashResult Result { get; set; }

        public MainWindow Main { get; set; }

        public List<eGameType> GameTypeList { get; set; }

        public Double Amount { get; set; }

        public NewPlayerDlg(Guid selectedTableId)
        {
            InitializeComponent();
            Result = new CashResult();
            Result.CashTableId = selectedTableId;

            Result.CashResultId = Guid.NewGuid();
            DataContext = this;
            GameTypeList = Common.Extensions.GetValueList<eGameType>();
        }

        private bool Validate()
        {
            int errorCount = 0;

            errorCount = Amount == 0 ? errorCount + 1 : errorCount;
            errorCount = cbTable.SelectedItem == null ? errorCount + 1 : errorCount;
            errorCount = Result.User == null ? errorCount + 1 : errorCount;

            return errorCount == 0;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                Result.CashResultId = Guid.NewGuid();
                Result.StartTime = DateTime.Now;
                Result.PlayerId = Guid.NewGuid();
                Result.UserId = Result.User.UserId;
                Result.TournamentId = Main.Tournament.TournamentId;

                Result.CashIns = new List<CashIn>();
                Result.CashIns.Add(new CashIn()
                {
                    Amount = Amount,
                    CashInId = Guid.NewGuid(),
                    CashResultId = Result.CashResultId,
                    DateCreated = DateTime.Now
                });

                Main.AddPlayer((cbTable.SelectedItem as CashTable).CashTableId, Result);
                this.Close();
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox.Text != string.Empty)
            {
                var userList = BusinessObjects.Data.UserData.GetListBySearchString(eConnectionString.Local, RemoveDiacritics(textBox.Text));

                var filteredUserList = userList.Where(u => !Main.PlayingPlayerIds.Contains(u.UserId)).ToList();
                lbxUsers.ItemsSource = filteredUserList;
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
                Result.User = selectedUser;
                txtSearch.Text = selectedUser.FullDislpayName;
                lbxUsers.ItemsSource = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = string.Empty;
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
    }
}