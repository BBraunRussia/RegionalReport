using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class HistoryList : BaseList
    {
        private static HistoryList _uniqueInstance;

        private HistoryList(string tableName)
            : base(tableName)
        { }

        public static HistoryList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new HistoryList("SF_History");

            return _uniqueInstance;
        }

        public History GetItem(IHistory orgHistory, Action action)
        {
            var list = GetList(orgHistory).Where(item => item.Action == action).ToList();

            return (list.Count == 0) ? null : list.First();
        }
        /*
        public HistoryOrganization GetItem(Organization organization)
        {
            var list = GetList(organization);

            return (list.Count == 0) ? null : list.First();
        }
        */
        internal List<History> GetList(IHistory orgHistory)
        {
            return List.Where(item => (item as History).ID == orgHistory.ID && (item as History).Type == orgHistory.Type).Select(item => item as History).ToList();
        }

        public void Add(History history)
        {
            if (!List.Exists(item => item == history))
                List.Add(history);
        }

        public string GetItemString(IHistory orgHistory, Action action)
        {
            History history = GetItem(orgHistory, action);

            return (history == null) ? string.Empty : history.ToString();
        }
    }
}
