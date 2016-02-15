using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class UserLpuRRList : BaseList
    {
        private static UserLpuRRList _uniqueInstance;

        private UserLpuRRList(string tableName)
            : base(tableName)
        { }

        public static UserLpuRRList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new UserLpuRRList("SF_UserLpuRR");

            return _uniqueInstance;
        }

        public DataTable ToDataTable(User user)
        {
            var list = List.Where(item => (item as UserLpuRR).User == user && !(item as UserLpuRR).LpuRR.IsInList);

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");
            dt.Columns.Add("Регион RR");

            foreach (UserLpuRR item in list)
                dt.Rows.Add(new object[] { item.ID, item.LpuRR.Name, item.LpuRR.RegionRR.Name });

            return dt;
        }

        public bool IsInList(LpuRR lpuRR, User user)
        {
            var list = List.Select(item => item as UserLpuRR).ToList();
            return list.Exists(item => item.LpuRR == lpuRR && item.User == user && item.YearEnd >= DateTime.Today.Year);
        }

        public DataTable ToDataTableWithSF()
        {
            var list = List.Select(item => item as UserLpuRR).ToList();

            return ToDataTableWithSF(list);
        }

        public DataTable ToDataTableWithSF(SDiv sdiv)
        {
            var list = List.Where(item => (item as UserLpuRR).Sdiv == sdiv).Select(item => item as UserLpuRR).ToList();

            return ToDataTableWithSF(list);
        }

        private DataTable ToDataTableWithSF(List<UserLpuRR> list)
        {
            list = list.Where(item => item.YearEnd == DateTime.Today.Year).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("№ ЛПУ-RR");
            dt.Columns.Add("ФИО рег. преда");
            dt.Columns.Add("Полное название ЛПУ-RR");
            dt.Columns.Add("Регион RR");
            dt.Columns.Add("Сокр. название ЛПУ-SF");
            dt.Columns.Add("Город");
            dt.Columns.Add("№ ЛПУ-SF");

            LpuList lpuList = new LpuList();

            foreach (UserLpuRR item in list)
            {
                if (item.LpuRR == null)
                    continue;

                string lpuSFID = string.Empty;
                string lpuSFName = string.Empty;
                string lpuSFCity = string.Empty;

                LPU lpu = lpuList.GetItem(item.LpuRR);

                if ((lpu != null) && (lpu.ParentOrganization == null))
                {
                    lpuSFID = lpu.ID.ToString();
                    lpuSFName = lpu.ShortName;
                    lpuSFCity = lpu.City.Name;
                }

                dt.Rows.Add(new object[] { item.LpuRR.ID, item.User.Name, item.LpuRR.FullName, item.LpuRR.RegionRR.Name, lpuSFName, lpuSFCity, lpuSFID });
            }

            return dt;
        }
    }
}
