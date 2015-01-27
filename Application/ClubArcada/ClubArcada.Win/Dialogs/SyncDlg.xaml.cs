using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class SyncDlg : Window
    {
        public MainWindow MainWindow { get; set; }

        public SyncDlg(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            InitializeComponent();
            SyncUsers();
            SyncTournaments();
            CheckTournament();
        }

        private bool IsBusy { get { return busyIndicator.IsBusy; } set { busyIndicator.IsBusy = value; } }

        private void SyncUsers()
        {
            IsBusy = true;

            var worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                try
                {
                    var onlineUsers = BusinessObjects.Data.UserData.GetList(eConnectionString.Online);
                    var localUsers = BusinessObjects.Data.UserData.GetList(eConnectionString.Local);

                    var usersToSync = onlineUsers.Where(u => !localUsers.Select(us => us.UserId).Contains(u.UserId)).ToList();
                    BusinessObjects.Data.UserData.Insert(eConnectionString.Local, usersToSync);
                }
                catch (Exception e)
                {
                }
            };

            worker.RunWorkerCompleted += delegate
            {
            };

            worker.RunWorkerAsync();
            tbBusy.Text = "Sync Users...";
        }

        private void SyncTournaments()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                var online = BusinessObjects.Data.TournamentData.GetList(eConnectionString.Online);
                var local = BusinessObjects.Data.TournamentData.GetList(eConnectionString.Local);

                var usersToSync = online.Where(t => !local.Select(ts => ts.TournamentId).Contains(t.TournamentId)).ToList();
                BusinessObjects.Data.TournamentData.Insert(eConnectionString.Local, usersToSync);
            };

            worker.RunWorkerCompleted += delegate
            {
                IsBusy = false;
                this.Close();
            };

            worker.RunWorkerAsync();
            tbBusy.Text = "Sync Tournaments...";
        }

        private void CheckTournament()
        {
            var tour = BusinessObjects.Data.TournamentData.CheckIsExistByDateTime(eConnectionString.Local, DateTime.Now);

            if (tour == null)
            {
                tour = new BusinessObjects.DataClasses.Tournament();
                tour.Date = DateTime.Now;
                tour.DateCreated = DateTime.Now;
                tour.DateDeleted = null;
                tour.IsHidden = true;
                tour.LeagueId = BusinessObjects.Data.LeagueData.GetActiveLeague(eConnectionString.Online).LeagueId;
                tour.Name = "Cash Game Hidden";
                tour.TournamentId = Guid.NewGuid();
                tour.Description = string.Empty;
                tour.GameType = 'C';

                TournamentData.Insert(eConnectionString.Local, tour);
            }
            else
            {
                MainWindow.CashResults = BusinessObjects.Data.CashResultData.GetListByTournamentId(eConnectionString.Local, tour.TournamentId);
            }

            MainWindow.Tournament = tour;
        }
    }
}