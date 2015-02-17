using ClubArcada.BusinessObjects;
using ClubArcada.BusinessObjects.Data;
using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Win.Other;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class SyncDlg : DialogBase
    {
        public SyncDlg()
        {
            InitializeComponent();
            SyncUsers();
            //SyncTournaments();
            CheckTournament();
        }

        private bool IsBusy
        {
            get { return bi.Visibility == System.Windows.Visibility.Visible; } 
            set 
            {
                bi.Visibility = value ? Visibility.Visible : Visibility.Collapsed; 
            } 
        }

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

                    var usersToSync = onlineUsers.Where(u => !localUsers.Select(us => us.UserId).Contains(u.UserId)).ToList().Clone();
                    BusinessObjects.Data.UserData.Insert(eConnectionString.Local, usersToSync);

                    foreach (var u in onlineUsers)
                    {
                        BusinessObjects.Data.UserData.Update(eConnectionString.Local, u);
                    }
                }
                catch (Exception e)
                {
                }
            };

            worker.RunWorkerCompleted += delegate
            {
                this.Close();
            };

            worker.RunWorkerAsync();
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
        }

        private void CheckTournament()
        {
            //var tour = BusinessObjects.Data.TournamentData.CheckIsExistByDateTime(eConnectionString.Local, DateTime.Now);

            Tournament tour = null;

            if (tour == null)
            {
                tour = new BusinessObjects.DataClasses.Tournament();
                tour.Date = DateTime.Now;
                tour.DateCreated = DateTime.Now;
                tour.DateDeleted = null;
                tour.IsHidden = true;
                tour.LeagueId = BusinessObjects.Data.LeagueData.GetActiveLeague(eConnectionString.Online).LeagueId;
                tour.Name = "Cash Game";
                tour.TournamentId = Guid.NewGuid();
                tour.Description = string.Empty;
                tour.GameType = 'C';

                TournamentData.Insert(eConnectionString.Local, tour);
            }
            else
            {
                App.ParentWindow.CashResults = BusinessObjects.Data.CashResultData.GetListByTournamentId(eConnectionString.Local, tour.TournamentId);
            }

            App.ParentWindow.Tournament = tour;
        }
    }
}