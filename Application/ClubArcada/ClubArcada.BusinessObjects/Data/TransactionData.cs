using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System;
using System.Linq;

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

        public static double GetBonus(Guid userId)
        {
            using (var app = new PKDBDataContext(eConnectionString.Online.GetEnumDescription()))
            {
                var result = app.Transactions.Where(t => t.UserId == userId && t.TransactionType == (int)eTransactionType.Bonus && !t.DateUsed.HasValue).ToList();

                if (result.IsNotNull() && result.Count > 0)
                {
                    return result.Sum(t => t.Amount);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}