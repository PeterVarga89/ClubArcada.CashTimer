using ClubArcada.BusinessObjects;
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

        # endregion

        public Visibility IsGrayVisible { get { return Result.EndTime.HasValue ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility PauseBtnVisibility { get { return Timer.IsEnabled ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility PlayBtnVisibility { get { return !Timer.IsEnabled && !App.ParentWindow.IsPaused ? Visibility.Visible : Visibility.Collapsed; } }

        public CashResult Result { get; set; }

        private DispatcherTimer Timer { get; set; }

        public PlayerLineCtrl(CashResult res)
        {
            Result = res;
            InitializeComponent();
            DataContext = this;

            Timer = new DispatcherTimer();
            Timer.Interval = new System.TimeSpan(0, 1, 0);
            Timer.Tick += Timer_Tick;
            if (!App.ParentWindow.IsPaused)
                Start();
            else
                Stop();

            txtTimer.Text = "0 mins";
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            //NORMAL
            //Result.Duration = Result.Duration + (int)Result.GameType;
             
            Result.MinutesPlayed++;

            //CASHBACK
            Result.Duration = Result.MinutesPlayed;

            //BENDIK
            //Result.Duration = Result.MinutesPlayed / 60;

            txtTimer.Text = string.Format("{0} mins", Result.MinutesPlayed);
            PropertyChanged.Raise(() => Result);
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
                foreach(var t in App.ParentWindow.Tables)
                {
                    t.RefreshVisibility();
                }

                gridPause.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
        }

        public void Refresh()
        {
            PropertyChanged.Raise(() => Result);
            PropertyChanged.Raise(() => IsGrayVisible);
            PropertyChanged.Raise(() => PauseBtnVisibility);
            PropertyChanged.Raise(() => PlayBtnVisibility);
        }

        public void Start()
        {
            gridPause.Visibility = Visibility.Collapsed;
            Timer.IsEnabled = true;
            Refresh();
        }

        public void Stop()
        {
            gridPause.Visibility = Visibility.Visible;
            Timer.IsEnabled = false;
            Refresh();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.Start();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            this.Stop();
        }

        private void btnRePlay_Click(object sender, RoutedEventArgs e)
        {
            Result.EndTime = null;
            Dialogs.CashInDlg cashInDlg = new Dialogs.CashInDlg(Result);
            cashInDlg.Sender = this;
            cashInDlg.ShowDialog();

            if (cashInDlg.DialogResult.HasValue && cashInDlg.DialogResult.Value)
            {
                this.Start();
            }

            foreach (var t in App.ParentWindow.Tables)
            {
                t.RefreshVisibility();
            }
        }

        private void btnBonus_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Dialogs.BonusDialog(Result.User);
            dlg.ShowDialog();
            this.Refresh();
        }
    }
}