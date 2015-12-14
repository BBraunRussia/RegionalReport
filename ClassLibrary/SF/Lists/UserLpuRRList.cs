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

            dt.Rows.Add(new object[] { 0, "Прочие ЛПУ", "Российская федерация" });

            return dt;
        }
    }
}
