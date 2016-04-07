﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public class PersSalesReport : InitProvider
    {
        private LpuRR _lpuRR;
        private User _user;
        private RegionRR _regionRR;
        private SDiv _sdiv;
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

            int.TryParse(row[4].ToString(), out _year);
            double.TryParse(row[5].ToString(), out _euro);
            double.TryParse(row[6].ToString(), out _rub);
        }

        public int idLPU { get { return _lpuRR.ID; } }
        public string LpuSName { get { return _lpuRR.Name; } }
        public string LpuName { get { return _lpuRR.FullName; } }
        public string UserName { get { return _user.Name; } }
        public string SDiv { get { return _sdiv.ToString(); } }
        public double Euro { get { return _euro; } }
        public double Rub { get { return _rub; } }

        public DataTable ToDataTable(int year)
        {
            return _provider.DoOther("exec PersSalesReport_Select", year);
        }
    }
}
