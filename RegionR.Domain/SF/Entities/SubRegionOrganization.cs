using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegionReport.Domain.SF.Entities
{
    public class SubRegionOrganization
    {
        public int OrganizationID { get; set; }
        public int SubRegionID { get; set; }

        public SubRegionOrganization(DataRow row)
        {
            int idOrganization;
            int.TryParse(row[0].ToString(), out idOrganization);
            OrganizationID = idOrganization;

            int idSubRegion;
            int.TryParse(row[1].ToString(), out idSubRegion);
            SubRegionID = idSubRegion;
        }
    }
}
