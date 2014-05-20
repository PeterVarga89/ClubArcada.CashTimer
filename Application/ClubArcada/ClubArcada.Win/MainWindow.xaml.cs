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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClubArcada.Win
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var syncDlg = new Dialogs.SyncDlg();
            syncDlg.ShowDialog();

            InitializeComponent();

            //spPlayerLineContainer.Children.Add(new Controls.PlayerLineCtrl());
            //spPlayerLineContainer.Children.Add(new Controls.PlayerLineCtrl());
            //spPlayerLineContainer.Children.Add(new Controls.PlayerLineCtrl());
            //spPlayerLineContainer.Children.Add(new Controls.PlayerLineCtrl());
        }

        public void AddPlayer(Guid tableId, CashResult cashResult)
        {

        }

        public void AddTable(int tableName)
        {
            tabCtrl.Items.Add(new TabItem() { Header = string.Format("stôl č. {0}", tableName) });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addPlayerDlg = new Dialogs.AddPlayerDlg();
            addPlayerDlg.ShowDialog();
        }

        private void Button_btnNewTable(object sender, RoutedEventArgs e)
        {
            AddTable(1);
        }
    }
}
