using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class UserLpuRR : BaseDictionary
    {
        private int _idUser;
        private int _idLpuRR;

        public UserLpuRR(DataRow row)
            : base(row)
        {
            int.TryParse(row[1].ToString(), out _idUser);
            int.TryParse(row[2].ToString(), out _idLpuRR);
        }

        public User User
        {
            get
            {
                UserList userList = UserList.GetUniqueInstance();
                return userList.GetItem(_idUser) as User;
            }
        }

        public LpuRR LpuRR
        {
            get
            {
                LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
                return lpuRRList.GetItem(_idLpuRR) as LpuRR;
            }
        }
    }
}
