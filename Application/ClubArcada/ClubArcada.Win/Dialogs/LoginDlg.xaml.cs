using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            var user = BusinessObjects.Data.UserData.Login(txtEmail.Text, txtPassword.Password);
            if(user == null)
            {
                return;
            }
            else
            {
                App.User = user;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
