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

        public OrganizationList()
        {
            _provider = Provider.GetProvider();
            _list = new List<Organization>();

            LoadFromDataBase();
        }

        public List<LPU> ListLpu { get { return _list.Where(item => (item is LPU) && item.ParentOrganization == null).Select(item => item as LPU).ToList(); } }
        public List<OtherOrganization> ListOther { get { return _list.Where(item => item is OtherOrganization).Select(item => item as OtherOrganization).ToList(); } }

        public static OrganizationList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new OrganizationList();

            return _uniqueInstance;
        }

        private void LoadFromDataBase()
        {
            DataTable dt = _provider.Select("SF_Organization");

            foreach (DataRow row in dt.Rows)
            {
                Organization organization = Organization.CreateItem(row);
                Add(organization);
            }
        }

        public void Reload()
        {
            _list.Clear();

            LoadFromDataBase();
        }

        public Organization GetItem(int id)
        {
            return _list.Where(item => item.ID == id).First();
        }

        internal void Add(Organization organization)
        {
            if (!_list.Exists(item => item == organization))
                _list.Add(organization);
        }

        public void Delete(Organization item)
        {
            _list.Remove(item);

            _provider.Delete("SF_Organization", item.ID);
        }

        public List<Organization> GetChildList(Organization organization)
        {
            var listBranch = GetBranchList(organization);
            var listSubOrganization = GetSubOrganizationList(organization);

            listBranch.AddRange(listSubOrganization);

            return listBranch;
        }

        public List<Organization> GetSubOrganizationList(Organization organization)
        {
            return _list.Where(item => item.ParentOrganization == organization && !(item is LPU)).OrderBy(item => item.ShortName).ToList();
        }

        public List<Organization> GetBranchList(Organization organization)
        {
            return _list.Where(item => item.ParentOrganization == organization && (item is LPU)).OrderBy(item => item.ShortName).ToList();
        }
    }
}
