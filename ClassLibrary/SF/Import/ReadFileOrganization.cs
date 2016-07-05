using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ClassLibrary.Common;
using System.Data;

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
            organization.ParentOrganization = (model.ParentNumberSF == string.Empty) ? null : organizationList.GetItem(model.ParentNumberSF);
            organization.Name = model.Name;
            organization.ShortName = model.ShortName;
            organization.Email = model.Email;
            organization.Website = model.Website;
            organization.Phone = model.Phone;
            organization.Deleted = model.Deleted;

            if (organization is IHaveRegion)
            {
                IHaveRegion orgHaveRegion = organization as IHaveRegion;
                orgHaveRegion.INN = model.INN;
                orgHaveRegion.KPP = model.KPP;
                orgHaveRegion.RealRegion = GetItem(realRegionList, model.RealRegion, "Регион", model.NumberSF) as RealRegion;
                orgHaveRegion.City = GetItem(cityList, model.City, "Город", model.NumberSF) as City;
                orgHaveRegion.PostIndex = model.PostIndex;
                orgHaveRegion.Street = model.Street;
            }
            if (organization is LPU)
            {
                LPU lpu = organization as LPU;
                lpu.AdmLevel = GetItem(admLevelList, model.AdmLevel, "AdmLevel", model.NumberSF) as AdmLevel;
                lpu.TypeFin = GetItem(typeFinList, model.TypeFin, "TypeFin", model.NumberSF) as TypeFin;
                lpu.MainSpec = GetItem(mainSpecList, model.MainSpec, "MainSpec", model.NumberSF) as MainSpec;

                SubRegion subRegion = subRegionList.GetItemByCode(model.SubRegion);
                lpu.SubRegion = subRegion;
                if (subRegion == null)
                {
                    WriteToLog(model.SubRegion, "Субрегион", model.NumberSF);
                }
                
                lpu.BedsTotal = model.BedsTotal;
                lpu.BedsIC = model.BedsIC;
                lpu.BedsSurgical = model.BedsSurgical;
                lpu.Operating = model.Operating;
                
                lpu.TypeLPU = GetItem(typeLPUList, model.TypeLPU, "Тип ЛПУ", model.NumberSF) as TypeLPU;
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
            
            if (organization is OtherOrganization)
            {
                (organization as OtherOrganization).Pharmacy = model.Pharmacy;
            }

            organization.Save();
        }

        private BaseDictionary GetItem(BaseList list, string name, string nameHeader, string numberSF)
        {
            BaseDictionary item = list.GetItem(name);
            if (item == null)
            {
                WriteToLog(name, nameHeader, numberSF);
            }

            return item;
        }

        private void WriteToLog(string name, string nameHeader, string numberSF)
        {
            Logger.Write(string.Format("{0} не найден: {1}, SFNumber: {2}", nameHeader, name, numberSF));
        }
        
        private Organization GetOrganization(OrganizationModel model)
        {
            Organization organization = organizationList.GetItem(model.NumberSF);

            if (organization == null)
            {
                TypeOrg typeOrg = GetTypeOrg(model);
                organization = new Organization(typeOrg);
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
            
            throw new NullReferenceException("Не удалось определеть тип организации");
        }
        /*
        private void Update(DataRow row)
        {
            Organization organization = GetOrganization(row);
            if (organization == null)
                return;

            organization.NumberSF = fields[0];
            organization.ParentOrganization = (fields[3] == string.Empty) ? null : organizationList.GetItem(fields[3]);
            organization.Name = fields[4];
            organization.ShortName = fields[5];
            if (organization is IHaveRegion)
            {
                IHaveRegion orgHaveRegion = organization as IHaveRegion;
                orgHaveRegion.INN = fields[6];
                orgHaveRegion.KPP = fields[7];
                orgHaveRegion.RealRegion = realRegionList.GetItem(fields[8]) as RealRegion;
                orgHaveRegion.City = cityList.GetItem(fields[9]) as City;
                orgHaveRegion.PostIndex = fields[10];
                orgHaveRegion.Street = fields[11];
            }
            if (organization is LPU)
            {
                LPU lpu = organization as LPU;
                lpu.AdmLevel = admLevelList.GetItem(fields[12]) as AdmLevel;
                lpu.TypeFin = typeFinList.GetItem(fields[13]) as TypeFin;
                lpu.MainSpec = mainSpecList.GetItem(fields[14]) as MainSpec;
                lpu.SubRegion = subRegionList.GetItemByCode(fields[15]);
                lpu.BedsTotal = fields[16];
                lpu.BedsIC = fields[17];
                lpu.BedsSurgical = fields[18];
                lpu.Operating = fields[19];
                lpu.TypeLPU = typeLPUList.GetItem(fields[32]) as TypeLPU;
                lpu.Ownership = ownershipList.GetItem(fields[33]) as Ownership;
            }
            if (organization is IAvitum)
            {
                IAvitum avitum = organization as IAvitum;
                avitum.MachineGD = fields[20];
                avitum.MachineGDF = fields[21];
                avitum.MachineCRRT = fields[22];
                avitum.Shift = fields[23];
                avitum.PatientGD = fields[24];
                avitum.PatientPD = fields[25];
                avitum.PatientCRRT = fields[26];
            }

            organization.Email = fields[27];
            organization.Website = fields[28];
            organization.Phone = fields[29];

            if (organization is OtherOrganization)
            {
                (organization as OtherOrganization).Pharmacy = fields[30];
            }

            organization.Deleted = Convert.ToBoolean(fields[34]);

            organization.Save();
        }
        
        private Organization GetOrganization(DataRow row)
        {
            int idOrganization;
            int.TryParse(row[Columns.sf_id.ToString()].ToString(), out idOrganization);

            if (idOrganization == 0)
            {
                TypeOrg typeOrg = GetTypeOrg(row);
                return new Organization(typeOrg);
            }
            else
            {
                return organizationList.GetItem(idOrganization);
            }
        }

        private TypeOrg GetTypeOrg(DataRow row)
        {
            string recordType = row[Columns.recordType.ToString()].ToString();
            string parent = row[Columns.sf_parent_id.ToString()].ToString();
            string mainSpec = row[Columns.mainSpec.ToString()].ToString();
            string typeOrg = row[Columns.typeOrg.ToString()].ToString();

            if (RecordType.RU_Hospital.ToString() == recordType)
                return TypeOrg.ЛПУ;
            if (((RecordType.RU_Department.ToString() == recordType) && (mainSpec == "Pharmaciuticals")) || (RecordType.RU_Pharmacy.ToString() == recordType))
                return TypeOrg.Аптека;
            if (RecordType.RU_Department.ToString() == recordType)
                return TypeOrg.Отделение;
            if (typeOrg == ExportOrganization.clientType[0])
                return TypeOrg.Отдел;
            if (typeOrg == ExportOrganization.clientType[1])
                return TypeOrg.Административное_Учреждение;
            if (typeOrg == ExportOrganization.clientType[2])
                return TypeOrg.Дистрибьютор;

            throw new NullReferenceException("Не удалось определеть тип организации");
        }
        */
    }
}
