using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LPU : Organization, IHaveRegion, IAvitum
    {
        private int _bedsTotal;
        private int _bedsIC;
        private int _bedsSurgical;
        private int _operating;
        
        public LPU(DataRow row)
            : base(row)
        {
            int idTypeLPU;
            int.TryParse(row[10].ToString(), out idTypeLPU);
            TypeLPU = TypeLPUList.GetUniqueInstance().GetItem(idTypeLPU) as TypeLPU;

            int idOwnership;
            int.TryParse(row[11].ToString(), out idOwnership);
            Ownership = OwnershipList.GetUniqueInstance().GetItem(idOwnership) as Ownership;

            int idAdmLevel;
            int.TryParse(row[12].ToString(), out idAdmLevel);
            AdmLevel = AdmLevelList.GetUniqueInstance().GetItem(idAdmLevel) as AdmLevel;

            KPP = row[13].ToString();
            PostIndex = row[14].ToString();
            
            int idCity;
            int.TryParse(row[15].ToString(), out idCity);
            City = CityList.GetUniqueInstance().GetItem(idCity) as City;

            District = row[16].ToString();
            Street = row[17].ToString();

            INN = row[18].ToString();

            int idLpuRR;
            int.TryParse(row[19].ToString(), out idLpuRR);
            LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
            LpuRR = lpuRRList.GetItem(idLpuRR) as LpuRR;

            int.TryParse(row[20].ToString(), out _bedsTotal);
            int.TryParse(row[21].ToString(), out _bedsIC);
            int.TryParse(row[22].ToString(), out _bedsSurgical);
            int.TryParse(row[23].ToString(), out _operating);
            
            int idSubRegion;
            int.TryParse(row[32].ToString(), out idSubRegion);
            SubRegionList subRegionList = SubRegionList.GetUniqueInstance();
            SubRegion = subRegionList.GetItem(idSubRegion) as SubRegion;

            int idTypeFin;
            int.TryParse(row[33].ToString(), out idTypeFin);
            TypeFinList typeFinList = TypeFinList.GetUniqueInstance();
            TypeFin = typeFinList.GetItem(idTypeFin) as TypeFin;

            int idLpuRR2;
            int.TryParse(row[34].ToString(), out idLpuRR2);
            LpuRR2 = lpuRRList.GetItem(idLpuRR2) as LpuRR;
        }

        public LPU(TypeOrg typeOrg)
            : base(typeOrg)
        { }

        public string KPP { get; set; }
        public string PostIndex { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string INN { get; set; }
        public LpuRR LpuRR { get; set; }
        public LpuRR LpuRR2 { get; set; }
        public SubRegion SubRegion { get; set; }
        public TypeFin TypeFin { get; set; }
        public City City { get; set; }        

        public RealRegion RealRegion
        {
            get { return (City == null) ? null : City.RealRegion; }
            set
            {
                CityList cityList = CityList.GetUniqueInstance();
                City city = cityList.GetItem(value);
                City = city;
            }
        }

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
        
        public object[] GetRow()
        {
            string typeOrgName = ((TypeOrg == TypeOrg.ЛПУ) && (ParentOrganization != null)) ? "Филиал ЛПУ" : TypeOrg.ToString();

            return new object[] { ID, NumberSF, ShortName, typeOrgName, (ParentOrganization == null) ? INN : (ParentOrganization as LPU).INN, RealRegion.Name, City.Name, (LpuRR == null) ? "Прочие ЛПУ" : LpuRR.Name, ((LpuRR2 == null) || (LpuRR2.ID == 0)) ? String.Empty : LpuRR2.Name, (LpuRR == null) ? "Российская федерация" : LpuRR.RegionRR.Name };
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
            int.TryParse(_provider.Insert("SF_LPU", ID, NumberSF, TypeOrg, Name, ShortName, (MainSpec == null) ? 0 : MainSpec.ID, Email, Website, Phone,
                idParentOrganization, (TypeLPU == null) ? 0 : TypeLPU.ID, (Ownership == null) ? 0 : Ownership.ID, (AdmLevel == null) ? 0 : AdmLevel.ID,
                INN, KPP, PostIndex, (City == null) ? 0 : City.ID, District, Street, idLPURR, _bedsTotal, _bedsIC, _bedsSurgical, _operating, _machineGD,
                _machineGDF, _machineCRRT, _shift, _patientGD, _patientPD, _patientCRRT, (SubRegion == null) ? 0 : SubRegion.ID,
                (TypeFin == null) ? 0 :TypeFin.ID, idLPURR2, Deleted.ToString()), out id);

            ID = id;

            OrganizationList.GetUniqueInstance().Add(this);
        }

        public bool IsTotalLessThenSum()
        {
            return (_bedsTotal < (_bedsIC + _bedsSurgical));
        }

        public bool IsHaveDepartment()
        {
            return OrganizationList.GetUniqueInstance().GetChildList(this).Exists(item => item.TypeOrg == ClassLibrary.TypeOrg.Отделение);
        }
    }
}
