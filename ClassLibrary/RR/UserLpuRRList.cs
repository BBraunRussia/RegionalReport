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
        private const int ROLE_RP_ID = 5;
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
            dt.Columns.Add("№ ЛПУ-RR", typeof(int));
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

        public bool IsInList(LpuRR lpuRR, int year)
        {
            var list = List.Select(item => item as UserLpuRR).ToList();
            return list.Exists(item => item.LpuRR == lpuRR && item.YearEnd == year);
        }

        public DataTable ToDataTableWithSF(User user)
        {
            var list = List.Select(item => item as UserLpuRR).ToList();

            if (user.RoleSF == RolesSF.Пользователь)
            {
                UserRightList userRightList = UserRightList.GetUniqueInstance();
                
                list = list.Where(item => userRightList.IsInList(user, item.LpuRR.RegionRR)).ToList();

                RoleList roleList = RoleList.GetUniqueInstance();
                Role role = roleList.GetItem(ROLE_RP_ID) as Role;

                if (user.Role == role)
                    list = list.Where(item => item.User == user).ToList();
            }

            //list = list.Where(item => item.YearEnd == DateTime.Today.Year).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("№ ЛПУ-RR", typeof(int));
            dt.Columns.Add("ФИО РП");
            dt.Columns.Add("Дивизион");
            dt.Columns.Add("Начало отчётности");
            dt.Columns.Add("Окончание отчётности");
            dt.Columns.Add("Сокр. название ЛПУ-RR");
            dt.Columns.Add("Полное название ЛПУ-RR");
            dt.Columns.Add("Регион RR");
            dt.Columns.Add("Статус");
            dt.Columns.Add("Сокр. название ЛПУ-SF");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("Город");
            dt.Columns.Add("№ ЛПУ-SF");

            LpuList lpuList = new LpuList();

            foreach (UserLpuRR item in list)
            {
                if ((item.LpuRR == null) || (item.User == null))
                    continue;

                string lpuSFID = string.Empty;
                string lpuSFName = string.Empty;
                string lpuSFRealRegion = string.Empty;
                string lpuSFCity = string.Empty;
                
                LPU lpu = lpuList.GetItem(item.LpuRR);

                if (lpu != null)
                {
                    lpuSFID = lpu.ID.ToString();
                    lpuSFName = lpu.ShortName;
                    lpuSFRealRegion = lpu.RealRegion.Name;
                    lpuSFCity = lpu.City.Name;
                }

                dt.Rows.Add(new object[] { item.LpuRR.ID, item.User.Name, item.Sdiv.ToString(), item.YearBegin, item.YearEnd, item.LpuRR.Name, item.LpuRR.FullName,
                    item.LpuRR.RegionRR.Name, item.LpuRR.StatusLPU.ToString(), lpuSFName, lpuSFRealRegion, lpuSFCity, lpuSFID });
            }

            return dt;
        }

        public void Add(UserLpuRR userLpuRR)
        {
            base.Add(userLpuRR);
        }
    }
}
