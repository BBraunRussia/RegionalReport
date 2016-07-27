using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ClassLibrary.Common;
using System.Data;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Models;
using ClassLibrary.SF.Interfaces;
using ClassLibrary.SF.Export;

namespace ClassLibrary.SF.Import
{
    public class ReadFileOrganization
    {
        private OrganizationList organizationList;
        private RealRegionList realRegionList;
        private CityList cityList;
        private AdmLevelList admLevelList;
        private TypeFinList typeFinList;
        private MainSpecList mainSpecList;
        private SubRegionList subRegionList;
        private TypeLPUList typeLPUList;
        private OwnershipList ownershipList;
        
        public ReadFileOrganization()
        {
            organizationList = OrganizationList.GetUniqueInstance();
            realRegionList = RealRegionList.GetUniqueInstance();
            cityList = CityList.GetUniqueInstance();
            admLevelList = AdmLevelList.GetUniqueInstance();
            typeFinList = TypeFinList.GetUniqueInstance();
            mainSpecList = MainSpecList.GetUniqueInstance();
            subRegionList = SubRegionList.GetUniqueInstance();
            typeLPUList = TypeLPUList.GetUniqueInstance();
            ownershipList = OwnershipList.GetUniqueInstance();
        }

        public void Start()
        {
            ReadFile readFile = new ReadFile(ImportFileType.Organization);

            if (readFile.dt == null)
                return;

            foreach (DataRow row in readFile)
            {
                Update(new OrganizationModel(row));
            }
        }

        private void Update(OrganizationModel model)
        {
            Organization organization = GetOrganization(model);
            if (organization == null)
                return;
            
            organization.NumberSF = model.NumberSF;
            organization.CrmID = model.CrmID;
            organization.ParentOrganization = (model.ParentNumberSF == string.Empty) ? null : organizationList.GetItem(model.ParentNumberSF);
            organization.Name = model.Name;
            organization.ShortName = model.ShortName;
            organization.Email = model.Email;
            organization.Website = model.Website;
            organization.Phone = model.Phone;
            organization.Pharmacy = model.Pharmacy;
            organization.Deleted = model.Deleted;

            organization.INN = model.INN;
            organization.KPP = model.KPP;
            organization.RealRegion = GetItem(realRegionList, model.RealRegion, "Region", model.NumberSF) as RealRegion;
            organization.City = GetItem(cityList, model.City, "City", model.NumberSF) as City;
            organization.PostIndex = model.PostIndex;
            organization.Street = model.Street;
            
            if (organization is LPU)
            {
                LPU lpu = organization as LPU;
                lpu.AdmLevel = GetItem(admLevelList, model.AdmLevel, "AdminLevel", model.NumberSF) as AdmLevel;
                lpu.TypeFin = GetItem(typeFinList, model.TypeFin, "TypeFin", model.NumberSF) as TypeFin;
                lpu.MainSpec = GetItem(mainSpecList, model.MainSpec, "MainSpec", model.NumberSF) as MainSpec;

                SubRegion subRegion = subRegionList.GetItemByCode(model.SubRegion);
                lpu.SubRegion = subRegion;
                if (subRegion == null)
                {
                    Logger.WriteNotFound(model.SubRegion, "SubRegion", model.NumberSF);
                }
                
                lpu.BedsTotal = model.BedsTotal;
                lpu.BedsIC = model.BedsIC;
                lpu.BedsSurgical = model.BedsSurgical;
                lpu.Operating = model.Operating;
                
                lpu.TypeLPU = GetItem(typeLPUList, model.TypeLPU, "Hospital type", model.NumberSF) as TypeLPU;
                lpu.Ownership = GetItem(ownershipList, model.Ownership, "Ownership", model.NumberSF) as Ownership;
            }
            if (organization is IAvitum)
            {
                IAvitum avitum = organization as IAvitum;
                avitum.MachineGD = model.MachineGD;
                avitum.MachineGDF = model.MachineGDF;
                avitum.MachineCRRT = model.MachineCRRT;
                avitum.Shift = model.Shifts;
                avitum.PatientGD = model.PatientGD;
                avitum.PatientPD = model.PatientPD;
                avitum.PatientCRRT = model.PatientCRRT;
            }
            
            organization.Save();
        }

        private BaseDictionary GetItem(BaseList list, string name, string nameHeader, string numberSF)
        {
            BaseDictionary item = list.GetItem(name);
            if (item == null)
            {
                Logger.WriteNotFound(name, nameHeader, numberSF);
            }

            return item;
        }
        
        private Organization GetOrganization(OrganizationModel model)
        {
            Organization organization = organizationList.GetItem(model.NumberSF);

            if (organization == null)
            {
                try
                {
                    TypeOrg typeOrg = GetTypeOrg(model);
                    organization = new Organization(typeOrg);
                }
                catch (NullReferenceException)
                {
                    Logger.WriteNotFound(string.Concat(model.RecordType, " ", model.ClientType), "Organization type", model.NumberSF);
                }
            }

            return organization;
        }

        public TypeOrg GetTypeOrg(OrganizationModel model)
        {
            if (RecordType.RU_Hospital.ToString() == model.RecordType)
                return TypeOrg.ЛПУ;
            if (((RecordType.RU_Department.ToString() == model.RecordType) && (model.MainSpec == "Pharmaciuticals")) || (RecordType.RU_Pharmacy.ToString() == model.RecordType))
                return TypeOrg.Аптека;
            if (RecordType.RU_Department.ToString() == model.RecordType)
                return TypeOrg.Отделение;
            if (RecordType.RU_Institution_Buying.ToString() == model.RecordType)
                return TypeOrg.Дистрибьютор;
            if (model.ClientType == ExportOrganization.clientType[0])
                return TypeOrg.Отдел;
            if (model.ClientType == ExportOrganization.clientType[1])
                return TypeOrg.Административное_Учреждение;
            if (model.ClientType == ExportOrganization.clientType[2])
                return TypeOrg.Дистрибьютор;
            if (model.ClientType == ExportOrganization.clientType[3])
                return TypeOrg.Ветеренарная_клиника;
            if (model.ClientType == ExportOrganization.clientType[4])
                return TypeOrg.Стоматология;
            
            throw new NullReferenceException("Failed recognize Organization type");
        }
    }
}
