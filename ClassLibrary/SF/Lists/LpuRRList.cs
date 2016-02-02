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

        public DataTable ToDataTable(User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();
            
            var list = List.Where(item => userRightList.IsInList(user, (item as LpuRR).RegionRR) && !(item as LpuRR).IsInList);

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
