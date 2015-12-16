using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public abstract class BaseList : IDataTable
    {
        private List<BaseDictionary> _list;
        private IProvider _provider;
        private string _tableName;

        public BaseList(string tableName)
        {
            _tableName = tableName;

            _provider = Provider.GetProvider();
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
                _list.Add(baseDictionary);
            }
        }

        public DataTable ToDataTable()
        {
            return (_list.Count == 0) ? null : _list.OrderBy(item => item.Name).Select(item => item.GetRow()).CopyToDataTable();
        }

        public BaseDictionary GetItem(int id)
        {
            return _list.Where(item => item.ID == id).First();
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