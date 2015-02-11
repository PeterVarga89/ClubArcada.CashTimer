using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class AlertDialog : DialogBase
    {
        public string AlertText { get; set; }

        public AlertDialog(string alertText)
        {
            InitializeComponent();
            DataContext = this;

            AlertText = alertText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}