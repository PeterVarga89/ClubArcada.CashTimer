using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ClubArcada.Win.Dialogs
{
    public partial class SyncDlg : Window
    {
        public SyncDlg()
        {
            InitializeComponent();
            SyncUsers();
            SyncTournaments();
        }

        private bool IsBusy { get { return busyIndicator.IsBusy; } set { busyIndicator.IsBusy = value; } }

        private void SyncUsers()
        {
            IsBusy = true;

            var worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                var onlineUsers = BusinessObjects.Data.UserData.GetList(BusinessObjects.Enumerators.eConnectionString.Online);
                var localUsers = BusinessObjects.Data.UserData.GetList(BusinessObjects.Enumerators.eConnectionString.Local);

                var usersToSync = onlineUsers.Where(u => !localUsers.Select(us => us.UserId).Contains(u.UserId)).ToList();
                BusinessObjects.Data.UserData.Insert(BusinessObjects.Enumerators.eConnectionString.Local, usersToSync);
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
                var online = BusinessObjects.Data.TournamentData.GetList(BusinessObjects.Enumerators.eConnectionString.Online);
                var local = BusinessObjects.Data.TournamentData.GetList(BusinessObjects.Enumerators.eConnectionString.Local);

                var usersToSync = online.Where(t => !local.Select(ts => ts.TournamentId).Contains(t.TournamentId)).ToList();
                BusinessObjects.Data.TournamentData.Insert(BusinessObjects.Enumerators.eConnectionString.Local, usersToSync);
                System.Threading.Thread.Sleep(4000);
            };

            worker.RunWorkerCompleted += delegate
            {
                IsBusy = false;
                this.Close();
            };

            worker.RunWorkerAsync();
            tbBusy.Text = "Sync Tournaments...";
        }
    }
}
