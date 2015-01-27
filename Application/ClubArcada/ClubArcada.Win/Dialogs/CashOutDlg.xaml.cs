﻿using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Globalization;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class CashOutDlg : Window
    {
        public Controls.PlayerLineCtrl Sender { get; set; }

        public CashResult CashResult { get; set; }

        public CashOutDlg(CashResult cashResult)
        {
            InitializeComponent();

            CashResult = cashResult;
            InitializeComponent();
            DataContext = this;

            txtDate.Text = DateTime.Now.ToString(new CultureInfo("sk-SK"));

            Title = string.Format("Cash Out - {0}", cashResult.User.FullDislpayName);

            txtCashOut.Focus();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CashResult.EndTime = null;
            Sender.Timer.Start();
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CashResult.EndTime = DateTime.Now;
            Sender.Timer.Stop();
            Sender.Refresh();
            this.Close();

            var mainWindow = (Application.Current.MainWindow as MainWindow);

            mainWindow.PlayingPlayerIds.Remove(CashResult.UserId);
        }

        private void txtCashOut_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCashOut.Text = string.Empty;
        }
    }
}