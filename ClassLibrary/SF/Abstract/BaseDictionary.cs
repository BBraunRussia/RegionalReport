using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public abstract class BaseDictionary
    {
        private int _id;
        private string _name;

        public BaseDictionary(DataRow row)
        {
            int.TryParse(row[0].ToString(), out _id);
            _name = row[1].ToString();
        }

        public BaseDictionary() { }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int ID
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual DataRow GetRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            DataRow row = dt.NewRow();
            row[0] = _id;
            row[1] = _name;

            return row;
        }
    }
}
