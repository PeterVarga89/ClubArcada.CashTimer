using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.Data;
using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClubArcada.Win.Other;

namespace ClubArcada.Win.Dialogs
{
    public partial class RegisterUserDlg : DialogBase
    {
        public RegisterUserDlg()
        {
            InitializeComponent();
            this.Loaded += delegate { txtNickName.Focus(); };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                User user = new User()
                {
                    UserId = Guid.NewGuid(),
                    Comment = string.Empty,
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
                    UserData.Insert(eConnectionString.Online, user.Clone());
                    UserData.Insert(eConnectionString.Local, user.Clone());

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

            ValidateControl(txtNickName, ref isValid, UserData.IsNickNameExist(eConnectionString.Online, txtNickName.Text));
            ValidateControl(txtFirstName, ref isValid, string.IsNullOrEmpty(txtFirstName.Text));
            ValidateControl(txtLastName, ref isValid, string.IsNullOrEmpty(txtLastName.Text));

            return isValid;
        }

        private void ValidateControl(Control control, ref bool isValid, bool req)
        {
            if (req)
            {
                isValid = false;
                control.Background = Brushes.Red;
            }
            else
            {
                control.Background = Brushes.White;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}