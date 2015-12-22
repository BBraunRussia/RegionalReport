using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class RegionCompetitorsList
    {
        private List<RegionCompetitors> _list;
        private IProvider _provider;

        private static RegionCompetitorsList _uniqueInstance;

        private RegionCompetitorsList(string tableName)
        {
            DataBase.ChangeDataBase(DataLayer.DBNames.Competitors);
            _provider = Provider.GetProvider();
            _list = new List<RegionCompetitors>();

            LoadFromDataBase(tableName);
            DataBase.ChangeDataBase(DataLayer.DBNames.RegionalR_TestSF);
        }

        public static RegionCompetitorsList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new RegionCompetitorsList("SF_Region");

            return _uniqueInstance;
        }
        
        private void LoadFromDataBase(string tableName)
        {
            DataTable dt = _provider.Select(tableName);

            foreach (DataRow row in dt.Rows)
            {
                RegionCompetitors regionCompetitors = new RegionCompetitors(row);
                _list.Add(regionCompetitors);
            }
        }

        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");
            
            var list = _list.Select(item => item.GetRow()).ToList();

            foreach (var item in list)
                dt.Rows.Add(item);

            return dt;
        }

        public RegionCompetitors GetItem(int id)
        {
            return (id == 0) ? null : _list.Where(item => item.ID == id).First();
        }
    }
}
