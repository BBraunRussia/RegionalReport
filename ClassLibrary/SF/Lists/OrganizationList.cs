﻿using System;
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

        public List<LPU> ListLpu { get { return _list.Where(item => (item is LPU) && item.ParentOrganization == null).Select(item => item as LPU).ToList(); } }

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
                Add(organization);
            }
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
        }

        public List<Organization> GetChildList(Organization oraganization)
        {
            return _list.Where(item => item.ParentOrganization == oraganization).ToList();
        }
    }
}