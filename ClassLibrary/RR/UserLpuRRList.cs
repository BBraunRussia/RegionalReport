using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;

namespace ClassLibrary
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
                _uniqueInstance = new UserLpuRRList("UserLpu");

            return _uniqueInstance;
        }

        public DataTable ToDataTable(User user, RegionRR regionRR, SDiv sdiv)
        {
            var list = List.Select(item => (item as UserLpuRR)).ToList();

            list = list.Where(item => item.User == user && item.LpuRR.RegionRR == regionRR && item.Sdiv == sdiv).OrderBy(item => item.LpuRR.Name).ToList();

            return CreateTable(list);
        }

        private DataTable CreateTable(List<UserLpuRR> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Номер", typeof(int));
            dt.Columns.Add("Сокращенное наименование");
            dt.Columns.Add("Полное наименование");
            dt.Columns.Add("Начало отчётности");
            dt.Columns.Add("Окончание отчётности");
            
            foreach (UserLpuRR item in list)
                dt.Rows.Add(new object[] { item.ID, item.LpuRR.ID, item.LpuRR.Name, item.LpuRR.FullName, item.YearBegin, item.YearEnd });

            return dt;
        }

        public bool IsInList(LpuRR lpuRR, User user)
        {
            var list = List.Select(item => item as UserLpuRR).ToList();
            return list.Exists(item => item.LpuRR == lpuRR && item.User == user && item.YearEnd >= DateTime.Today.Year);
        }

        public bool IsInList(LpuRR lpuRR)
        {
            var list = List.Select(item => item as UserLpuRR).ToList();
            return list.Exists(item => item.LpuRR == lpuRR);
        }

        public DataTable ToDataTableWithSF()
        {
            var list = List.Select(item => item as UserLpuRR).ToList();

            return ToDataTableWithSF(list);
        }

        public DataTable ToDataTableWithSF(SDiv sdiv)
        {
            var list = List.Select(item => item as UserLpuRR).ToList();
            list = list.Where(item => item.Sdiv == sdiv).ToList();

            return ToDataTableWithSF(list);
        }

        private DataTable ToDataTableWithSF(List<UserLpuRR> list)
        {
            list = list.Where(item => item.YearEnd == DateTime.Today.Year && item.LpuRR.StatusLPU == StatusLPU.Активен).ToList();

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

        public void Add(UserLpuRR userLpuRR)
        {
            base.Add(userLpuRR);
        }
    }
}
