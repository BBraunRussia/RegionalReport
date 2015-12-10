using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LpuCompetitorsList
    {
        private List<LpuCompetitors> _list;
        private IProvider _provider;

        private static LpuCompetitorsList _uniqueInstance;

        private LpuCompetitorsList(string tableName)
        {
            DataBase.ChangeDataBase(DataLayer.DBNames.Competitors);
            _provider = Provider.GetProvider();
            _list = new List<LpuCompetitors>();

            LoadFromDataBase(tableName);
            DataBase.ChangeDataBase(DataLayer.DBNames.RegionalR_TestSF);
        }

        public static LpuCompetitorsList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new LpuCompetitorsList("SF_LPU");

            return _uniqueInstance;
        }
        
        private void LoadFromDataBase(string tableName)
        {
            DataTable dt = _provider.Select(tableName);

            foreach (DataRow row in dt.Rows)
            {
                LpuCompetitors lpuCompetitors = new LpuCompetitors(row);
                _list.Add(lpuCompetitors);
            }
        }

        public DataTable ToDataTable()
        {
            return (_list.Count == 0) ? null : _list.Select(item => item.GetRow()).CopyToDataTable();
        }

        public LpuCompetitors GetItem(int id)
        {
            return _list.Where(item => item.ID == id).First();
        }
    }
}
