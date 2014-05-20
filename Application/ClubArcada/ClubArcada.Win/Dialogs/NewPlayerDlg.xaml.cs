using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClubArcada.Common;

namespace ClubArcada.Win.Dialogs
{
    public partial class NewPlayerDlg : Window
    {
        CashResult Result { get; set; }

        public List<ClubArcada.BusinessObjects.Enumerators.eGameType> GameTypeList { get; set; }

        Double Amount { get; set; }

        public NewPlayerDlg()
        {
            InitializeComponent();
            Result = new CashResult();

            Result.CashResultId = Guid.NewGuid();
            DataContext = this;
            GameTypeList = Common.Extensions.GetValueList<ClubArcada.BusinessObjects.Enumerators.eGameType>();
        }

        private bool Validate()
        {
            bool isValid = true;

            isValid = Amount != 0;

            return isValid;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                CashResult res = new CashResult();
                res.CashIns.Add(new CashIn() { Amount = Amount });
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if(textBox.Text != string.Empty)
            {
               var userList = BusinessObjects.Data.UserData.GetListBySearchString(ClubArcada.BusinessObjects.Enumerators.eConnectionString.Online, textBox.Text);

               lbxUsers.ItemsSource = userList;
               lbxUsers.DisplayMemberPath = "FullDislpayName";
            }
            else
            {
                lbxUsers.ItemsSource = null;
            }
        }
    }
}
