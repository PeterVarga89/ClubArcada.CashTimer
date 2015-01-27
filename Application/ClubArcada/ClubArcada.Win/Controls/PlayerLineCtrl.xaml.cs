using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Win.Other;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ClubArcada.Win.Controls
{
    public partial class PlayerLineCtrl : UserControl, INotifyPropertyChanged
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
            Result,
            IsGrayVisible,
            PauseBtnVisibility,
            PlayBtnVisibility
        }

        # endregion

        public Visibility IsGrayVisible { get { return Result.EndTime.HasValue ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility PauseBtnVisibility { get { return Timer.IsEnabled ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility PlayBtnVisibility { get { return !Timer.IsEnabled ? Visibility.Visible : Visibility.Collapsed; } }

        public CashResult Result { get; set; }

        public DispatcherTimer Timer { get; set; }

        private MainWindow Main { get; set; }

        public PlayerLineCtrl(CashResult res, MainWindow mainWindow)
        {
            Result = res;
            Main = mainWindow;
            InitializeComponent();
            DataContext = this;

            Timer = new DispatcherTimer();
            Timer.Interval = new System.TimeSpan(0, 1, 0);
            Timer.Tick += Timer_Tick;
            if (!(Application.Current.MainWindow as MainWindow).IsPaused)
                Timer.Start();

            txtTimer.Text = "0 mins";
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            Result.Duration = Result.Duration + (int)Result.GameType;
            Result.MinutesPlayed++;

            txtTimer.Text = string.Format("{0} mins", Result.MinutesPlayed);
            PropertyChange(Property.Result);
        }

        private void btnCashIn_Click(object sender, RoutedEventArgs e)
        {
            Dialogs.CashInDlg cashInDlg = new Dialogs.CashInDlg(Result);
            cashInDlg.Sender = this;
            cashInDlg.ShowDialog();
        }

        private void btnCashOut_Click(object sender, RoutedEventArgs e)
        {
            this.Timer.Stop();
            Result.EndTime = DateTime.Now;
            Dialogs.CashOutDlg cashInDlg = new Dialogs.CashOutDlg(Result);
            cashInDlg.Sender = this;
            cashInDlg.ShowDialog();

            if(Result.EndTime.HasValue)
            {
                foreach(var t in Main.Tables)
                {
                    t.RefreshVisibility();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
        }

        public void Refresh()
        {
            PropertyChanged.Raise(this, Property.Result.ToString());
            PropertyChanged.Raise(this, Property.IsGrayVisible.ToString());
            PropertyChanged.Raise(this, Property.PauseBtnVisibility.ToString());
            PropertyChanged.Raise(this, Property.PlayBtnVisibility.ToString());
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            gridPause.Visibility = Visibility.Collapsed;
            if (!Timer.IsEnabled)
                Timer.Start();

            Refresh();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            gridPause.Visibility = System.Windows.Visibility.Visible;
            if (Timer.IsEnabled)
                Timer.Stop();

            Refresh();
        }

        private void btnRePlay_Click(object sender, RoutedEventArgs e)
        {
            Result.EndTime = null;
            Dialogs.CashInDlg cashInDlg = new Dialogs.CashInDlg(Result);
            cashInDlg.Sender = this;
            cashInDlg.ShowDialog();

            if (cashInDlg.DialogResult.HasValue && cashInDlg.DialogResult.Value)
            {
                Timer.Start();
                Refresh();
            }

            foreach (var t in Main.Tables)
            {
                t.RefreshVisibility();
            }
        }
    }
}