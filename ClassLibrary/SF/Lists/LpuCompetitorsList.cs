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

        public DataTable ToDataTable(User user)
        {
            if ((user.Role == Roles.Администратор) || (user.Role == Roles.Руководство1) || (user.Role == Roles.Руководство2))
                return _list.Select(item => item.GetRow()).CopyToDataTable();

            RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
            List<RegionCompetitors> list = realRegionList.ToList(user);

            var list2 = _list.Where(item => IsInList(list, item.RegionCompetitors)).Select(item => item.GetRow());

            return (list2.Count() == 0) ? null : list2.CopyToDataTable();
        }

        private bool IsInList(List<RegionCompetitors> list, RegionCompetitors regionCompetitors)
        {
            return list.Exists(item => item == regionCompetitors);
        }

        public LpuCompetitors GetItem(int id)
        {
            return _list.Where(item => item.ID == id).First();
        }
    }
}
