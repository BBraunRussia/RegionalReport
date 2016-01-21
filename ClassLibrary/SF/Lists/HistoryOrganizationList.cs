using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class HistoryOrganizationList : BaseList
    {
        private static HistoryOrganizationList _uniqueInstance;

        private HistoryOrganizationList(string tableName)
            : base(tableName)
        { }

        public static HistoryOrganizationList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new HistoryOrganizationList("SF_HistoryOrganization");

            return _uniqueInstance;
        }

        public HistoryOrganization GetItem(Organization organization, Action action)
        {
            var list = GetList(organization).Where(item => item.Action == action).ToList();

            return (list.Count == 0) ? null : list.First();
        }
        /*
        public HistoryOrganization GetItem(Organization organization)
        {
            var list = GetList(organization);

            return (list.Count == 0) ? null : list.First();
        }
        */
        internal List<HistoryOrganization> GetList(Organization organization)
        {
            return List.Where(item => (item as HistoryOrganization).Organization.ID == organization.ID).Select(item => item as HistoryOrganization).ToList();
        }

        public void Add(HistoryOrganization historyOrganization)
        {
            if (!List.Exists(item => item == historyOrganization))
                List.Add(historyOrganization);
        }
    }
}
