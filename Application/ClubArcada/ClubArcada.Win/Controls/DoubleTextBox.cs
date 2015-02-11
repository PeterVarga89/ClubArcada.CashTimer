using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClubArcada.Win.Controls
{
    public class DoubleTextBox : TextBox
    {
        protected override void OnKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyUp(e);

            Text = Text.Replace(',', '.');
            CaretIndex = this.Text.Length;
        }
    }
}
