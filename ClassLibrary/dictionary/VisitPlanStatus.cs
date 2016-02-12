using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataLayer;

namespace ClassLibrary
{
    public class VisitPlanStatus
    {
        public static DataTable getDataTable()
        {
            Sql sql1 = new Sql();

            return sql1.GetRecords("exec SelVPStatus");
        }
    }
}
