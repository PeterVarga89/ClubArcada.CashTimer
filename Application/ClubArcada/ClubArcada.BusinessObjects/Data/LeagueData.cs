using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System.Linq;

namespace ClubArcada.BusinessObjects.Data
{
    public class LeagueData
    {
        public static League GetActiveLeague(eConnectionString connectionString)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.Leagues.SingleOrDefault(u => u.IsActive);
            }
        }
    }
}