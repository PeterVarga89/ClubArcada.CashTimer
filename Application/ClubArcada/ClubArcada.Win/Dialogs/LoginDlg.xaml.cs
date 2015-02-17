using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.ComponentModel;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class LoginDlg : Window
    {
        public LoginDlg()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == string.Empty)
                return;

            if (txtPassword.Password == string.Empty)
                return;

            var mail = txtEmail.Text.Trim();
            var pass = txtPassword.Password.Trim();

            var worker = new BackgroundWorker();
            User user = null;

            worker.DoWork += delegate
            {
                user = BusinessObjects.Data.UserData.Login(mail, pass);
            };

            worker.RunWorkerCompleted += delegate
            {
                busy.IsBusy = false;
                if (user != null)
                {
                    App.User = user;
                    this.DialogResult = true;
                    this.Close();
                }
            };

            worker.RunWorkerAsync();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}