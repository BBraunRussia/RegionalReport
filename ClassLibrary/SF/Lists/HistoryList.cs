using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class HistoryList : InitProvider
    {
        private List<History> _list;
        private string _tableName;

        private static HistoryList _uniqueInstance;

        private HistoryList(string tableName)
        {
            _tableName = tableName;

            _list = new List<History>();

            LoadFromDataBase();
        }

        private void LoadFromDataBase()
        {
            DataTable dt = _provider.Select(_tableName);

            foreach (DataRow row in dt.Rows)
            {
                History history = new History(row);

                if (history.CanAdd)
                    Add(history);
            }
        }

        public void Reload()
        {
            _list.Clear();

            LoadFromDataBase();
        }

        public static HistoryList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new HistoryList("SF_History");

            return _uniqueInstance;
        }

        public History GetItem(IHistory orgHistory, HistoryAction action)
        {
            var list = GetList(orgHistory).Where(item => item.Action == action).ToList();

            return (list.Count == 0) ? null : list.First();
        }
        
        internal List<History> GetList(IHistory orgHistory)
        {
            return _list.Where(item => (item as History).ID == orgHistory.ID && (item as History).Type == orgHistory.Type).Select(item => item as History).ToList();
        }

        public void Add(History history)
        {
            if (!_list.Exists(item => item == history))
                _list.Add(history);
        }

        public string GetItemString(IHistory orgHistory, HistoryAction action)
        {
            History history = GetItem(orgHistory, action);

            return (history == null) ? string.Empty : history.ToString();
        }
    }
}
