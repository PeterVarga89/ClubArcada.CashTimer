using System;
using System.Collections.Generic;
using System.Windows;
using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.DataClasses;

namespace ClubArcada.Win.Dialogs
{
    public partial class TotalCashOutDlg : DialogBase
    {
        public int CashInTotal { get { return App.ParentWindow.TotalCashIn; } }

        public DateTime Date { get { return DateTime.Now; } }

        public double CashOutTotal { get { return App.ParentWindow.TotalCashOut; } }

        public int Floor { get; set; }

        public int Dealer { get; set; }

        public double Food { get; set; }

        public int Rake { get; set; }

        public int RunHelp { get; set; }

        public int CGLeague { get; set; }

        public int APCLeague { get; set; }

        public int PaidBonus { get; set; }

        public string Comment { get; set; }

        public TotalCashOutDlg()
        {
            InitializeComponent();
            DataContext = this;

            if (CashInTotal < CashOutTotal)
            {
                Dialogs.AlertDialog dlg = new AlertDialog("POZOR! OUT je väčší ako IN !!!");
                dlg.ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateOnline();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateOnline()
        {
            App.ParentWindow.Tournament.DateEnded = DateTime.Now;
            BusinessObjects.Data.TournamentData.Insert(eConnectionString.Online, App.ParentWindow.Tournament);

            var tc = new TournamentCashout();

            tc.APCBank = APCLeague;
            tc.CGBank = CGLeague;
            tc.Comment = "-";
            tc.Dotation = 0;
            tc.Floor = Floor;
            tc.Food = Food;
            tc.PrizePool = 0;
            tc.Rake = Rake;
            tc.RunnerHelp = RunHelp;
            tc.BonusCash = 0;
            tc.BonusUsed = PaidBonus;
            tc.Dealer = Dealer;
            tc.TournamentCashoutId = Guid.NewGuid();
            tc.TournamentId = App.ParentWindow.Tournament.TournamentId;

            BusinessObjects.Data.TournamentData.InsertCashout(eConnectionString.Online, tc);

            foreach (var t in App.ParentWindow.Tables)
                BusinessObjects.Data.TableData.Insert(eConnectionString.Online, t);

            foreach (var cr in App.ParentWindow.CashResults)
            {
                BusinessObjects.Data.CashResultData.InsertOrUpdate(eConnectionString.Online, cr);

                foreach (var ci in cr.CashIns)
                {
                    BusinessObjects.Data.CashResultData.InsertOrUpdate(eConnectionString.Online, ci);
                }
            }

            var docMemStream = ClubArcada.Documents.Documents.GetStream(DateTime.Now, App.ParentWindow.Tournament, tc, App.ParentWindow.CashResults, App.ParentWindow.Tables);
            ClubArcada.Mailer.Mailer.SendMail("Cash Game Report_" + DateTime.Now.ToString("dd_MM_yyyy"), docMemStream.Copy(), "cashgame_report_" + DateTime.Now.ToString("dd_MM_yyyy") + ".pdf", "");
            Environment.Exit(0);
        }

        private List<CashResult> GetFixedCashResults(List<CashResult> list)
        {
            List<CashResult> newList = new List<CashResult>();

            foreach (var c in list)
            {
                foreach (var cr in list)
                {
                    if (c.CashResultId != cr.CashResultId && c.UserId == cr.UserId)
                    {
                        c.Duration = c.Duration + cr.Duration;
                    }
                    else
                    {
                    }
                }
            }

            return newList;
        }
    }
}