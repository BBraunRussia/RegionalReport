using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class CityList : BaseList
    {
        private static CityList _uniqueInstance;

        private CityList(string tableName)
            : base(tableName)
        { }

        public static CityList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new CityList("SF_City");

            return _uniqueInstance;
        }

        public DataTable ToDataTable(RealRegion realRegion)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");
            dt.Columns.Add("Телефонный код");

            List<City> list = List.Where(item => (item as City).RealRegion == realRegion).Select(item => item as City).ToList();

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }

        public bool IsInList(City city)
        {
            return List.Where(item => item.Name == city.Name && (item as City).RealRegion == city.RealRegion).Count() > 0;
        }

        internal void Add(City city)
        {
            if (!List.Exists(item => item.ID == city.ID))
                List.Add(city);
        }
    }
}
