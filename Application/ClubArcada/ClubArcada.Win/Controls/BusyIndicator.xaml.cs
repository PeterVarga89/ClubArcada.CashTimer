using System;
using System.Windows;
using System.Windows.Controls;

namespace ClubArcada.Win.Controls
{
    public partial class BusyIndicator : UserControl
    {
        public BusyIndicator()
        {
            InitializeComponent();
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        public Boolean IsVisible { get { return (Boolean)this.GetValue(IsVisibleProperty); } set { this.SetValue(IsVisibleProperty, value); } }
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(Boolean), typeof(BusyIndicator), new PropertyMetadata(false));
    }
}