using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class AddPlayerDlg : Window
    {
        public MainWindow Main { get; set; }

        public AddPlayerDlg()
        {
            InitializeComponent();
            this.Closed += AddPlayerDlg_Closed;
        }

        void AddPlayerDlg_Closed(object sender, System.EventArgs e)
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
            ToOpenOnClose = new Dialogs.NewPlayerDlg();
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
