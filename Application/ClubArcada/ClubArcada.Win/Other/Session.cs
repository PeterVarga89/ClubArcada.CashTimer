using ClubArcada.BusinessObjects.DataClasses;

namespace ClubArcada.Win
{
    internal static class Session
    {
        public static User LoggedInUser { get; set; }

        public static Tournament CurrentTournament { get; set; }
    }
}