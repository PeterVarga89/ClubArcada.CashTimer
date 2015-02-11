using System;
using System.Windows;
using System.Windows.Controls;

namespace ClubArcada.Win.Dialogs
{
    public partial class AddPlayerDlg : DialogBase
    {
        public MainWindow Main { get; set; }

        public AddPlayerDlg()
        {
            InitializeComponent();
            this.Closed += AddPlayerDlg_Closed;
        }

        private void AddPlayerDlg_Closed(object sender, System.EventArgs e)
        {
            if (ToOpenOnClose != null)
                ToOpenOnClose.ShowDialog();
        }

        private Window ToOpenOnClose { get; set; }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExistingPlayer_Click(object sender, RoutedEventArgs e)
        {
            ToOpenOnClose = new Dialogs.NewPlayerDlg(new Guid((Main.tabCtrl.SelectedContent as ListBox).Tag.ToString()));
            (ToOpenOnClose as NewPlayerDlg).Main = Main;
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            ToOpenOnClose = new Dialogs.RegisterUserDlg();
            (ToOpenOnClose as RegisterUserDlg).Main = Main;
            this.Close();
        }

        private void Inkognito_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}