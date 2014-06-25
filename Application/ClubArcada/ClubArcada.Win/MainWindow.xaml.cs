using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ClubArcada.Win
{
    public partial class MainWindow : Window
    {
        public List<CashTable> Tables { get; set; }
        public List<CashResult> CashResults { get; set; }

        public List<Guid> PlayingPlayerIds { get; set; }

        public DateTime StartDate { get; set; }

        public MainWindow()
        {
            StartDate = DateTime.Now;

            var syncDlg = new Dialogs.SyncDlg();
            syncDlg.ShowDialog();

            InitializeComponent();

            CashResults = new List<CashResult>();
            Tables = new List<CashTable>();
            PlayingPlayerIds = new List<Guid>();

            InkInputHelper.DisableWPFTabletSupport();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Enables WPF to mark edit field as supporting text pattern (Automation Concept) 
            AutomationElement asForm = AutomationElement.FromHandle(new WindowInteropHelper(this).Handle);

            // Windows 8 API to enable touch keyboard to monitor for focus tracking in this WPF application 
            InputPanelConfigurationLib.InputPanelConfiguration inputPanelConfig = new InputPanelConfigurationLib.InputPanelConfiguration();
            inputPanelConfig.EnableFocusTracking();
        }

        public void AddPlayer(Guid tableId, CashResult cashResult)
        {
            CashResults.Add(cashResult);

            foreach (var item in tabCtrl.Items)
            {
                var tabitem = (item as TabItem);

                if(tableId.ToString() == tabitem.Tag.ToString())
                {
                    PlayingPlayerIds.Add(cashResult.UserId);
                    (tabitem.Content as StackPanel).Children.Add(new Controls.PlayerLineCtrl(cashResult));
                    tabitem.IsSelected = true;
                    tabitem.Focus();
                }
            }
        }

        public void AddTable(CashTable table)
        {
            var tabitem = new TabItem() { Header = table.Name, Tag = table.CashTableId };
            tabitem.Content = new StackPanel() { Orientation = Orientation.Vertical };

            tabCtrl.Items.Add(tabitem);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addPlayerDlg = new Dialogs.AddPlayerDlg();
            addPlayerDlg.Main = this;
            addPlayerDlg.ShowDialog();
        }

        private void Button_btnNewTable(object sender, RoutedEventArgs e)
        {
            var dlg = new Dialogs.NewTableDlg();
            dlg.Main = this;
            dlg.ShowDialog();
            tabCtrl.Focus();

            btnAddPlayer.IsEnabled = Tables.Count > 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CashResults == null || CashResults.Count == 0)
            {
                var alertDlg = new Dialogs.AlertDialog("Nie sú žiadne záznamy!");
                alertDlg.ShowDialog();
                return;
            }

            if (CashResults.Where(cr => !cr.EndTime.HasValue).ToList().Count > 0)
            {
                var alertDlg = new Dialogs.AlertDialog("Všetky hráči musia byť cashout!");
                alertDlg.ShowDialog();
                return;
            }

            Dialogs.TotalCashOutDlg totalCashoutDlg = new Dialogs.TotalCashOutDlg(this);
            totalCashoutDlg.ShowDialog();
        }

        public int TotalCashIn
        {
            get
            {
                return (int)CashResults.Sum(cr => cr.CashInTotal);
            }
        }

        public int TotalCashOut
        {
            get
            {
                return CashResults.Sum(cr => cr.CashOut.HasValue ? cr.CashOut.Value : 0);
            }
        }
    }
}
