using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class OrganizationList
    {
        private List<Organization> _list;
        private IProvider _provider;
        private static OrganizationList _uniqueInstance;

        public OrganizationList(string tableName)
        {
            _provider = Provider.GetProvider();
            _list = new List<Organization>();

            LoadFromDataBase(tableName);
        }

        public List<LPU> List { get { return _list.Where(item => item.TypeOrg == TypeOrg.ЛПУ).Select(item => item as LPU).ToList(); } }

        public static OrganizationList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new OrganizationList("SF_Organization");

            return _uniqueInstance;
        }

        private void LoadFromDataBase(string tableName)
        {
            DataTable dt = _provider.Select(tableName);

            foreach (DataRow row in dt.Rows)
            {
                Organization organization = Organization.CreateItem(row);
                _list.Add(organization);
            }
        }

        public Organization GetItem(int id)
        {
            return _list.Where(item => item.ID == id).First();
        }
    }
}
