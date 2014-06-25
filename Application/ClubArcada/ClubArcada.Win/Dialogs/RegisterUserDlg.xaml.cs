using ClubArcada.BusinessObjects.Data;
using ClubArcada.BusinessObjects.DataClasses;
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
    public partial class RegisterUserDlg : Window
    {
        public MainWindow Main { get; set; }

        public RegisterUserDlg()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                User user = new User()
                {
                    UserId = Guid.NewGuid(),
                    Comment = txtDescription.Text,
                    DateActivated = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateDeleted = null,
                    Email = txtEmail.Text,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    IsAdmin = false,
                    IsBlocked = false,
                    IsMail = true,
                    IsPersonal = false,
                    IsSms = true,
                    NickName = txtNickName.Text,
                    Password = string.Empty,
                    PhoneNumber = txtPhoneNumber.Text,
                    PeronalId = string.Empty
                };

                UserData.Insert(BusinessObjects.Enumerators.eConnectionString.Online, user);

                StringBuilder bodyMessage = new StringBuilder();
                bodyMessage.Append("Ncik: " + user.NickName).AppendLine();
                bodyMessage.Append("Meno: " + user.FirstName).AppendLine();
                bodyMessage.Append("Priezvisko: " + user.LastName).AppendLine().AppendLine();

                bodyMessage.Append("Tel.: " + user.PhoneNumber).AppendLine();
                bodyMessage.Append("E-mail.: " + user.Email).AppendLine();

                Other.Functions.SendMail("New Player Registration", bodyMessage.ToString());
                this.Close();
            }
        }

        private bool Validate()
        {
            bool isValid = true;

            ValidateControl(ref isValid, UserData.IsNickNameExist(BusinessObjects.Enumerators.eConnectionString.Online, txtNickName.Text));
            ValidateControl(ref isValid, string.IsNullOrEmpty(txtFirstName.Text));
            ValidateControl(ref isValid, string.IsNullOrEmpty(txtLastName.Text));
            ValidateControl(ref isValid, string.IsNullOrEmpty((cbxGender.SelectedItem as ComboBoxItem).Tag.ToString()));

            return isValid;
        }


        private void ValidateControl(ref bool isValid, bool req)
        {
            if (req)
            {
                isValid = false;
                txtNickName.Background = Brushes.Red;
            }
            else
            {
                txtNickName.Background = Brushes.White;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
