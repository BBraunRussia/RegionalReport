using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Interfaces;

namespace ClassLibrary.SF.Models
{
    public class LPU : Organization, IAvitum
    {
        private int _bedsTotal;
        private int _bedsIC;
        private int _bedsSurgical;
        private int _operating;
        
        public LPU(DataRow row)
            : base(row)
        {
            int idTypeLPU;
            int.TryParse(row["TypeLPU_id"].ToString(), out idTypeLPU);
            TypeLPU = TypeLPUList.GetUniqueInstance().GetItem(idTypeLPU) as TypeLPU;

            int idOwnership;
            int.TryParse(row["Ownership_id"].ToString(), out idOwnership);
            Ownership = OwnershipList.GetUniqueInstance().GetItem(idOwnership) as Ownership;

            int idAdmLevel;
            int.TryParse(row["AdmLevel_id"].ToString(), out idAdmLevel);
            AdmLevel = AdmLevelList.GetUniqueInstance().GetItem(idAdmLevel) as AdmLevel;
                        
            int idLpuRR;
            int.TryParse(row["lpuRR_id"].ToString(), out idLpuRR);
            LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
            LpuRR = lpuRRList.GetItem(idLpuRR) as LpuRR;

            int.TryParse(row["Organization_bedsTotal"].ToString(), out _bedsTotal);
            int.TryParse(row["Organization_bedsIC"].ToString(), out _bedsIC);
            int.TryParse(row["Organization_Surgical"].ToString(), out _bedsSurgical);
            int.TryParse(row["Organization_Operating"].ToString(), out _operating);
            
            int idSubRegion;
            int.TryParse(row["subregion_id"].ToString(), out idSubRegion);
            SubRegionList subRegionList = SubRegionList.GetUniqueInstance();
            SubRegion = subRegionList.GetItem(idSubRegion) as SubRegion;

            int idTypeFin;
            int.TryParse(row["typeFin_id"].ToString(), out idTypeFin);
            TypeFinList typeFinList = TypeFinList.GetUniqueInstance();
            TypeFin = typeFinList.GetItem(idTypeFin) as TypeFin;

            int idLpuRR2;
            int.TryParse(row["lpuRR2_id"].ToString(), out idLpuRR2);
            LpuRR2 = lpuRRList.GetItem(idLpuRR2) as LpuRR;
        }

        public LPU(TypeOrg typeOrg)
            : base(typeOrg)
        { }
        
        public LpuRR LpuRR { get; set; }
        public LpuRR LpuRR2 { get; set; }
        public SubRegion SubRegion { get; set; }
        public TypeFin TypeFin { get; set; }

        public TypeLPU TypeLPU { get; set; }
        public AdmLevel AdmLevel { get; set; }
        public Ownership Ownership { get; set; }
        
        public string BedsTotal
        {
            get { return (_bedsTotal == 0) ? string.Empty : _bedsTotal.ToString(); }
            set { int.TryParse(value, out _bedsTotal); }
        }
        public string BedsIC
        {
            get { return (_bedsIC == 0) ? string.Empty : _bedsIC.ToString(); }
            set { int.TryParse(value, out _bedsIC); }
        }
        public string BedsSurgical
        {
            get { return (_bedsSurgical == 0) ? string.Empty : _bedsSurgical.ToString(); }
            set { int.TryParse(value, out _bedsSurgical); }
        }
        public string Operating
        {
            get { return (_operating == 0) ? string.Empty : _operating.ToString(); }
            set { int.TryParse(value, out _operating); }
        }
        
        public override object[] GetRow()
        {
            string typeOrgName = ((TypeOrg == TypeOrg.ЛПУ) && (ParentOrganization != null)) ? "Филиал ЛПУ" : TypeOrg.ToString();

            return new object[] { ID, CrmID, ShortName, typeOrgName, (ParentOrganization == null) ? INN : (ParentOrganization as LPU).INN,
                (RealRegion == null) ? string.Empty : RealRegion.Name, (City == null) ? string.Empty : City.Name,
                (LpuRR == null) ? "Прочие ЛПУ" : LpuRR.Name, ((LpuRR2 == null) || (LpuRR2.ID == 0)) ? String.Empty : LpuRR2.Name,
                (LpuRR == null) ? "Российская федерация" : LpuRR.RegionRR.Name };
        }
        
        public override void Save()
        {
            if (ID == 0)
            {
                LpuList lpuList = new LpuList();
                if (lpuList.IsInList(INN))
                    throw new NullReferenceException("В списке уже есть организация с таким ИНН");
            }

            int idParentOrganization;
            idParentOrganization = (ParentOrganization == null) ? 0 : ParentOrganization.ID;

            int idLPURR = (LpuRR == null) ? 0 : LpuRR.ID;
            int idLPURR2 = (LpuRR2 == null) ? 0 : LpuRR2.ID;

            int id;
            int.TryParse(_provider.Insert("SF_LPU",
                ID,
                NumberSF,
                TypeOrg,
                Name,
                ShortName,
                (MainSpec == null) ? 0 : MainSpec.ID,
                Email,
                Website,
                Phone,
                idParentOrganization,
                (TypeLPU == null) ? 0 : TypeLPU.ID,
                (Ownership == null) ? 0 : Ownership.ID,
                (AdmLevel == null) ? 0 : AdmLevel.ID,
                INN,
                KPP,
                PostIndex,
                (City == null) ? 0 : City.ID,
                Street,
                idLPURR,
                _bedsTotal,
                _bedsIC,
                _bedsSurgical,
                _operating,
                _machineGD,
                _machineGDF,
                _machineCRRT,
                _shift,
                _patientGD,
                _patientPD,
                _patientCRRT,
                (SubRegion == null) ? 0 : SubRegion.ID,
                (TypeFin == null) ? 0 :TypeFin.ID,
                idLPURR2,
                Deleted.ToString(),
                CrmID
            ), out id);

            ID = id;

            OrganizationList.GetUniqueInstance().Add(this);
        }

        public bool IsTotalLessThenSum()
        {
            return (_bedsTotal < (_bedsIC + _bedsSurgical));
        }

        public bool IsHaveDepartment()
        {
            return OrganizationList.GetUniqueInstance().GetChildList(this).Count(item => item.TypeOrg == ClassLibrary.TypeOrg.Отделение) > 0;
        }
    }
}
