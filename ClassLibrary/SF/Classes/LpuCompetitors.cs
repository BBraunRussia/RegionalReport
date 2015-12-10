using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LpuCompetitors : BaseDictionary
    {
        private string _inn;
        private int _idRegionCompetitors;
        private string _kpp;

        public LpuCompetitors(DataRow row)
            : base(row)
        {
            _inn = row[2].ToString();
            int.TryParse(row[3].ToString(), out _idRegionCompetitors);
            _kpp = row[4].ToString();
        }

        private RegionCompetitors RegionCompetitors
        {
            get
            {
                RegionCompetitorsList regionCompetitorsList = RegionCompetitorsList.GetUniqueInstance();
                return regionCompetitorsList.GetItem(_idRegionCompetitors) as RegionCompetitors;
            }
        }

        public override DataRow GetRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Официальное название");
            dt.Columns.Add("Регион");
            dt.Columns.Add("ИНН");

            DataRow row = dt.NewRow();
            row[0] = Name;
            row[1] = RegionCompetitors.Name;
            row[2] = _inn;

            return row;
        }
    }
}
