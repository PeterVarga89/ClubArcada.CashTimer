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
    public partial class TotalCashOutDlg : Window
    {
        public MainWindow Main { get; set; }

        public DateTime Date { get { return Main.StartDate; } }
        public int CashInTotal { get { return Main.TotalCashIn; } }
        public int CashOutTotal { get { return Main.TotalCashOut; } }
        public int Food { get; set; }
        public int CGLeague { get; set; }
        public int APCLeague { get; set; }
        public int PaidBonus { get; set; }

        public string Comment { get; set; }

        public TotalCashOutDlg(MainWindow main)
        {
            Main = main;
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
