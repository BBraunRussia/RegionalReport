using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class LPUController
    {
        private LPU _lpu;
        private LPU _parentLPU;

        private bool _isLoad;

        private TreeNode _currentNode;

        private SubRegionList _subRegionList;

        public LPUController(LPU lpu)
        {
            _lpu = lpu;

            if (_lpu.ParentOrganization != null)
                _parentLPU = (_lpu.ParentOrganization as LPU);

            _isLoad = false;

            _subRegionList = SubRegionList.GetUniqueInstance();
        }

        public bool IsLoad
        {
            get { return _isLoad; }
            set { _isLoad = value; }
        }

        public LPU ParentLPU { get { return _parentLPU; } }

        public LPU LPU { get { return _lpu; } }

        public int idTypeLPU { get { return (_lpu.TypeLPU == null) ? 1 : _lpu.TypeLPU.ID; } }
        public int idOwnership { get { return (_lpu.Ownership == null) ? 1 : _lpu.Ownership.ID; } }
        public int idAdmLevel { get { return (_lpu.AdmLevel == null) ? 1 : _lpu.AdmLevel.ID; } }
        public int idMainSpec { get { return (_lpu.MainSpec == null) ? 1 : _lpu.MainSpec.ID; } }
        public int idTypeFin { get { return (_lpu.TypeFin == null) ? 1 : _lpu.TypeFin.ID; } }

        public int idSubRegion
        {
            get
            {
                if (_lpu.SubRegion != null)
                {
                    return _lpu.SubRegion.ID;
                }
                else
                {
                    RegionRR regionRR = (_parentLPU == null) ? _lpu.RealRegion.RegionRR : _parentLPU.RealRegion.RegionRR;
                    return _subRegionList.GetItem(regionRR).ID;
                }
            }
        }

        public string BranchName { get { return (_parentLPU == null) ? string.Empty : _lpu.ShortName; } }
        public string LPUName { get { return (_parentLPU == null) ? _lpu.ShortName : _lpu.ParentOrganization.ShortName; } }

        public int idLpuRR { get { return (_lpu.LpuRR == null) ? 0 : _lpu.LpuRR.ID; } }
        public string RegionRR { get { return (_lpu.LpuRR == null) ? string.Empty : _lpu.LpuRR.RegionRR.Name; } }

        public string INN { get { return (_parentLPU == null) ? _lpu.INN : _parentLPU.INN; } }
    }
}
