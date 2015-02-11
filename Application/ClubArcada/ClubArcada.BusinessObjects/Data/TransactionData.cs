using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System;

namespace ClubArcada.BusinessObjects.Data
{
    public class TransactionData
    {
        public static void Create(eConnectionString connectionString, Transaction entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.TransactionId = Guid.NewGuid();

            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                app.Transactions.InsertOnSubmit(entity);
                app.SubmitChanges();
            }
        }
    }
}