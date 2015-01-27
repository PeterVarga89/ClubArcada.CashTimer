using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClubArcada.Win.Dialogs
{
    public partial class NewTableDlg : Window
    {
        public NewTableDlg()
        {
            InitializeComponent();
            DataContext = this;
            this.Loaded += NewTableDlg_Loaded;

            Title = "Nový stôl";
        }

        void NewTableDlg_Loaded(object sender, RoutedEventArgs e)
        {
            var listToDelete = new List<ComboBoxItem>();

            foreach (var item in cbTable.Items)
            {
                var cbxItem = (ComboBoxItem)item;

                var tableNames = Main.Tables.Select(t => t.Name.ToLower()).ToList();
                var name = cbxItem.Content.ToString();

                foreach (var tn in tableNames)
                {
                    if (tn.Contains(name))
                        listToDelete.Add(cbxItem);
                }
            }

            foreach (var ltd in listToDelete)
            {
                cbTable.Items.Remove(ltd);
            }

        }

        public MainWindow Main { get; set; }

        public CashTable CashTable { get; set; }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (cbGameType.SelectedItem == null)
                return;

            if (cbTable.SelectedItem == null)
                return;

            CashTable = new CashTable();
            CashTable.CashTableId = Guid.NewGuid();
            CashTable.DateCreated = DateTime.Now;
            CashTable.GameType = int.Parse((cbGameType.SelectedItem as ComboBoxItem).Tag.ToString());
            CashTable.Name = string.Format("Stôl č. {0}", (cbTable.SelectedItem as ComboBoxItem).Tag.ToString());

            Main.Tables.Add(CashTable);
            Main.AddTable(CashTable);
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
