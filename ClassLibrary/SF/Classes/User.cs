using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class User : BaseDictionary
    {
        private RolesSF _roleSF;

        public User(DataRow row)
            : base(row)
        {
            int idRole;
            int.TryParse(row[2].ToString(), out idRole);

            _roleSF = (idRole == 1) ? RolesSF.Администратор : RolesSF.Пользователь;
        }

        public RolesSF RoleSF
        {
            get
            {
                UserRoleSFList userRoleSFList = UserRoleSFList.GetUniqueInstance();
                UserRoleSF userRoleSF = userRoleSFList.GetItem(ID) as UserRoleSF;
                if (userRoleSF != null)
                    _roleSF = userRoleSF.RoleSF;

                return _roleSF;
            }
        }
    }
}
