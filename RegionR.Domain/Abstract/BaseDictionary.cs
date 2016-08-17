using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RegionReport.Domain;

namespace ClassLibrary
{
    public abstract class BaseDictionary : InitProvider
    {
        public BaseDictionary(DataRow row)
        {
            CanAdd = true;

            int id;
            int.TryParse(row[0].ToString(), out id);
            ID = id;

            Name = row[1].ToString();
            NameEng = row[2].ToString();
        }

        public BaseDictionary() { }

        public int ID { get; protected set; }
        public string Name { get; set; }
        public string NameEng { get; private set; }
        public bool CanAdd { get; set; }

        public virtual object[] GetRow()
        {
            return new object[] { ID, Name };
        }

        public string GetName(Language lang)
        {
            return (lang == Language.Rus) ? Name : NameEng;
        }
    }
}
