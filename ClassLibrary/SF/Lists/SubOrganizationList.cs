using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Models;

namespace ClassLibrary.SF.Lists
{
    public class SubOrganizationList
    {
        private IEnumerable<Organization> _list;
        private Organization _organization;

        public SubOrganizationList(Organization organization)
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            _list = organizationList.GetSubOrganizationList(organization);
            _organization = organization;
        }

        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название подразделения");
            dt.Columns.Add("Тип подразделения");

            dt.Rows.Add(new object[] { _organization.ID, "Администрация", _organization.TypeOrg });

            foreach (var item in _list)
                dt.Rows.Add(new object[] { item.ID, item.ShortName, item.TypeOrg });

            return dt;
        }
    }
}
