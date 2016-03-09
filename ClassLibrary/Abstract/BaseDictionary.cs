using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public abstract class BaseDictionary : InitProvider
    {
        private int _id;
        private string _name;

        private bool _canAdd;
        
        public BaseDictionary(DataRow row)
        {
            _canAdd = true;
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

        public virtual object[] GetRow()
        {
            return new object[] { _id, _name };
        }

        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; }
        }
    }
}
