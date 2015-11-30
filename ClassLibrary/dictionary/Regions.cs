using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataLayer;
using ClassLibrary;

namespace ClassLibrary
{
    public class Regions
    {
        private int id;
        private string code;
        private string name;
        private string nameRus;

        public Regions()
        {
            id = 0;
            code = string.Empty;
            name = string.Empty;
            nameRus = string.Empty;
        }

        public Regions(int id)
        {
            this.id = id;

            DataTable dt = new DataTable();
            dt = getRegionInfo();

            if (isHaveRecord(dt))
            {
                code = dt.Rows[0].ItemArray[1].ToString();
                name = dt.Rows[0].ItemArray[2].ToString();
                nameRus = dt.Rows[0].ItemArray[3].ToString();
            }
        }

        private DataTable getRegionInfo()
        {
            sql sql1 = new sql();
            return sql1.GetRecords("exec Region_Select_Info @p1", id);
        }

        private bool isHaveRecord(DataTable dt)
        {
            return dt.Rows.Count > 0;
        }

        public string getCode()
        {
            return code;
        }

        public string getName()
        {
            return name;
        }

        public string getNameRus()
        {
            return nameRus;
        }

        public string save(string code, string name, string nameRus)
        {
            sql sql1 = new sql();
            return sql1.GetRecordsOne("exec region_Insert @p1, @p2, @p3, @p4", id, code, name, nameRus);
        }

        public static DataTable getDataTable()
        {
            sql sql1 = new sql();
            return sql1.GetRecords("exec Region_Select");
        }
    }
}
