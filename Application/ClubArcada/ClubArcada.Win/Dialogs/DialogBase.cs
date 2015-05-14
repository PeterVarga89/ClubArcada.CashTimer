using System;
using System.ComponentModel;
using System.Windows;
using ClubArcada.Win;
using ClubArcada.BusinessObjects;

namespace ClubArcada.Win.Dialogs
{
    public class DialogBase : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isBusy;
        protected bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                PropertyChanged.Raise(() => IsBusy);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Owner = App.ParentWindow;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            App.ParentWindow.IsShadeVisible = System.Windows.Visibility.Visible;

            this.Width = this.Owner.ActualWidth  * 0.8;

            this.SizeChanged += DialogBase_SizeChanged;
            this.Owner.SizeChanged += DialogBase_SizeChanged;
        }

        private void DialogBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = this.Owner.ActualWidth - 200;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            App.ParentWindow.IsShadeVisible = System.Windows.Visibility.Collapsed;
        }
    }
}