using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class UserLpuRR : BaseDictionary
    {
        private User _user;
        private LpuRR _lpuRR;
        private SDiv _sdiv;
        private int _year1;
        private int _year2;

        public UserLpuRR(DataRow row)
            : base(row)
        {
            int idUser;
            int.TryParse(row[1].ToString(), out idUser);
            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(idUser) as User;

            int idLpuRR;
            int.TryParse(row[2].ToString(), out idLpuRR);
            LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
            _lpuRR = lpuRRList.GetItem(idLpuRR) as LpuRR;

            int idSdiv;
            int.TryParse(row[3].ToString(), out idSdiv);
            _sdiv = (SDiv)idSdiv;

            int.TryParse(row[4].ToString(), out _year1);
            int.TryParse(row[5].ToString(), out _year2);
        }

        public User User { get { return _user; } }
        public LpuRR LpuRR { get { return _lpuRR; } }
        public SDiv Sdiv { get { return _sdiv; } }
        public int YearBegin { get { return _year1; } }
        public int YearEnd { get { return _year2; } }
    }
}
