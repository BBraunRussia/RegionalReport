using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegionReport.Domain.SF.Lists
{
    public class SubRegionOrganizationList : BaseList
    {
        private static SubRegionOrganizationList _uniqueInstance;

        private SubRegionOrganizationList(string tableName)
            : base(tableName)
        { }

        public static SubRegionOrganizationList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new SubRegionOrganizationList("SF_SubRegionOrganization");

            return _uniqueInstance;
        }
    }
}
