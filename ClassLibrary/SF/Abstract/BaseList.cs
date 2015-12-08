﻿using System;
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

        public BaseList(string tableName)
        {
            _provider = Provider.GetProvider();
            _list = new List<BaseDictionary>();

            LoadFromDataBase(tableName);
        }

        private void LoadFromDataBase(string tableName)
        {
            DataTable dt = _provider.Select(tableName);

            foreach (DataRow row in dt.Rows)
            {
                BaseDictionary baseDictionary = SFFactory.CreateItem(tableName, row);
                _list.Add(baseDictionary);
            }
        }

        public DataTable ToDataTable()
        {
            return _list.Select(item => item.GetRow()).CopyToDataTable();
        }
    }
}
