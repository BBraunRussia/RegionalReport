using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public class PersSalesReportList : InitProvider
    {
        private static Dictionary<int, PersSalesReportList> _uniqueInstanceList;
        private List<PersSalesReport> _list;

        static PersSalesReportList()
        {
            _uniqueInstanceList = new Dictionary<int, PersSalesReportList>();
        }

        private PersSalesReportList(int year)
        {
            DataTable dt = _provider.DoOther("exec PersSalesReport_Select", year);

            _list = new List<PersSalesReport>();

            foreach (DataRow row in dt.Rows)
            {
                PersSalesReport persSalesReport = new PersSalesReport(row);
                _list.Add(persSalesReport);
            }
        }

        public static PersSalesReportList GetUniqueInstance(int year)
        {
            if (!_uniqueInstanceList.ContainsKey(year))
                _uniqueInstanceList.Add(year, new PersSalesReportList(year));

            return _uniqueInstanceList[year];
        }

        public List<PersSalesReport> GetList()
        {
            return _list;
        }
    }
}
