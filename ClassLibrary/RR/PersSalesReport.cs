using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public class PersSalesReport : InitProvider
    {
        public DataTable ToDataTable(int year)
        {
            return _provider.DoOther("exec PersSalesReport_Select", year);
        }
    }
}
