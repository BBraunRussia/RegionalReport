using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF.Import
{
    internal class RelationshipModel
    {
        internal RelationshipModel(DataRow row)
        {
            Parse(row);
        }

        public string PersonNumberSF { get; private set; }
        public string OrganizationNumberSF { get; private set; }
        public bool Deleted { get; private set; }

        private void Parse(DataRow row)
        {
            PersonNumberSF = row["Z_PARTNER2__C"].ToString();
            OrganizationNumberSF = row["Z_PARTNER1__C"].ToString();
            Deleted = Convert.ToBoolean(row["Z_INACTIVE__C"].ToString());
        }
    }
}
