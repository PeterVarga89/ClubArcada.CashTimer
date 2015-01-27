using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class ConfirmDialog : Window
    {
        public string AlertText { get; set; }

        public ConfirmDialog(string alertText)
        {
            InitializeComponent();
            DataContext = this;
            AlertText = alertText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}