using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClubArcada.Win.Dialogs
{
    public partial class KeyboardDlg : Window
    {
        private List<Button> ButtonList { get; set; }

        private TextBox _tbx;
        public TextBox TBX
        {
            get
            {
                return _tbx;
            }
            set
            {
                _tbx = value;
            }
        }

        public KeyboardDlg()
        {
            InitializeComponent();
            this.Loaded += KeyboardDlg_Loaded;
        }

        private void KeyboardDlg_Loaded(object sender, RoutedEventArgs e)
        {
            this.Owner = App.ParentWindow;

            this.SizeChanged += delegate { SetSizeAndPosition(); };
            this.Owner.SizeChanged += delegate { SetSizeAndPosition(); };

            SetSizeAndPosition();
            ButtonList = FindVisualChildren<Button>(gridContainer).ToList();

            foreach (var b in ButtonList)
                b.Click += b_Click;

            foreach (var b in ButtonList)
                b.Content = b.Content.ToString().ToLower();
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);

            if (btn.Content.ToString().ToLower() == "↑".ToLower())
            {
                btn.Content = "↓".ToLower();
                foreach (var b in ButtonList)
                {
                    b.Content = b.Content.ToString().ToUpper();
                }
            }
            else if (btn.Content.ToString().ToLower() == "↓".ToLower())
            {
                btn.Content = "↑".ToLower();
                foreach (var b in ButtonList)
                {
                    b.Content = b.Content.ToString().ToLower();
                }
            }

            if (btn.Content.ToString().ToLower() == "ok")
            {
                this.Visibility = System.Windows.Visibility.Hidden;
                TBX.Focus();
            }

            if (btn.Content.ToString().ToLower() == "←".ToLower())
            {
                if (TBX.Text != string.Empty)
                {
                    TBX.Text = TBX.Text.Remove(TBX.Text.Length - 1, 1);
                    TBX.CaretIndex = TBX.Text.Length;
                    TBX.Focus();
                }
            }

            if (btn.Content.ToString().Length == 1 && btn.Content.ToString().ToLower() != "↓".ToLower() && btn.Content.ToString().ToLower() != "↑".ToLower() && btn.Content.ToString().ToLower() != "←".ToLower())
            {
                TBX.Text = TBX.Text + btn.Content.ToString();
                TBX.CaretIndex = TBX.Text.Length;
                TBX.Focus();
            }

            if (btn.Content.ToString().ToLower() == "Medzera".ToLower())
            {
                TBX.Text = TBX.Text + " ";
                TBX.CaretIndex = TBX.Text.Length;
                TBX.Focus();
            }
        }

        private void SetSizeAndPosition()
        {
            this.Width = App.ParentWindow.ActualWidth;
            this.Left = this.Owner.Left + 8;
            this.Top = this.Owner.ActualHeight - this.ActualHeight;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}