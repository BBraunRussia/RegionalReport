using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class User : BaseDictionary
    {
        private int _idRole;

        public User(DataRow row)
            : base(row)
        {
            int.TryParse(row[2].ToString(), out _idRole);
        }

        public Role Role
        {
            get
            {
                RoleList roleList = RoleList.GetUniqueInstance();
                return roleList.GetItem(_idRole) as Role;
            }
        }
    }
}
