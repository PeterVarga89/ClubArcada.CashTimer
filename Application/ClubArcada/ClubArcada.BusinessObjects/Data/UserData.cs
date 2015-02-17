using ClubArcada.BusinessObjects.DataClasses;
using ClubArcada.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubArcada.BusinessObjects.Data
{
    public class UserData
    {
        public static List<User> GetList(eConnectionString connectionString)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.Users.ToList();
            }
        }

        public static List<User> GetListBySearchString(eConnectionString connectionString, string searchString)
        {
            var list = GetList(connectionString).Where(u => !u.DateDeleted.HasValue).ToList();

            var result = list.Where(u =>
                    u.NickName.ToLower().RemoveDiacritics().StartsWith(searchString.ToLower())
                    ||
                    u.FirstName.ToLower().RemoveDiacritics().StartsWith(searchString.ToLower())
                    ||
                    u.LastName.ToLower().RemoveDiacritics().StartsWith(searchString.ToLower())
                    ||
                    (u.FirstName + " " + u.LastName).ToLower().RemoveDiacritics().StartsWith(searchString.ToLower())
                    ).OrderBy(u => u.NickName).ToList();

            return result;
        }

        public static User GetById(eConnectionString connectionString, Guid id)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.Users.SingleOrDefault(u => u.UserId == id);
            }
        }

        public static void Insert(eConnectionString connectionString, User entity)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                app.Users.InsertOnSubmit(entity);
                app.SubmitChanges();
            }
        }

        public static void Insert(eConnectionString connectionString, List<User> entityList)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                app.Users.InsertAllOnSubmit(entityList);
                app.SubmitChanges();
            }
        }

        public static void Update(eConnectionString connectionString, User entity)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                var user = app.Users.SingleOrDefault(u => u.UserId == entity.UserId);

                user.DateDeleted = entity.DateDeleted;
                user.NickName = entity.NickName;
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.PhoneNumber = entity.PhoneNumber;
                user.Email = entity.Email;
                user.IsAutoReturn = entity.IsAutoReturn;
                app.SubmitChanges();
            }
        }

        public static bool IsNickNameExist(eConnectionString connectionString, string nickName)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                var userlist = app.Users.Where(u => u.NickName.ToLower() == nickName.ToLower()).ToList();
                return userlist.Count > 0;
            }
        }

        public static double GetUserBalance(Guid userId)
        {
            using (var app = new PKDBDataContext(eConnectionString.Online.GetEnumDescription()))
            {
                var re = app.GetUserBalance(userId);
                if (re.IsNull())
                    return 0;

                var res = app.GetUserBalance(userId).SingleOrDefault();

                if (res.IsNull())
                    return 0;

                var result = app.GetUserBalance(userId).SingleOrDefault().Column1;
                return result.HasValue ? result.Value : 0;
            }
        }

        public static User Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return (User)null;

            using (var app = new PKDBDataContext(eConnectionString.Online.GetEnumDescription()))
            {
                var user = app.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
                return user;
            }
        }
    }
}