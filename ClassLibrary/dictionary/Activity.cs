using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataLayer;

namespace ClassLibrary
{
    public class Activity
    {
        public static DataTable getDataTable()
        {
            sql sql1 = new sql();

            return sql1.GetRecords("exec SelActivity");
        }
    }
}
