using System;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public class DialogBase : Window
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Owner = App.ParentWindow;
            App.ParentWindow.IsShadeVisible = System.Windows.Visibility.Visible;

            this.Width = App.ParentWindow.ActualWidth;
            this.Left = this.Owner.Left + 8;
            this.Top = this.Owner.Top;

            this.SizeChanged += DialogBase_SizeChanged;
            this.Owner.SizeChanged += DialogBase_SizeChanged;
        }

        private void DialogBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = App.ParentWindow.ActualWidth;
            this.Left = this.Owner.Left + 8;
            this.Top = this.Owner.Top;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            App.ParentWindow.IsShadeVisible = System.Windows.Visibility.Collapsed;
        }
    }
}