using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubArcada.BusinessObjects.Data
{
    public class CashResultData
    {
        public static List<CashResult> GetListByTournamentId(eConnectionString connectionString, Guid tournamentId)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.CashResults.Where(t => t.TournamentId == tournamentId).ToList();
            }
        }

        public static List<CashResult> GetListByTableId(eConnectionString connectionString, Guid tableId)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.CashResults.Where(t => t.CashTableId == tableId).ToList();
            }
        }

        public static void InsertOrUpdate(eConnectionString connectionString, CashResult entity)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                var loaded = app.CashResults.SingleOrDefault(c => c.CashResultId == entity.CashResultId);

                if (loaded == null)
                {
                    app.CashResults.InsertOnSubmit(entity);
                    app.SubmitChanges();
                }
                else
                {
                    //loaded.
                }
            }
        }

        public static void InsertOrUpdate(eConnectionString connectionString, CashIn entity)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                var loaded = app.CashIns.SingleOrDefault(c => c.CashInId == entity.CashInId);

                if (loaded == null)
                {
                    entity.DateCreated = DateTime.Now;

                    app.CashIns.InsertOnSubmit(entity);
                    app.SubmitChanges();
                }
                else
                {
                    //loaded.
                }
            }
        }
    }
}