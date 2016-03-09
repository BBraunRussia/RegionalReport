using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace ClassLibrary
{
    public abstract class BaseList : InitProvider
    {
        private Dictionary<int, BaseDictionary> _list;
        private string _tableName;

        public BaseList(string tableName)
        {
            _tableName = tableName;

            _list = new Dictionary<int, BaseDictionary>();

            LoadFromDataBase();
        }

        protected List<BaseDictionary> List { get { return _list.Values.ToList(); } }

        private void LoadFromDataBase()
        {
            DataTable dt = _provider.Select(_tableName);

            foreach (DataRow row in dt.Rows)
            {
                BaseDictionary baseDictionary = Factory.CreateItem(_tableName, row);
                if (baseDictionary.CanAdd)
                    Add(baseDictionary);
            }
        }

        public void Reload()
        {
            _list.Clear();

            LoadFromDataBase();
        }

        public virtual DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");

            var list = _list.OrderBy(item => item.Value.Name);
            
            foreach (var item in list)
                dt.Rows.Add(item.Value.GetRow());

            return dt;
        }

        public virtual BaseDictionary GetItem(int id)
        {
            if ((id == 0) || (_list.Count == 0))
                return null;

            return (_list.ContainsKey(id)) ? _list[id] : null;
        }

        public BaseDictionary GetItem(string name)
        {
            var list = _list.Where(item => item.Value.Name == name);
            return (list.Count() == 0) ? null : list.First().Value;
        }

        public string Delete(BaseDictionary item)
        {
            _list.Remove(item.ID);

            return _provider.Delete(_tableName, item.ID);
        }

        protected void Add(BaseDictionary item)
        {
            if (!_list.ContainsKey(item.ID))
                _list.Add(item.ID, item);
        }
    }
}