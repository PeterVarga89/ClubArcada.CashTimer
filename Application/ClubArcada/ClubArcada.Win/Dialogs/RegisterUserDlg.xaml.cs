using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.Data;
using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

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
                    Comment = txtDescription.Text.Trim(),
                    DateActivated = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateDeleted = null,
                    Email = txtEmail.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    IsAdmin = false,
                    IsBlocked = false,
                    IsMail = true,
                    IsPersonal = false,
                    IsSms = true,
                    NickName = txtNickName.Text,
                    Password = string.Empty,
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    PeronalId = string.Empty
                };

                var worker = new BackgroundWorker();
                worker.DoWork += delegate
                {
                    UserData.Insert(eConnectionString.Online, user);
                    UserData.Insert(eConnectionString.Local, user);

                    var mailBody = string.Format(Mailer.Constants.MailNewUserRegistrationBody, user.NickName, user.FirstName, user.LastName, user.PhoneNumber, user.Email);
                    ClubArcada.Mailer.Mailer.SendMail(Mailer.Constants.MailNewUserRegistrationSubject, mailBody);
                };

                worker.RunWorkerCompleted += delegate
                {
                    busyIndicator.IsBusy = false;
                    this.Close();
                };

                busyIndicator.IsBusy = true;
                worker.RunWorkerAsync();
            }
        }

        private bool Validate()
        {
            bool isValid = true;

            ValidateControl(ref isValid, UserData.IsNickNameExist(eConnectionString.Online, txtNickName.Text));
            ValidateControl(ref isValid, string.IsNullOrEmpty(txtFirstName.Text));
            ValidateControl(ref isValid, string.IsNullOrEmpty(txtLastName.Text));

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