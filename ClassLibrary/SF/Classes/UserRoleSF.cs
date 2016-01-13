using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class UserRoleSF : BaseDictionary
    {
        private User _user;
        private RolesSF _roleSF;

        public UserRoleSF(DataRow row)
            : base(row)
        {
            int idUser;
            int.TryParse(row[0].ToString(), out idUser);

            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(idUser) as User;

            int idRoleSF;
            int.TryParse(row[1].ToString(), out idRoleSF);

            _roleSF = (RolesSF)idRoleSF;
        }

        public User User
        {
            get { return _user; }
        }

        public RolesSF RoleSF
        {
            get { return _roleSF; }
        }

        public override object[] GetRow()
        {
            return new object[] { User.Name, RoleSF.ToString() };
        }
    }
}
