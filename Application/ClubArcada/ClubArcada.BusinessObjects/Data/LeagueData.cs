using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.BusinessObjects.Data
{
    public class LeagueData
    {
        public static League GetActiveLeague(Enumerators.eConnectionString connectionString)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.Leagues.SingleOrDefault(u => u.IsActive);
            }
        }
    }
}
