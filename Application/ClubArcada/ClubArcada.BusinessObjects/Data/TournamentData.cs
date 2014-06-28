using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubArcada.BusinessObjects.Data
{
    public class TournamentData
    {
        public static List<Tournament> GetList(Enumerators.eConnectionString connectionString)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.Tournaments.Where(t => t.GameType == 'C').ToList();
            }
        }

        public static Tournament GetById(Enumerators.eConnectionString connectionString, Guid id)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.Tournaments.SingleOrDefault(u => u.TournamentId == id);
            }
        }

        public static void Insert(Enumerators.eConnectionString connectionString, Tournament entity)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                app.Tournaments.InsertOnSubmit(entity);
                app.SubmitChanges();
            }
        }

        public static void Insert(Enumerators.eConnectionString connectionString, List<Tournament> entityList)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                app.Tournaments.InsertAllOnSubmit(entityList);
                app.SubmitChanges();
            }
        }

        public static Tournament CheckIsExistByDateTime(Enumerators.eConnectionString connectionString, DateTime dateTime)
        {
            var list = GetList(connectionString);

            var found = list.SingleOrDefault(t => t.Date.Date == dateTime.Date || (t.Date.Date.AddDays(1) == dateTime.Date.AddDays(1) && t.Date.Hour < 12));

            return found;
        }
    }
}
