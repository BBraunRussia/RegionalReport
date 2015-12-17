using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class RealRegion : BaseDictionary
    {
        private RegionRR _regionRR;
        private RegionCompetitors _regionCompetitors;

        public RealRegion(DataRow row)
            : base(row)
        {
            int idRegionRR;
            int.TryParse(row[2].ToString(), out idRegionRR);
            RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
            _regionRR = regionRRList.GetItem(idRegionRR) as RegionRR;

            int idRegionCompetitors;
            int.TryParse(row[3].ToString(), out idRegionCompetitors);
            RegionCompetitorsList regionCompetitorsList = RegionCompetitorsList.GetUniqueInstance();
            _regionCompetitors = regionCompetitorsList.GetItem(idRegionCompetitors) as RegionCompetitors;
        }

        public RegionRR RegionRR { get { return _regionRR; } }
        public RegionCompetitors RegionCompetitors { get { return _regionCompetitors; } }
    }
}
