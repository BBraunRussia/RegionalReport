﻿using System;
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

        public string INN { get { return _inn; } }
        public string KPP { get { return _kpp; } }

        public RegionCompetitors RegionCompetitors
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
            dt.Columns.Add("id");
            dt.Columns.Add("Официальное название");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("ИНН");

            DataRow row = dt.NewRow();
            row[0] = ID;
            row[1] = Name;
            row[2] = RegionCompetitors.Name;
            row[3] = _inn;

            return row;
        }
    }
}