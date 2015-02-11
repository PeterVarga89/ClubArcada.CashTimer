using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using ClubArcada.Win.Controls;
using ClubArcada.Win.Other;
using InputPanelConfigurationLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ClubArcada.Win
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        # region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void PropertyChange(Property property)
        {
            PropertyChanged.Raise(this, property.ToString());
        }

        private enum Property
        {
            NotSet = 0,
            PausedVisibility,
            IsShadeVisible
        }

        private void Refresh()
        {
            PropertyChange(Property.PausedVisibility);
        }

        # endregion

        private Visibility _isShadeVisible;
        public Visibility IsShadeVisible
        {
            get
            {
                return _isShadeVisible;
            }
            set
            {
                _isShadeVisible = value;
                PropertyChange(Property.IsShadeVisible);
            }
        }

        public Visibility PausedVisibility { get { return IsPaused ? Visibility.Visible : Visibility.Collapsed; } }

        public Tournament Tournament { get; set; }

        public List<CashTable> Tables { get; set; }

        public List<CashResult> CashResults { get; set; }

        public List<Guid> PlayingPlayerIds { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            IsShadeVisible = System.Windows.Visibility.Collapsed;
            this.Loaded += MainWindow_Loaded;
            DataContext = this;
            App.ParentWindow = this;
            InkInputHelper.DisableWPFTabletSupport();

            this.SizeChanged += MainWindow_SizeChanged;

            if (App.User.IsNotNull())
                txtLoggedInUser.Text = string.Format("Zodpovedný: {0},", App.User.FullName);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HandleBackground(e.NewSize.Width, e.NewSize.Height);
        }

        private void HandleBackground(double width, double height)
        {
            if (Height > width)
            {
                img_bg.Source = (ImageSource)FindResource("ImagePortrait");
            }
            else
            {
                img_bg.Source = (ImageSource)FindResource("ImageLandscape");
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HandleBackground(this.ActualWidth, this.ActualHeight);

            //Windows 8 API to enable touch keyboard to monitor for focus tracking in this WPF application
            InputPanelConfiguration cp = new InputPanelConfiguration();
            IInputPanelConfiguration icp = cp as IInputPanelConfiguration;
            if (icp != null)
                icp.EnableFocusTracking();

            var checkTimer = new DispatcherTimer();
            checkTimer.Interval = new TimeSpan(0, 0, 5);
            checkTimer.Tick += checkTimer_Tick;
            checkTimer.Start();

            var syncDlg = new Dialogs.SyncDlg(this);
            syncDlg.ShowDialog();

            CashResults = new List<CashResult>();
            Tables = new List<CashTable>();
            PlayingPlayerIds = new List<Guid>();
        }

        private void checkTimer_Tick(object sender, EventArgs e)
        {
            txtBattery.Text = string.Format("Battery: {0}%", System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifePercent * 100);
        }

        public void AddPlayer(Guid tableId, CashResult cashResult)
        {
            var table = Tables.SingleOrDefault(t => t.CashTableId == tableId);
            cashResult.CashTableId = tableId;

            if (table.CashResults.IsNull())
                table.CashResults = new System.Collections.ObjectModel.ObservableCollection<CashResult>();

            if (table.ActivePlayerCount == 9)
            {
                Dialogs.AlertDialog alertDlg = new Dialogs.AlertDialog("Stôl je plný!");
                alertDlg.ShowDialog();
                return;
            }

            cashResult.GameType = table.GameTypeEnum;

            table.CashResults.Add(cashResult);
            table.RefreshVisibility();
            CashResults.Add(cashResult);

            foreach (var item in tabCtrl.Items)
            {
                var tabitem = (item as TabItem);

                if (tableId.ToString() == tabitem.Tag.ToString())
                {
                    PlayingPlayerIds.Add(cashResult.UserId);
                    (tabitem.Content as ListBox).Items.Add(new Controls.PlayerLineCtrl(cashResult, this));
                    tabitem.IsSelected = true;
                    tabitem.Focus();
                }
            }
        }

        public void AddTable(CashTable table)
        {
            var tmpl = (ControlTemplate)this.FindResource("TabItemDataTemplate");

            var tabitem = new TabItemCtrl() { Header = table.Name, DataContext = table, Template = tmpl, Tag = table.CashTableId.ToString(), IsSelected = true };
            tabitem.Content = new ListBox() { Padding = new Thickness(0), ItemContainerStyle = new Style(), Tag = table.CashTableId.ToString(), Background = Brushes.Transparent, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch, BorderThickness = new Thickness(0) };

            tabCtrl.Items.Add(tabitem);
            BusinessObjects.Data.TableData.Insert(BusinessObjects.eConnectionString.Local, table);
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
                Environment.Exit(0);
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

        public double TotalCashOut
        {
            get
            {
                return CashResults.Sum(cr => cr.CashOut.HasValue ? cr.CashOut.Value : 0);
            }
        }

        private void SetExistingData()
        {
            var tables = BusinessObjects.Data.TableData.GetList(BusinessObjects.eConnectionString.Local, Tournament.TournamentId);

            if (tables != null && tables.Count > 0)
            {
                foreach (var table in tables)
                {
                    AddTable(table);
                }
            }
        }

        private void DoGlobalPause(bool doPlay)
        {
            foreach (var tabItem in tabCtrl.Items)
            {
                foreach (var sp in LogicalTreeHelper.GetChildren(tabItem as FrameworkElement))
                {
                    foreach (PlayerLineCtrl tb in FindLogicalChildren<PlayerLineCtrl>(sp as FrameworkElement))
                    {
                        if (!tb.Result.EndTime.HasValue)
                        {
                            if (doPlay)
                            {
                                tb.Timer.Start();
                            }
                            else
                            {
                                tb.Timer.Stop();
                            }
                            tb.Refresh();
                        }
                    }
                }
            }

            Refresh();
        }

        public bool IsPaused = true;

        private void btnGlobalPause_Click(object sender, RoutedEventArgs e)
        {
            IsPaused = !IsPaused;

            if (!IsPaused)
            {
                btnGlobalPause.Content = "Pause";
                DoGlobalPause(true);
            }
            else
            {
                btnGlobalPause.Content = "Start";
                DoGlobalPause(false);
            }
        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            var returnList = new List<T>();

            if (depObj != null)
            {
                foreach (var child in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (child != null && child is T)
                    {
                        returnList.Add((T)child);
                    }
                }
            }
            return returnList;
        }

        private void btnChangeBlinds_Click(object sender, RoutedEventArgs e)
        {
            if (tabCtrl.SelectedContent == null)
            {
                var alertDlg = new Dialogs.AlertDialog("Najprv pridajte stôl!");
                alertDlg.ShowDialog();
                return;
            }

            var confirmDlg = new Dialogs.ConfirmDialog("Určite chcete zmeniť blindy?");
            confirmDlg.ShowDialog();

            if (!confirmDlg.DialogResult.HasValue || (confirmDlg.DialogResult.HasValue && confirmDlg.DialogResult.Value == false))
            {
                return;
            }

            if (tabCtrl.SelectedContent == null)
                return;

            var tableId = new Guid((tabCtrl.SelectedContent as ListBox).Tag.ToString());

            var table = Tables.SingleOrDefault(t => t.CashTableId == tableId);

            if (table.GameType == 1)
            {
                table.GameType = 2;
            }
            else
            {
                table.GameType = 1;
            }

            table.RefreshVisibility();

            foreach (var c in CashResults.Where(cr => cr.CashTableId == table.CashTableId))
            {
                c.GameType = table.GameTypeEnum;
            }
        }

        public void CreateCashinForBonus(Guid userId, decimal amount)
        {
        }

        private void btnTrasaction_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Dialogs.TransactionDlg();
            dlg.Show();
        }
    }
}