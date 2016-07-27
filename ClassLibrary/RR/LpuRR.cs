using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;
using ClassLibrary.SF.Lists;

namespace ClassLibrary
{
    public class LpuRR : BaseDictionary
    {
        private RegionRR _regionRR;
        private string _fullName;
        private StatusLPU _statusLPU;

        public LpuRR(DataRow row)
            : base(row)
        {
            int idRegionRR;
            int.TryParse(row[2].ToString(), out idRegionRR);
            RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
            _regionRR = regionRRList.GetItem(idRegionRR) as RegionRR;

            _fullName = row[3].ToString();

            int idStatusLPU;
            int.TryParse(row[4].ToString(), out idStatusLPU);
            _statusLPU = (StatusLPU) idStatusLPU;
        }

        public LpuRR(RegionRR regionRR)
        {
            _regionRR = regionRR;
            Name = string.Empty;
            FullName = string.Empty;
            _statusLPU = StatusLPU.Активен;
        }

        public RegionRR RegionRR { get { return _regionRR; } }
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }
        public StatusLPU StatusLPU
        {
            get { return _statusLPU; }
            set { _statusLPU = value; }
        }

        public bool IsInList
        {
            get
            {
                LpuList lpuList = new LpuList();
                return lpuList.GetItem(this) != null;
            }
        }

        public void Save()
        {
            if (ID == 0)
            {
                int id;
                int.TryParse(_provider.Insert("LPU", Name, FullName, _regionRR.ID, StatusLPU), out id);

                ID = id;
            }
            else
                _provider.Update("LPU", ID, Name, FullName, StatusLPU);

            LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
            lpuRRList.Add(this);
        }
    }
}
