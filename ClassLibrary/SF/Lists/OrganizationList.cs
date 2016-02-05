using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class OrganizationList
    {
        private Dictionary<int, Organization> _list;
        private IProvider _provider;
        private static OrganizationList _uniqueInstance;

        public OrganizationList()
        {
            _provider = Provider.GetProvider();
            _list = new Dictionary<int, Organization>();

            LoadFromDataBase();
        }

        public List<LPU> ListLpu { get { return _list.Where(item => (item.Value is LPU) && item.Value.ParentOrganization == null).Select(item => item.Value as LPU).ToList(); } }
        public List<OtherOrganization> ListOther { get { return _list.Where(item => item.Value is OtherOrganization).Select(item => item.Value as OtherOrganization).ToList(); } }

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
            return (_list.ContainsKey(id)) ? _list[id] : null;
        }

        internal void Add(Organization organization)
        {
            if (!_list.ContainsKey(organization.ID))
                _list.Add(organization.ID, organization);
        }

        public void Delete(Organization organization)
        {
            PersonList personList = PersonList.GetUniqueInstance();
            personList.Delete(organization);

            var subOrgList = _list.Where(itemSubOrg => itemSubOrg.Value.ParentOrganization == organization).ToList();
            subOrgList.ForEach(itemSubOrg => itemSubOrg.Value.Delete());

            _list.Remove(organization.ID);

            _provider.Delete("SF_Organization", organization.ID);
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
            return _list.Where(item => item.Value.ParentOrganization == organization && !(item.Value is LPU)).OrderBy(item => item.Value.ShortName).Select(item => item.Value).ToList();
        }

        public List<Organization> GetBranchList(Organization organization)
        {
            return _list.Where(item => item.Value.ParentOrganization == organization && (item.Value is LPU)).OrderBy(item => item.Value.ShortName).Select(item => item.Value).ToList();
        }
    }
}
