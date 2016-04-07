using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public class PersSalesReport
    {
        private LpuRR _lpuRR;
        private User _user;
        private RegionRR _regionRR;
        private SDiv _sdiv;
        private User _rd;
        private int _year;
        private double _euro;
        private double _rub;

        public PersSalesReport(DataRow row)
        {
            LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
            int idLpuRR;
            int.TryParse(row[0].ToString(), out idLpuRR);
            _lpuRR = lpuRRList.GetItem(idLpuRR) as LpuRR;

            UserList userList = UserList.GetUniqueInstance();
            int idUser;
            int.TryParse(row[1].ToString(), out idUser);
            _user = userList.GetItem(idUser) as User;

            RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
            int idRegionRR;
            int.TryParse(row[2].ToString(), out idRegionRR);
            _regionRR = regionRRList.GetItem(idRegionRR) as RegionRR;

            int idSDiv;
            int.TryParse(row[3].ToString(), out idSDiv);
            _sdiv = (SDiv)idSDiv;

            int idRD;
            int.TryParse(row[4].ToString(), out idRD);
            _rd = userList.GetItem(idRD) as User;

            int.TryParse(row[5].ToString(), out _year);
            double.TryParse(row[6].ToString(), out _euro);
            double.TryParse(row[7].ToString(), out _rub);
        }

        public int idLPU { get { return _lpuRR.ID; } }
        public string LpuSName { get { return _lpuRR.Name; } }
        public string LpuName { get { return _lpuRR.FullName; } }
        public string UserName { get { return _user.Name; } }
        public string RegionName { get { return _regionRR.Name; } }
        public string SDiv { get { return _sdiv.ToString(); } }
        public string RDName { get { return (_rd == null) ? "нет директора" : _rd.Name; } }
        public double Euro { get { return _euro; } }
        public double Rub { get { return _rub; } }
    }
}
