using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Mailer;
using ClubArcada.Win.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClubArcada.Win.Dialogs
{
    public partial class NewPlayerDlg : DialogBase
    {
        public List<CashTable> TableList { get { return App.ParentWindow.Tables; } }

        public CashResult Result { get; set; }

        public List<eGameType> GameTypeList { get; set; }

        public double Amount { get; set; }

        public double Borrowed { get; set; }

        public NewPlayerDlg(Guid selectedTableId)
        {
            InitializeComponent();
            Result = new CashResult();
            Result.CashTableId = selectedTableId;
            this.Owner = App.ParentWindow;

            Result.CashResultId = Guid.NewGuid();
            DataContext = this;
            GameTypeList = Common.Extensions.GetValueList<eGameType>();
            this.Loaded += delegate { txtSearch.Focus(); };
        }

        private bool Validate()
        {
            int errorCount = 0;

            errorCount = cbTable.SelectedItem == null ? errorCount + 1 : errorCount;
            errorCount = Result.User == null ? errorCount + 1 : errorCount;

            return errorCount == 0;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                if (Result.User == null)
                {
                    var alertDlg = new Dialogs.AlertDialog("Vyberte si hráča!");
                    alertDlg.ShowDialog();
                    return;
                }

                if (Amount == 0)
                {
                    var alertDlg = new Dialogs.AlertDialog("Cash in nemôže byť nula!");
                    alertDlg.ShowDialog();
                    return;
                }

                var confirmText = string.Format("Určite chcete registrovať hráča {0} do hry, cash in: {1}€ ?", Result.User.FullName, Amount);

                var confirmDlg1 = new Dialogs.ConfirmDialog(confirmText);
                confirmDlg1.ShowDialog();
                if (confirmDlg1.DialogResult.HasValue && confirmDlg1.DialogResult.Value)
                {
                    if (Borrowed != 0)
                    {
                        if (Borrowed > Amount)
                        {
                            var alertDlg = new Dialogs.AlertDialog("Požičaná čiastka nemôže byť väčšia ako Cash in!");
                            alertDlg.ShowDialog();
                            return;
                        }

                        var confirmDlg = new Dialogs.ConfirmDialog(string.Format("Určite chcete požičať peniaze? ({0}€)", Borrowed));
                        confirmDlg.ShowDialog();

                        if (!confirmDlg.DialogResult.HasValue || (confirmDlg.DialogResult.HasValue && confirmDlg.DialogResult.Value == false))
                        {
                            return;
                        }

                        BusinessObjects.Data.TransactionData.Create(eConnectionString.Online, new Transaction()
                        {
                            Amount = Borrowed * (-1),
                            UserId = Result.User.UserId,
                            TransactionType = (int)eTransactionType.NotSet,
                            DateDeleted = null,
                            DateUsed = null,
                            Description = string.Empty,
                            CratedByUserId = App.User.UserId
                        });

                        var balance = BusinessObjects.Data.UserData.GetUserBalance(Result.User.UserId);
                        var mailBody = string.Format(Mailer.Constants.MailNewBorrowBody, Result.User.NickName, Result.User.FirstName, Result.User.LastName, "Cash Game", Borrowed, balance, App.User.FullName);
                        ClubArcada.Mailer.Mailer.SendMail(Mailer.Constants.MailNewBorrowSubject, mailBody);
                    }

                    Result.CashResultId = Guid.NewGuid();
                    Result.StartTime = DateTime.Now;
                    Result.PlayerId = Guid.NewGuid();
                    Result.UserId = Result.User.UserId;
                    Result.TournamentId = App.ParentWindow.Tournament.TournamentId;

                    Result.CashIns = new List<CashIn>();
                    Result.CashIns.Add(new CashIn()
                    {
                        Amount = Amount,
                        CashInId = Guid.NewGuid(),
                        CashResultId = Result.CashResultId,
                        DateCreated = DateTime.Now
                    });

                    App.ParentWindow.AddPlayer((cbTable.SelectedItem as CashTable).CashTableId, Result);

                    var worker = new BackgroundWorker();
                    worker.DoWork += delegate
                    {
                        var balance = BusinessObjects.Data.UserData.GetUserBalance(Result.User.UserId);
                        if (balance < 0)
                            Mailer.Mailer.SendMail(Constants.MailUserRegisteredWithNegativeBalanceSubject, string.Format(Constants.MailUserRegisteredWithNegativeBalanceBody, Result.User.NickName, Result.User.FirstName, Result.User.LastName, balance, App.User.FullName));
                    };

                    worker.RunWorkerCompleted += delegate
                    {
                        this.Close();
                    };

                    worker.RunWorkerAsync();
                }
                else
                {
                    return;
                }
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as CustomTextBox;

            if (textBox.Text != string.Empty)
            {
                var userList = BusinessObjects.Data.UserData.GetListBySearchString(eConnectionString.Local, textBox.Text);

                var filteredUserList = userList.Where(u => !App.ParentWindow.PlayingPlayerIds.Contains(u.UserId)).ToList();
                lbxUsers.ItemsSource = filteredUserList;
                lbxUsers.DisplayMemberPath = "FullDislpayName";
            }
            else
            {
                lbxUsers.ItemsSource = null;
            }
        }
    }
}