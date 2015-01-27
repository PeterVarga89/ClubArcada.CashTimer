using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubArcada.BusinessObjects.Data
{
    public class TableData
    {
        public static void Insert(eConnectionString connectionString, CashTable entity)
        {
            if (!IsExist(connectionString, entity.CashTableId))
            {
                using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
                {
                    app.CashTables.InsertOnSubmit(entity);
                    app.SubmitChanges();
                }
            }
        }

        public static List<CashTable> GetList(eConnectionString connectionString, Guid tournamentId)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.CashTables.Where(t => t.TournamentId == tournamentId).ToList();
            }
        }

        public static void Close(eConnectionString connectionString, Guid tableId)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                var toclose = app.CashTables.SingleOrDefault(t => t.CashTableId == tableId);

                toclose.DateClosed = DateTime.Now;
                app.SubmitChanges();
            }
        }

        public static bool IsExist(eConnectionString connectionString, Guid tableId)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                var table = app.CashTables.SingleOrDefault(t => t.CashTableId == tableId);

                return table.IsNotNull();
            }
        }
    }
}