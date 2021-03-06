﻿using System;
using System.Collections.Generic;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Test.DataSimulation.Models
{
    internal class AppDataBuilder
    {
        public IList<OrgUser> BuildTestUsers()
        {
            var rd = new Random();
            IList<OrgUser> testUsers = new List<OrgUser>();
            for (var i = 1; i < 3000; i++)
            {
                var istr = FormatHelper.GetIntString(i).PadLeft(4, '0');
                var user = new OrgUser
                {
                    Id = ConfigHelper.NewGuid,
                    Code = $"i-t-{istr}",
                    Name = $"User{istr}",
                    Account = $"test{istr}",
                    Password = ConfigHelper.DefaultUserPwd,
                    Role = (int) UserRole.Manager,
                    Sex = rd.Next((int) UserSex.Man, (int) UserSex.Lady),
                    Birthday = DateTime.Now.AddDays(rd.Next(1, 6)),
                    Mobile = $"188{FormatHelper.GetIntString(rd.Next(1, 3000)).PadLeft(4, '0')}" +
                             $"{FormatHelper.GetIntString(rd.Next(1, 3000)).PadLeft(4, '0')}",
                    State = (int) UserState.Normal
                };
                //if (i < 100)
                //{
                //    user.Account = $"test{i}";
                //}
                //if (i < 10)
                //{
                //    user.Account = $"test00{i}";
                //}
                user.Mail = $"{user.Account}@163.com";

                testUsers.Add(user);
            }
            return testUsers;
        }
    }
}