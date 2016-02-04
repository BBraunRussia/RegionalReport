using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public abstract class BaseList : InitProvider
    {
        private List<BaseDictionary> _list;
        private string _tableName;

        public BaseList(string tableName)
        {
            _tableName = tableName;

            _list = new List<BaseDictionary>();

            LoadFromDataBase();
        }

        protected List<BaseDictionary> List { get { return _list; } }

        private void LoadFromDataBase()
        {
            DataTable dt = _provider.Select(_tableName);

            foreach (DataRow row in dt.Rows)
            {
                BaseDictionary baseDictionary = SFFactory.CreateItem(_tableName, row);
                if (baseDictionary.CanAdd)
                    _list.Add(baseDictionary);
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

            var list = _list.OrderBy(item => item.Name);
            
            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }

        public virtual BaseDictionary GetItem(int id)
        {
            return ((id == 0) || (_list.Count == 0) || (_list.Where(item => item.ID == id).Count() == 0)) ? null : _list.Where(item => item.ID == id).First();
        }

        public BaseDictionary GetItem(string name)
        {
            var list = _list.Where(item => item.Name == name);
            return (list.Count() == 0) ? null : list.First();
        }

        public void Delete(BaseDictionary item)
        {
            _list.Remove(item);

            _provider.Delete(_tableName, item.ID);
        }
    }
}