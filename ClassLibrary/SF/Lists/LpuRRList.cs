using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
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
                _uniqueInstance = new LpuRRList("SF_LpuRR");

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

            var list = List.Where(item => !(item as LpuRR).IsInList || (item as LpuRR) == lpuRR).OrderBy(item => item.Name);

            dt.Rows.Add("0", "Прочие ЛПУ");

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }
        
        public DataTable ToDataTable(User user)
        {
            List<BaseDictionary> list;

            if (user.RoleSF == RolesSF.Администратор)
            {
                list = List.Where(item => !(item as LpuRR).IsInList).OrderBy(item => item.Name).OrderBy(item => (item as LpuRR).RegionRR.Name).ToList();
            }
            else
            {
                UserRightList userRightList = UserRightList.GetUniqueInstance();

                list = List.Where(item => userRightList.IsInList(user, (item as LpuRR).RegionRR) && !(item as LpuRR).IsInList).ToList();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");
            dt.Columns.Add("Регион RR");

            foreach (LpuRR item in list)
                dt.Rows.Add(new object[] { item.ID, item.Name, item.RegionRR.Name });

            dt.Rows.Add(new object[] { 0, "Прочие ЛПУ", "Российская федерация" });

            return dt;
        }
    }
}
