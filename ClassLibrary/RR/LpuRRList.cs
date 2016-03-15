using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;

namespace ClassLibrary
{
    public class LpuRRList : BaseList
    {
        private static LpuRRList _uniqueInstance;

        private LpuRRList(string tableName)
            : base(tableName)
        { }

        public static LpuRRList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new LpuRRList("LPU");

            return _uniqueInstance;
        }

        public override DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");

            var list = List.OrderBy(item => item.Name);

            dt.Rows.Add("0", "Прочие ЛПУ");

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }

        public DataTable ToDataTable(LpuRR lpuRR)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");

            var list = List.Select(item => item as LpuRR).ToList();

            list = list.Where(item => (!item.IsInList || item == lpuRR) && item.StatusLPU == StatusLPU.Активен).OrderBy(item => item.Name).ToList();

            dt.Rows.Add("0", "Прочие ЛПУ");

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }
        
        public DataTable ToDataTable(User user)
        {
            List<LpuRR> list = List.Select(item => item as LpuRR).ToList();
            list = list.Where(item => !item.IsInList && item.StatusLPU == StatusLPU.Активен).OrderBy(item => item.Name).OrderBy(item => item.RegionRR.Name).ToList();

            if (user.RoleSF == RolesSF.Пользователь)
            {
                UserRightList userRightList = UserRightList.GetUniqueInstance();
                
                list = list.Where(item => userRightList.IsInList(user, item.RegionRR)).ToList();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("№ ЛПУ-RR");
            dt.Columns.Add("Сокр. название ЛПУ-RR");
            dt.Columns.Add("Полное название ЛПУ-RR");
            dt.Columns.Add("Регион RR");

            foreach (LpuRR item in list)
                dt.Rows.Add(new object[] { item.ID, item.Name, item.FullName, item.RegionRR.Name });

            dt.Rows.Add(new object[] { 0, "Прочие ЛПУ", "Прочие ЛПУ", "Российская федерация" });

            return dt;
        }

        public DataTable ToDataTableWithLpuSF(User user)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("№ ЛПУ-RR", typeof(int));
            dt.Columns.Add("Сокр. название ЛПУ-RR");
            dt.Columns.Add("Полное название ЛПУ-RR");
            dt.Columns.Add("Регион RR");
            dt.Columns.Add("Статус");
            dt.Columns.Add("Сокр. название ЛПУ-SF");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("Город");
            dt.Columns.Add("№ ЛПУ-SF");
            dt.Columns.Add("color");

            LpuList lpuList = new LpuList();

            List<LpuRR> listNew = new List<LpuRR>();
            listNew = List.Select(item => item as LpuRR).ToList();

            if (user.RoleSF == RolesSF.Пользователь)
            {
                UserRightList userRightList = UserRightList.GetUniqueInstance();

                listNew = listNew.Where(item => userRightList.IsInList(user, item.RegionRR)).ToList();
            }
            
            foreach (LpuRR lpuRR in listNew)
            {
                if (lpuRR.ID == 0)
                    continue;

                LPU lpu = lpuList.GetItem(lpuRR);
                                
                string lpuName = string.Empty;
                string realRegionName = string.Empty;
                string cityName = string.Empty;
                string lpuID = string.Empty;

                if (lpu != null)
                {
                    lpuName = lpu.ShortName;
                    realRegionName = lpu.RealRegion.Name;
                    cityName = lpu.City.Name;
                    lpuID = lpu.ID.ToString();
                }

                bool colorWhite = true;

                if (user.RoleSF == RolesSF.Пользователь)
                {
                    colorWhite = IsUserLpu(lpuRR, user);
                }

                dt.Rows.Add(new object[] { lpuRR.ID, lpuRR.Name, lpuRR.FullName, lpuRR.RegionRR.Name, lpuRR.StatusLPU.ToString(), lpuName, realRegionName, cityName, lpuID, colorWhite });
            }

            return dt;
        }

        public DataTable ToDataTable(RegionRR regionRR)
        {
            var list = List.Select(item => (item as LpuRR)).ToList();

            list = list.Where(item => item.RegionRR == regionRR).OrderBy(item => item.Name).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Номер", typeof(int));
            dt.Columns.Add("Сокр. название ЛПУ");
            dt.Columns.Add("Полное название ЛПУ");
            dt.Columns.Add("Статус");

            foreach (var item in list)
                dt.Rows.Add(new object[] { item.ID, item.Name, item.FullName, item.StatusLPU.ToString() });

            return dt;
        }

        private bool IsUserLpu(LpuRR lpuRR, User user)
        {
            UserLpuRRList userLpuRRList = UserLpuRRList.GetUniqueInstance();

            return userLpuRRList.IsInList(lpuRR, user);
        }

        public override BaseDictionary GetItem(int id)
        {
            return ((List.Count == 0) || (List.Where(item => item.ID == id).Count() == 0)) ? null : List.Where(item => item.ID == id).First();
        }

        public void Add(LpuRR lpuRR)
        {
            base.Add(lpuRR);
        }
    }
}
