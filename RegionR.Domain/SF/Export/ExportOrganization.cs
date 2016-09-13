using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Entities;
using ClassLibrary.SF.Common;
using RegionReport.Domain;

namespace ClassLibrary.SF.Export
{
    public enum RecordType { RU_Hospital, RU_Department, RU_Pharmacy, RU_Other, RU_Institution_Buying }
    
    public class ExportOrganization
    {
        private string[] _columnNamesEng = { "Z_RU_RR_Institution__c", "Parent", "Z_Record_Type_Developer_Name__c", "Z_RU_Customer_long_name__c",
                                               "Name", "Z_RU_TIN__c", "Z_RU_CRR__c", "BILLINGSTATE", "BILLINGCITY", "BILLINGPOSTALCODE", "BILLINGSTREET",
                                               "Z_R3_Email_Address__c", "Website", "Phone", "Z_Customer_Classification__c", "Z_RU_Institution_Sub_Type__c",
                                               "Z_Hospital_Type__c", "Ownership", "Z_RU_Type_of_geographical_level__c", "Z_SEE_Financial_Status__c",
                                               "Z_RU_Institution_Main_Specialities__c", "Z_R3_Territories_RU__c", "Z_Cd_Number_of_Beds__c", "Z_ICU_Beds__c",
                                               "Z_Number_of_acute_beds_in_hospital__c", "Z_RU_Number_of_surgical_rooms__c", "Z_RU_Number_of_HD_dialysis_machines__c",
                                               "Z_RU_Number_of_HDF_dialysis_machines__c", "Z_RU_Number_of_CRRT_machines__c", "Z_RU_Number_of_shifts__c",
                                               "Z_RU_Number_of_patient_HD__c", "Z_RU_Number_of_patient_PD__c", "Z_RU_Number_of_accute_patients_per_year__c" };

        private string[] _columnNamesRus = { "ID", "Parent ID", "CRM ID", "Тип записи", "Тип организации", "Client type", "Официальное название организации", "Сокращённое название",
            "ИНН", "КПП", "Регион России", "Город", "Индекс", "Уличный адрес", "Адрес эл. почты", "Веб-сайт", "Телефонный номер",
            "Категория коммерческой аптеки", "Тип ЛПУ", "Форма собственности", "Административное подчинение", "Тип финансирования", "Основная специализация",
            "Sales District", "Номер ЛПУ-RR", "Номер ЛПУ-RR2", "Кол-во коек общее", "Кол-во коек реанимационных", "Кол-во коек хирургических", "Кол-во операционных", "Кол-во ГД машин",
            "Кол-во ГДФ машин", "Кол-во CRRT машин", "Кол-во смен", "Кол-во ГД пациентов", "Кол-во ПД пациентов", "Кол-во CRRT пациентов",
            "Создал", "Дата и время создания", "Изменил", "Дата и время изменения" };
        
        private string[] _typeOrgRus = {"ЛПУ", "Филиал ЛПУ", "Отделение ЛПУ", "Аптека ЛПУ", "Отдел ЛПУ", "Аптека коммерческая", "Административное учреждение",
                                       "Дистрибьютор", "Дистрибьютор (пока не покупал)"};
        private string[] _typeOrgEng = {"Hospital", "Hospital branch", "Hospital department (medical)", "Hospital pharmacy", "Hospital department (non-medical)",
                                       "Pharmacy", "Governmental-administrative establishment", "Distributor (Buying)", "Distributor (Non-Buying)"};

        public static readonly string[] clientType = { "Department (purchasing dep., etc)", "Governmental-administrative establishment", "Dealer",
                                                     "Veterinary clinic", "Dentist" };

        public void ExportRus()
        {
            Export(Language.Rus);
        }

        public void ExportEng()
        {
            Export(Language.Eng);
        }
        
        private void Export(Language lang)
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();

            string[] columnNames = (lang == Language.Rus) ? _columnNamesRus : _columnNamesEng;

            DataTable dt = CreateDataTable(columnNames);

            HistoryList historyList = HistoryList.GetUniqueInstance();

            foreach (var item in organizationList.List)
            {
                Organization organization = item.Value;

                LPU lpu = (organization.TypeOrg == TypeOrg.ЛПУ) ? (organization as LPU) : null;

                int parentID = (organization.ParentOrganization == null) ? organization.ID : organization.ParentOrganization.ID;
                string recordType = GetRecordType(organization);
                
                string inn = (organization.INN == "") ? "" : "'" + organization.INN;
                string kpp = (organization.KPP == "") ? "" : "'" + organization.KPP;
                string realRegionName = (organization.RealRegion == null) ? "" : organization.RealRegion.Name;
                string postIndex = organization.PostIndex;
                
                string pharmacy = ((organization.TypeOrg == TypeOrg.Аптека) && (organization.ParentOrganization == null)) ? (organization as Organization).Pharmacy : string.Empty;
                string mainSpec = (organization.MainSpec != null) ? organization.MainSpec.GetName(lang) : string.Empty;
                
                string typeLPU = string.Empty;
                string ownership = string.Empty;
                string adminLevel = string.Empty;
                string typeFin = string.Empty;
                string subRegion = string.Empty;

                if (lpu != null)
                {
                    typeLPU = lpu.TypeLPU.GetName(lang);
                    ownership = lpu.Ownership.GetName(lang);
                    adminLevel = lpu.AdmLevel.GetName(lang);
                    typeFin = lpu.TypeFin.GetName(lang);
                    subRegion = lpu.SubRegion.Name.Split(' ')[0];
                }

                object[] row;

                if (lang == Language.Rus)
                {
                    string street = organization.Street;

                    string idLpuRR = string.Empty;
                    string idLpuRR2 = string.Empty;

                    if (lpu != null)
                    {
                        idLpuRR = ((lpu.ParentOrganization == null) || ((lpu.ParentOrganization != null) && (lpu.LpuRR.ID != 0))) ? lpu.LpuRR.ID.ToString() : string.Empty;
                        idLpuRR2 = (lpu.LpuRR2.ID != 0) ? lpu.LpuRR2.ID.ToString() : string.Empty;
                    }
                    
                    History created = historyList.GetItem(organization, HistoryAction.Создал);
                    History modifed = historyList.GetItem(organization, HistoryAction.Редактировал);
                    string createdAuthor = (created != null) ? created.Author : string.Empty;
                    string createdDatetime = (created != null) ? created.datetime : string.Empty;
                    string modifedAuthor = (modifed != null) ? modifed.Author : string.Empty;
                    string modifedDatetime = (modifed != null) ? modifed.datetime : string.Empty;

                    row = new object[] { organization.ID, parentID, organization.NumberSF, recordType, GetFormatTypeOrg(organization), GetClientType(organization),
                               organization.Name, organization.ShortName, inn, kpp, realRegionName, organization.City, postIndex, street,
                               organization.Email, organization.Website, organization.Phone, pharmacy, typeLPU, ownership, adminLevel, typeFin, mainSpec,
                               subRegion, idLpuRR, idLpuRR2,
                               (lpu != null) ? lpu.BedsTotal : string.Empty, (lpu != null) ? lpu.BedsIC : string.Empty, (lpu != null) ? lpu.BedsSurgical : string.Empty,
                               (lpu != null) ? lpu.Operating : string.Empty,
                               organization.MachineGD, organization.MachineGDF, organization.MachineCRRT, organization.Shift, organization.PatientGD,
                               organization.PatientPD, organization.PatientCRRT, createdAuthor, createdDatetime, modifedAuthor, modifedDatetime };
                }
                else
                {
                    string MachineGD = string.Empty;
                    string MachineGDF = string.Empty;
                    string MachineCRRT = string.Empty;
                    string Shift = string.Empty;
                    string PatientGD = string.Empty;
                    string PatientPD = string.Empty;
                    string PatientCRRT = string.Empty;

                    if ((lpu != null) && (lpu.IsHaveDepartment()))
                    {
                        OrganizationList organizationoList = OrganizationList.GetUniqueInstance();

                        var childList = from child in organizationoList.GetChildList(lpu)
                                        where child.TypeOrg == TypeOrg.Отделение
                                        select new
                                        {
                                            MachineGD = string.IsNullOrEmpty(child.MachineGD) ? 0 : Convert.ToInt32(child.MachineGD),
                                            MachineGDF = string.IsNullOrEmpty(child.MachineGDF) ? 0 : Convert.ToInt32(child.MachineGDF),
                                            MachineCRRT = string.IsNullOrEmpty(child.MachineCRRT) ? 0 : Convert.ToInt32(child.MachineCRRT),
                                            Shift = string.IsNullOrEmpty(child.Shift) ? 0 : Convert.ToInt32(child.Shift),
                                            PatientGD = string.IsNullOrEmpty(child.PatientGD) ? 0 : Convert.ToInt32(child.PatientGD),
                                            PatientPD = string.IsNullOrEmpty(child.PatientPD) ? 0 : Convert.ToInt32(child.PatientPD),
                                            PatientCRRT = string.IsNullOrEmpty(child.PatientCRRT) ? 0 : Convert.ToInt32(child.PatientCRRT)
                                        };
                        
                        MachineGD = childList.Sum(child => child.MachineGD).ToString();
                        if (MachineGD == "0")
                            MachineGD = string.Empty;

                        MachineGDF = childList.Sum(child => child.MachineGDF).ToString();
                        if (MachineGDF == "0")
                            MachineGDF = string.Empty;

                        MachineCRRT = childList.Sum(child => child.MachineCRRT).ToString();
                        if (MachineCRRT == "0")
                            MachineCRRT = string.Empty;

                        Shift = childList.Sum(child => child.Shift).ToString();
                        if (Shift == "0")
                            Shift = string.Empty;

                        PatientGD = childList.Sum(child => child.PatientGD).ToString();
                        if (PatientGD == "0")
                            PatientGD = string.Empty;

                        PatientPD = childList.Sum(child => child.PatientPD).ToString();
                        if (PatientPD == "0")
                            PatientPD = string.Empty;

                        PatientCRRT = childList.Sum(child => child.PatientCRRT).ToString();
                        if (PatientCRRT == "0")
                            PatientCRRT = string.Empty;
                    }
                    else
                    {
                        MachineGD = organization.MachineGD;
                        MachineGDF = organization.MachineGDF;
                        MachineCRRT = organization.MachineCRRT;
                        Shift = organization.Shift;
                        PatientGD = organization.PatientGD;
                        PatientPD = organization.PatientPD;
                        PatientCRRT = organization.PatientCRRT;
                    }

                    row = new object[] { organization.ID, parentID, recordType,
                               organization.Name, organization.ShortName, inn, kpp, realRegionName, organization.City, postIndex, GetAddressWithDistrict(organization),
                               organization.Email, organization.Website, organization.Phone, pharmacy, GetClientType(organization), typeLPU, ownership, adminLevel, typeFin, mainSpec, subRegion,
                               (lpu != null) ? lpu.BedsTotal : string.Empty, (lpu != null) ? lpu.BedsIC : string.Empty, (lpu != null) ? lpu.BedsSurgical : string.Empty,
                               (lpu != null) ? lpu.Operating : string.Empty, MachineGD, MachineGDF, MachineCRRT, Shift, PatientGD, PatientPD, PatientCRRT };
                }

                dt.Rows.Add(row);
            }

            CreateExcel excel = new CreateExcel(dt);
            excel.Show();
        }
        
        private DataTable CreateDataTable(string[] columnNames)
        {
            DataTable dt = new DataTable();

            foreach(var item in columnNames)
                dt.Columns.Add(item);

            return dt;
        }

        public string GetRecordType(Organization organization)
        {
            if (organization.TypeOrg == TypeOrg.ЛПУ)
                return RecordType.RU_Hospital.ToString();
            if ((organization.TypeOrg == TypeOrg.Отделение) || ((organization.TypeOrg == TypeOrg.Аптека) && (organization.ParentOrganization != null)))
                return RecordType.RU_Department.ToString();
            if (organization.TypeOrg == TypeOrg.Аптека)
                return RecordType.RU_Pharmacy.ToString();

            return RecordType.RU_Other.ToString();
        }
        
        private string GetFormatTypeOrg(Organization organization)
        {
            string[] typeOrg = _typeOrgRus;

            if ((organization.TypeOrg == TypeOrg.ЛПУ) && (organization.ParentOrganization == null))
                return typeOrg[0];
            else if (organization.TypeOrg == TypeOrg.ЛПУ)
                return typeOrg[1];
            else if (organization.TypeOrg == TypeOrg.Отделение)
                return typeOrg[2];
            else if ((organization.TypeOrg == TypeOrg.Аптека) && (organization.ParentOrganization != null))
                return typeOrg[3];
            else if (organization.TypeOrg == TypeOrg.Отдел)
                return typeOrg[4];
            else if (organization.TypeOrg == TypeOrg.Аптека)
                return typeOrg[5];
            else if (organization.TypeOrg == TypeOrg.Административное_Учреждение)
                return typeOrg[6];
            else if (organization.TypeOrg == TypeOrg.Дистрибьютор)
                return typeOrg[7];

            return typeOrg[8];
        }

        private string GetClientType(Organization organization)
        {
            if (organization.TypeOrg == TypeOrg.Отдел)
                return clientType[0];
            else if (organization.TypeOrg == TypeOrg.Административное_Учреждение)
                return clientType[1];
            else if (organization.TypeOrg == TypeOrg.Дистрибьютор)
                return clientType[2];

            return string.Empty;
        }
        
        private string GetAddressWithDistrict(Organization organization)
        {
            string district = string.Empty;
            string street = string.Empty;

            Organization mainOrganization = (organization.ParentOrganization == null) ? organization : organization.ParentOrganization;

            if (mainOrganization != null)
            {
                street = mainOrganization.Street;
            }
            
            return CheckIsEmpty(district, street);
        }

        private string CheckIsEmpty(string district, string street)
        {
            return string.IsNullOrEmpty(district) ? street : string.IsNullOrEmpty(street) ? district : district + ", " + street;
        }
    }
}
