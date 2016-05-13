﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LPU : Organization, IHaveRegion, IAvitum
    {
        private LpuRR _lpuRR;
        private LpuRR _lpuRR2;

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
        private int _bedsSurgical;
        private int _operating;
        
        private SubRegion _subRegion;
        private TypeFin _typeFin;
        
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
            int.TryParse(row[22].ToString(), out _bedsSurgical);
            int.TryParse(row[23].ToString(), out _operating);
            
            int idSubRegion;
            int.TryParse(row[32].ToString(), out idSubRegion);
            SubRegionList subRegionList = SubRegionList.GetUniqueInstance();
            _subRegion = subRegionList.GetItem(idSubRegion) as SubRegion;

            int idTypeFin;
            int.TryParse(row[33].ToString(), out idTypeFin);
            TypeFinList typeFinList = TypeFinList.GetUniqueInstance();
            _typeFin = typeFinList.GetItem(idTypeFin) as TypeFin;

            int idLpuRR2;
            int.TryParse(row[34].ToString(), out idLpuRR2);
            _lpuRR2 = lpuRRList.GetItem(idLpuRR2) as LpuRR;
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

        public LpuRR LpuRR2
        {
            get { return _lpuRR2; }
            set { _lpuRR2 = value; }
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
        
        public SubRegion SubRegion
        {
            get { return _subRegion; }
            set { _subRegion = value; }
        }

        public TypeFin TypeFin
        {
            get { return _typeFin; }
            set { _typeFin = value; }
        }

        public object[] GetRow()
        {
            string typeOrgName = ((TypeOrg == TypeOrg.ЛПУ) && (ParentOrganization != null)) ? "Филиал ЛПУ" : TypeOrg.ToString();

            return new object[] { ID, ShortName, typeOrgName, (ParentOrganization == null) ? INN : (ParentOrganization as LPU).INN, RealRegion.Name, City.Name, (LpuRR == null) ? "Прочие ЛПУ" : LpuRR.Name, ((LpuRR2 == null) || (LpuRR2.ID == 0)) ? String.Empty : LpuRR2.Name, (LpuRR == null) ? "Российская федерация" : LpuRR.RegionRR.Name };
        }
        
        public override void Save()
        {
            int id;

            int idParentOrganization;
            idParentOrganization = (ParentOrganization == null) ? 0 : ParentOrganization.ID;

            int idLPURR = (LpuRR == null) ? 0 : LpuRR.ID;
            int idLPURR2 = (LpuRR2 == null) ? 0 : LpuRR2.ID;

            int.TryParse(_provider.Insert("SF_LPU", ID, NumberSF, TypeOrg, Name, ShortName, MainSpec.ID, Email, WebSite, Phone, idParentOrganization, TypeLPU.ID, Ownership.ID, AdmLevel.ID,
                INN, KPP, PostIndex, City.ID, District, Street, idLPURR,
                _bedsTotal, _bedsIC, _bedsSurgical, _operating, _machineGD, _machineGDF, _machineCRRT, _shift, _patientGD, _patientPD, _patientCRRT, _subRegion.ID, _typeFin.ID, idLPURR2), out id);

            SetID(id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }

        public bool IsTotalLessThenSum()
        {
            return (_bedsTotal < (_bedsIC + _bedsSurgical));
        }

        public bool IsHaveDepartment()
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();

            return organizationList.GetChildList(this).Exists(item => item.TypeOrg == ClassLibrary.TypeOrg.Отделение);
        }
    }
}
