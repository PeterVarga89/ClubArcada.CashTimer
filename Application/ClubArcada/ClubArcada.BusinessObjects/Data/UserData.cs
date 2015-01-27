﻿using ClubArcada.BusinessObjects.DataClasses;
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
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                return app.Users.Where(u =>
                    u.NickName.ToLower().Contains(searchString.ToLower())
                    ||
                    u.FirstName.ToLower().Contains(searchString.ToLower())
                    ||
                    u.LastName.ToLower().Contains(searchString.ToLower())
                    ||
                    (u.FirstName + " " + u.LastName).ToLower().StartsWith(searchString.ToLower())
                    ).ToList();
            }
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

        public static bool IsNickNameExist(eConnectionString connectionString, string nickName)
        {
            using (var app = new PKDBDataContext(connectionString.GetEnumDescription()))
            {
                var userlist = app.Users.Where(u => u.NickName.ToLower() == nickName.ToLower()).ToList();
                return userlist.Count > 0;
            }
        }
    }
}