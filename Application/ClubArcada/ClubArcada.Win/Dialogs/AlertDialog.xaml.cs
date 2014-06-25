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
    public partial class AlertDialog : Window
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
