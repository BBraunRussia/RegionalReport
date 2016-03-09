using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary;

namespace RegionR
{
    public static class UserLogged
    {
        public static User Get()
        {
            UserList userList = UserList.GetUniqueInstance();
            return userList.GetItem(globalData.UserID) as User;
        }
    }
}
