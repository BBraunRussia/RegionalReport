using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LPU : Organization
    {
        private LpuRR _lpuRR;

        private int _idTypeLPU;
        private int _idOwnership;
        private int _idAdmLevel;

        private string _kpp;
        private string _postIndex;
        private int _idCity;
        private string _district;
        private string _street;

        private string _inn;

        private int _bedsTotal;
        private int _bedsIC;
        private int _surgical;
        private int _operating;
        private int _machineGD;
        private int _machineGDF;
        private int _machineCRRT;
        private int _shift;
        private int _patientGD;
        private int _patientPD;
        private int _patientCRRT;
        
        public LPU(DataRow row)
            : base(row)
        {
            int.TryParse(row[10].ToString(), out _idTypeLPU);
            int.TryParse(row[11].ToString(), out _idOwnership);
            int.TryParse(row[12].ToString(), out _idAdmLevel);
            _kpp = row[13].ToString();
            _postIndex = row[14].ToString();
            int.TryParse(row[15].ToString(), out _idCity);
            _district = row[16].ToString();
            _street = row[17].ToString();

            _inn = row[18].ToString();

            int idLpuRR;
            int.TryParse(row[19].ToString(), out idLpuRR);
            LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
            _lpuRR = lpuRRList.GetItem(idLpuRR) as LpuRR;

            int.TryParse(row[20].ToString(), out _bedsTotal);
            int.TryParse(row[21].ToString(), out _bedsIC);
            int.TryParse(row[22].ToString(), out _surgical);
            int.TryParse(row[23].ToString(), out _operating);
            int.TryParse(row[24].ToString(), out _machineGD);
            int.TryParse(row[25].ToString(), out _machineGDF);
            int.TryParse(row[26].ToString(), out _machineCRRT);
            int.TryParse(row[27].ToString(), out _shift);
            int.TryParse(row[28].ToString(), out _patientGD);
            int.TryParse(row[29].ToString(), out _patientPD);
            int.TryParse(row[30].ToString(), out _patientCRRT);
        }

        public LPU(TypeOrg typeOrg)
            : base(typeOrg)
        { }

        public string KPP
        {
            get { return _kpp; }
            set { _kpp = value; }
        }

        public string PostIndex
        {
            get { return _postIndex; }
            set { _postIndex = value; }
        }

        public string District
        {
            get { return _district; }
            set { _district = value; }
        }

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        public City City
        {
            get
            {
                if (_idCity == 0)
                    return null;

                CityList cityList = CityList.GetUniqueInstance();
                return cityList.GetItem(_idCity) as City;
            }
            set { _idCity = value.ID; }
        }

        public RealRegion RealRegion { get { return (City == null) ? null : City.RealRegion; } }

        public RegionCompetitors RegionCompetitors
        {
            set
            {
                RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
                RealRegion realRegion = realRegionList.GetItem(value);

                CityList cityList = CityList.GetUniqueInstance();

                City city = cityList.GetItem(realRegion);

                City = city;
            }
        }

        public TypeLPU TypeLPU
        {
            get
            {
                if (_idTypeLPU == 0)
                    return null;

                TypeLPUList typeLPUList = TypeLPUList.GetUniqueInstance();
                return typeLPUList.GetItem(_idTypeLPU) as TypeLPU;
            }
            set { _idTypeLPU = value.ID; }
        }

        public AdmLevel AdmLevel
        {
            get
            {
                if (_idAdmLevel == 0)
                    return null;

                AdmLevelList admLevelList = AdmLevelList.GetUniqueInstance();
                return admLevelList.GetItem(_idAdmLevel) as AdmLevel;
            }
            set { _idAdmLevel = value.ID; }
        }

        public Ownership Ownership
        {
            get
            {
                if (_idOwnership == 0)
                    return null;

                OwnershipList ownershipList = OwnershipList.GetUniqueInstance();
                return ownershipList.GetItem(_idOwnership) as Ownership;
            }
            set { _idOwnership = value.ID; }
        }

        public string INN
        {
            get { return _inn; }
            set { _inn = value; }
        }

        public LpuRR LpuRR
        {
            get { return _lpuRR; }
            set { _lpuRR = value; }
        }

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
        public string Surgical
        {
            get { return (_surgical == 0) ? string.Empty : _surgical.ToString(); }
            set { int.TryParse(value, out _surgical); }
        }
        public string Operating
        {
            get { return (_operating == 0) ? string.Empty : _operating.ToString(); }
            set { int.TryParse(value, out _operating); }
        }
        public string MachineGD
        {
            get { return (_machineGD == 0) ? string.Empty : _machineGD.ToString(); }
            set { int.TryParse(value, out _machineGD); }
        }
        public string MachineGDF
        {
            get { return (_machineGDF == 0) ? string.Empty : _machineGDF.ToString(); }
            set { int.TryParse(value, out _machineGDF); }
        }
        public string MachineCRRT
        {
            get { return (_machineCRRT == 0) ? string.Empty : _machineCRRT.ToString(); }
            set { int.TryParse(value, out _machineCRRT); }
        }
        public string Shift
        {
            get { return (_shift == 0) ? string.Empty : _shift.ToString(); }
            set { int.TryParse(value, out _shift); }
        }
        public string PatientGD
        {
            get { return (_patientGD == 0) ? string.Empty : _patientGD.ToString(); }
            set { int.TryParse(value, out _patientGD); }
        }
        public string PatientPD
        {
            get { return (_patientPD == 0) ? string.Empty : _patientPD.ToString(); }
            set { int.TryParse(value, out _patientPD); }
        }
        public string PatientCRRT
        {
            get { return (_patientCRRT == 0) ? string.Empty : _patientCRRT.ToString(); }
            set { int.TryParse(value, out _patientCRRT); }
        }

        public object[] GetRow()
        {
            string typeOrgName = ((TypeOrg == SF.TypeOrg.ЛПУ) && (ParentOrganization != null)) ? "Филиал ЛПУ" : TypeOrg.ToString();

            return new object[] { ID, NumberSF, ShortName, typeOrgName, (ParentOrganization == null) ? INN : (ParentOrganization as LPU).INN, RealRegion.Name, City.Name, (LpuRR == null) ? "Прочие ЛПУ" : LpuRR.Name, (LpuRR == null) ? "Российская федерация" : LpuRR.RegionRR.Name };
        }
        
        public override void Save()
        {
            int id;

            int idParentOrganization;
            idParentOrganization = (ParentOrganization == null) ? 0 : ParentOrganization.ID;

            int idLPURR = (LpuRR == null) ? 0 : LpuRR.ID;

            int.TryParse(_provider.Insert("SF_LPU", ID, NumberSF, TypeOrg, Name, ShortName, MainSpec.ID, Email, WebSite, Phone, idParentOrganization, TypeLPU.ID, Ownership.ID, AdmLevel.ID,
                INN, KPP, PostIndex, City.ID, District, Street, idLPURR,
                _bedsTotal, _bedsIC, _surgical, _operating, _machineGD, _machineGDF, _machineCRRT, _shift, _patientGD, _patientPD, _patientCRRT), out id);

            SetID(id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }
    }
}
