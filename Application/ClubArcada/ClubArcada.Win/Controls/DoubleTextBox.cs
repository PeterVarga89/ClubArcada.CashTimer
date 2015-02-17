﻿using System.Windows.Controls;

namespace ClubArcada.Win.Controls
{
    public class DoubleTextBox : TextBox
    {
        public DoubleTextBox()
            : base()
        {
            App.KeyBoard.TBX = this;
        }

        private void ShowKeyboard()
        {
            App.KeyBoard.TBX = this;
            App.KeyBoard.Show();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            Text = Text.Replace(',', '.');
            CaretIndex = this.Text.Length;
        }

        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {
            if (this.Text == "0")
                this.Text = string.Empty;

            ShowKeyboard();
            this.Focus();
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (this.Text == "0")
                this.Text = string.Empty;

            ShowKeyboard();
        }

        protected override void OnPreviewMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (this.Text == "0")
                this.Text = string.Empty;

            ShowKeyboard();
        }

        protected override void OnPreviewTouchUp(System.Windows.Input.TouchEventArgs e)
        {
            base.OnPreviewTouchUp(e);

            if (this.Text == "0")
                this.Text = string.Empty;

            ShowKeyboard();
        }

        protected override void OnLostFocus(System.Windows.RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            App.KeyBoard.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}