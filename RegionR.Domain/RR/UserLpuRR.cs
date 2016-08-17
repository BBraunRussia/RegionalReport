using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RegionReport.Domain;

namespace ClassLibrary
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

        public UserLpuRR(User user, LpuRR lpuRR, SDiv sdiv)
        {
            _user = user;
            _lpuRR = lpuRR;
            _sdiv = sdiv;
            Name = string.Empty;
            _year1 = DateTime.Today.Year;
            _year2 = DateTime.Today.Year;
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }
        public LpuRR LpuRR { get { return _lpuRR; } }
        public SDiv Sdiv { get { return _sdiv; } }
        public int YearBegin
        {
            get { return _year1; }
            set { _year1 = value; }
        }
        public int YearEnd
        {
            get { return _year2; }
            set { _year2 = value; }
        }

        public string Save(User userLogged)
        {
            if (ID == 0)
            {
                int idUserLpu;
                int.TryParse(_provider.Insert("UserLPU", ID, _user.ID, _lpuRR.ID, _sdiv, userLogged.ID), out idUserLpu);
                ID = idUserLpu;

                UserLpuRRList userLpuRRList = UserLpuRRList.GetUniqueInstance();
                userLpuRRList.Add(this);

                return "1";
            }
            else
            {
                return _provider.Update("UserLPU", ID, _user.ID, YearBegin, YearEnd);
            }
        }
    }
}
