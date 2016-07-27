using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Models;

namespace ClassLibrary
{
    public class User : BaseDictionary
    {
        private RolesSF _roleSF;
        private Role _role;

        public User(DataRow row)
            : base(row)
        {
            int idRole;
            int.TryParse(row[2].ToString(), out idRole);
            RoleList roleList = RoleList.GetUniqueInstance();
            _role = roleList.GetItem(idRole) as Role;

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
        
        public Role Role { get { return _role; } }
    }
}
