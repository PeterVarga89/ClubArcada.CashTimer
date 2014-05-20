using ClubArcada.BusinessObjects.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.Win
{
    internal static class Session
    {
        public static User LoggedInUser { get; set; }
        public static Tournament CurrentTournament { get; set; }
    }
}
