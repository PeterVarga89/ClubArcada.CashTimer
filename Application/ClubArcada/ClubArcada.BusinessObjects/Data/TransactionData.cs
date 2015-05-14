using System;
using System.Linq;
using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;

namespace ClubArcada.BusinessObjects.Data
{
    public class TransactionData
    {
        public static void Create(Transaction entity)
        {
            Create(eConnectionString.Online, entity);
        }

        private static void Create(eConnectionString connectionString, Transaction entity)
        {
            if (!TransactionData.HasAnyTrasaction(entity.UserId))
            {
                UserData.SetUserRefactoringType(entity.UserId, eAutoReturnState.Full);
            }

            if (entity.Amount < 0)
            {
                entity.Amount2 = entity.Amount;
            }

            entity.DateCreated = DateTime.Now;
            entity.TransactionId = Guid.NewGuid();

            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                app.Transactions.InsertOnSubmit(entity);
                app.SubmitChanges();
            }

            CreateMail(entity, UserData.GetUserBalance(entity.UserId));
        }

        public static double GetBonus(Guid userId)
        {
            using (var app = new PKDBDataContext(eConnectionString.Online.GetEnumDescription()))
            {
                var result = app.Transactions.Where(t => t.UserId == userId && t.TransactionType == (int)eTransactionType.Bonus && !t.DateUsed.HasValue && !t.DatePayed.HasValue).ToList();

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

        public static bool HasAnyTrasaction(Guid userId)
        {
            using (var app = new PKDBDataContext(eConnectionString.Online.GetEnumDescription()))
            {
                var transactions = app.Transactions.Where(t => t.UserId == userId && (eTransactionType)t.TransactionType != eTransactionType.Bonus && (eTransactionType)t.TransactionType != eTransactionType.BonusReturned);

                return transactions.Count() > 0;
            }
        }

        public static void HandleRefactoring(Guid userId, double refactorAmount, Guid loggedInUserId, string description = null)
        {
            if (description == null || description == string.Empty)
            {
                description = "Stiahnuté z výhry";
            }

            if (refactorAmount != 0)
            {
                using (var app = new PKDBDataContext(eConnectionString.Online.GetEnumDescription()))
                {
                    var tranList = app.Transactions.Where(t => t.UserId == userId && (t.TransactionType == (int)eTransactionType.Bar || t.TransactionType == (int)eTransactionType.CashGame || t.TransactionType == (int)eTransactionType.Tournament || t.TransactionType == (int)eTransactionType.NotSet) && !t.DateDeleted.HasValue && !t.DatePayed.HasValue).OrderByDescending(t => t.Amount2).ToList();

                    var availableAmount = refactorAmount;
                    foreach (var t in tranList)
                    {
                        if (availableAmount > 0 && t.Amount2.HasValue)
                        {
                            var originalAmount2 = t.Amount2;

                            if (t.Amount2.Value.ToAbs() > availableAmount.ToAbs())
                            {
                                t.Amount2 = t.Amount2 + availableAmount;
                                Create(new Transaction() { Amount = availableAmount, AttachedTransactionId = t.TransactionId, CratedByApplication = 1, UserId = t.UserId, CratedByUserId = loggedInUserId, TransactionType = (int)t.Type.GetOposite(), Description = description });
                                availableAmount = 0;
                            }
                            else
                            {
                                t.Amount2 = 0;
                                t.DatePayed = DateTime.Now;
                                Create(new Transaction() { Amount = originalAmount2.ToAbs(), AttachedTransactionId = t.TransactionId, CratedByApplication = 1, UserId = t.UserId, CratedByUserId = loggedInUserId, TransactionType = (int)t.Type.GetOposite(), Description = description });
                                availableAmount = availableAmount - originalAmount2.ToAbs();
                            }
                        }
                    }

                    app.SubmitChanges();
                }
            }
        }

        private static void CreateMail(Transaction transaction, double balance)
        {
            var user = UserData.GetById(eConnectionString.Local, transaction.UserId);
            var admin = UserData.GetById(eConnectionString.Local, transaction.CratedByUserId);

            var subject = "";
            var body = "";

            if (transaction.Type == eTransactionType.Bonus || transaction.Type == eTransactionType.BonusReturned)
            {
                subject = Mailer.Constants.MailNewBonusSubject;
                body = string.Format(Mailer.Constants.MailNewBonusBody, user.NickName, user.FirstName, user.LastName, "Cash Game", transaction.Amount, balance);
            }
            else
            {
                subject = transaction.Amount > 0 ? Mailer.Constants.MailNewReplySubject : Mailer.Constants.MailNewBorrowSubject;

                body = string.Format(
                Mailer.Constants.MailNewBorrowBody,
                user.NickName,
                user.FirstName,
                user.LastName,
                "Cash Game",
                transaction.Amount,
                balance,
                admin.FullName);
            }

            Mailer.Mailer.SendMail(subject, body);
        }
    }
}