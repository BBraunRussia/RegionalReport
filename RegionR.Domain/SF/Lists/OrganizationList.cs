using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using ClassLibrary.SF.Entities;
using System.Threading.Tasks;

namespace ClassLibrary.SF.Lists
{
    public class OrganizationList : IEnumerable
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

        public Dictionary<int, Organization> List { get { return _list; } }
        public IEnumerable<LPU> ListLpu { get { return _list.Where(item => (item.Value is LPU) && item.Value.ParentOrganization == null).Select(item => item.Value as LPU).ToList(); } }
        public IEnumerable<LPU> ListBranch { get { return _list.Where(item => (item.Value is LPU) && item.Value.ParentOrganization != null).Select(item => item.Value as LPU).ToList(); } }
        public IEnumerable<Organization> ListOther { get { return _list.Where(item => !(item.Value is LPU) && item.Value.ParentOrganization == null).Select(org => org.Value); } }

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

        private async Task LoadFromDataBaseAsync()
        {
            DataTable dt = _provider.Select("SF_Organization");

            foreach (DataRow row in dt.Rows)
            {
                Organization organization = await Organization.CreateItemAsync(row);
                Add(organization);
            }
        }

        internal void Add(Organization organization)
        {
            if (!_list.ContainsKey(organization.ID))
                _list.Add(organization.ID, organization);
        }

        public async Task Reload()
        {
            _list.Clear();

            await LoadFromDataBaseAsync();
        }

        public Organization GetFirst()
        {
            return _list.First().Value;
        }

        public Organization GetItem(int id)
        {
            return (_list.ContainsKey(id)) ? _list[id] : null;
        }
        
        public void Delete(Organization organization)
        {
            PersonList.GetUniqueInstance().Delete(organization);

            var subOrgList = _list.Where(itemSubOrg => itemSubOrg.Value.ParentOrganization == organization).ToList();
            subOrgList.ForEach(itemSubOrg => itemSubOrg.Value.Delete());

            _list.Remove(organization.ID);

            _provider.Delete("SF_Organization", organization.ID);
        }

        public IEnumerable<Organization> GetChildList(Organization organization)
        {
            var listBranch = GetBranchList(organization);
            var listSubOrganization = GetSubOrganizationList(organization);

            listBranch.AddRange(listSubOrganization);

            return listBranch;
        }

        public IEnumerable<Organization> GetSubOrganizationList(Organization organization)
        {
            return _list.Where(item => item.Value.ParentOrganization == organization && !(item.Value is LPU)).OrderBy(item => item.Value.ShortName).Select(item => item.Value).ToList();
        }

        public List<Organization> GetBranchList(Organization organization)
        {
            return _list.Where(item => item.Value.ParentOrganization == organization && (item.Value is LPU)).OrderBy(item => item.Value.ShortName).Select(item => item.Value).ToList();
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Organization GetItem(string numberSF)
        {
            var organizationList = _list.Where(item => item.Value.NumberSF == numberSF).ToList();

            return (organizationList.Count == 0) ? null : organizationList.First().Value;
        }
    }
}
