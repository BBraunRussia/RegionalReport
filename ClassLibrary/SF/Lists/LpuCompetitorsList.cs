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
            if (user.RoleSF == RolesSF.Администратор)
                return CreateTable(_list);

            RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
            List<RealRegion> list = realRegionList.ToList(user);

            var list2 = _list.Where(item => IsInList(list, item.RealRegion)).ToList();

            return (list2.Count > 0) ? CreateTable(list2) : CreateTable(_list);
        }

        private DataTable CreateTable(List<LpuCompetitors> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Официальное название");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("ИНН");

            LpuList lpuList = new LpuList();

            var listNew = list.Where(item => !(lpuList.IsInList(item.INN))).OrderBy(item => item.RealRegion.Name).Select(item => item.GetRow()).ToList();

            foreach (var item in listNew)
                dt.Rows.Add(item);

            return dt;
        }

        private bool IsInList(List<RealRegion> list, RealRegion realRegion)
        {
            return list.Exists(item => item == realRegion);
        }

        public LpuCompetitors GetItem(int id)
        {
            return _list.Where(item => item.ID == id).First();
        }
    }
}
