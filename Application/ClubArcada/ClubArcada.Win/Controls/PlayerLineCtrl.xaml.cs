using ClubArcada.BusinessObjects.DataClasses;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ClubArcada.Win.Other;

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
            IsGrayVisible
        }

        # endregion

        public Visibility IsGrayVisible { get { return Result.EndTime.HasValue ? Visibility.Visible : Visibility.Collapsed; } }

        public CashResult Result { get; set; }

        public DispatcherTimer Timer { get; set; }

        public PlayerLineCtrl(CashResult res)
        {
            Result = res;

            InitializeComponent();
            DataContext = this;

            Timer = new DispatcherTimer();
            Timer.Interval = new System.TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
            Timer.Start();

            txtTimer.Text = "0 mins";
        }

        private int TimerValue = 1;
        private int TimerValueMins = 0;
        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (TimerValue == 60)
            {
                TimerValue = 1;
                TimerValueMins++;
            }

            txtTimer.Text = string.Format("{0} mins {1}s", TimerValueMins, TimerValue);
            TimerValue++;
        }

        private void btnCashIn_Click(object sender, RoutedEventArgs e)
        {
            Dialogs.CashInDlg cashInDlg = new Dialogs.CashInDlg(Result);
            cashInDlg.Sender = this;
            cashInDlg.ShowDialog();
        
        }

        private void btnCashOut_Click(object sender, RoutedEventArgs e)
        {
            Dialogs.CashOutDlg cashInDlg = new Dialogs.CashOutDlg(Result);
            cashInDlg.Sender = this;
            cashInDlg.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Refresh()
        {
            PropertyChanged.Raise(this, Property.Result.ToString());
            PropertyChanged.Raise(this, Property.IsGrayVisible.ToString());
        }
    }
}
