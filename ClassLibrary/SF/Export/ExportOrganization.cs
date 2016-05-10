using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public enum RecordType { RU_Institution, RU_Department, RU_Pharmacy, RU_Other, RU_Buying_institution }
    
    public class ExportOrganization
    {
        private const int CRM_ID = 0;

        private string[] _columnNamesEng = { "RR ID", "RR Parent ID", "Record type", "Institution Sub-Type", "Full name", "Short name", "TIN", "CRR",
            "State/Province", "City", "Post code", "Address", "E-mail", "Web site", "Phone number", "Customer Classification", "Hospital type", "Ownership",
            "Administrative level", "Financing channel", "Main speciality", "Sales District", "Number of beds (Total)", "Number of ICU Beds",
            "Number of acute beds", "Number of surgical rooms", "Number of HD machines", "Number of HDF machines", "Number of CRRT machines", "Number of shifts",
            "Number of HD patients", "Number of PD patients", "Number of CRRT patients" };

        private string[] _columnNamesRus = { "ID", "Parent ID", "CRM ID", "Тип записи", "Тип организации", "Client type", "Официальное название организации", "Сокращённое название",
            "ИНН", "КПП", "Регион России", "Город", "Индекс", "Район", "Уличный адрес", "Адрес эл. почты", "Веб-сайт", "Телефонный код города", "Телефонный номер",
            "Категория коммерческой аптеки", "Тип ЛПУ", "Форма собственности", "Административное подчинение", "Тип финансирования", "Основная специализация",
            "Sales District", "Номер ЛПУ-RR", "Номер ЛПУ-RR2", "Кол-во коек общее", "Кол-во коек реанимационных", "Кол-во коек хирургических", "Кол-во операционных", "Кол-во ГД машин",
            "Кол-во ГДФ машин", "Кол-во CRRT машин", "Кол-во смен", "Кол-во ГД пациентов", "Кол-во ПД пациентов", "Кол-во CRRT пациентов",
            "Создал", "Дата и время создания", "Изменил", "Дата и время изменения" };
        
        private string[] _typeOrgRus = {"ЛПУ", "Филиал ЛПУ", "Отделение ЛПУ", "Аптека ЛПУ", "Отдел ЛПУ", "Аптека коммерческая", "Административное учреждение",
                                       "Дистрибьютор", "Дистрибьютор (пока не покупал)"};
        private string[] _typeOrgEng = {"Hospital", "Hospital branch", "Hospital department (medical)", "Hospital pharmacy", "Hospital department (non-medical)",
                                       "Pharmacy", "Governmental-administrative establishment", "Distributor (Buying)", "Distributor (Non-Buying)"};

        private string[] _clientType = { "Department (purchasing dep., etc)", "Governmental-administrative establishment", "Dealer" };

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

                IHaveRegion mainOrganization = (organization is IHaveRegion) ? (organization as IHaveRegion) : null;
                LPU lpu = (organization.TypeOrg == TypeOrg.ЛПУ) ? (organization as LPU) : null;

                int parentID = (organization.ParentOrganization == null) ? organization.ID : organization.ParentOrganization.ID;
                string recordType = GetRecordType(organization);

                string inn = string.Empty;
                string kpp = string.Empty;
                string realRegionName = string.Empty;
                string city = string.Empty;
                string postIndex = string.Empty;
                

                if (mainOrganization != null)
                {
                    inn = mainOrganization.INN;
                    kpp = mainOrganization.KPP;
                    realRegionName = mainOrganization.RealRegion.Name;
                    city = mainOrganization.City.Name;
                    postIndex = mainOrganization.PostIndex;
                }

                string pharmacy = ((organization.TypeOrg == TypeOrg.Аптека) && (organization.ParentOrganization == null)) ? (organization as OtherOrganization).Pharmacy : string.Empty;
                string mainSpec = (organization.MainSpec != null) ? organization.MainSpec.GetName(lang) : string.Empty;
                string phoneCode = (mainOrganization != null) ? mainOrganization.City.PhoneCode : (organization.ParentOrganization as IHaveRegion).City.PhoneCode;
                
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
                    string district = string.Empty;
                    string street = string.Empty;

                    if (mainOrganization != null)
                    {
                        district = mainOrganization.District;
                        street = mainOrganization.Street;
                    }

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

                    row = new object[] { organization.ID, parentID, CRM_ID, recordType, GetFormatTypeOrg(organization), GetClientType(organization),
                               organization.Name, organization.ShortName, inn, kpp, realRegionName, city, postIndex, district, street,
                               organization.Email, organization.WebSite, phoneCode, organization.Phone, pharmacy, typeLPU, ownership, adminLevel, typeFin, mainSpec,
                               subRegion, idLpuRR, idLpuRR2,
                               (lpu != null) ? lpu.BedsTotal : string.Empty, (lpu != null) ? lpu.BedsIC : string.Empty, (lpu != null) ? lpu.BedsSurgical : string.Empty,
                               (lpu != null) ? lpu.Operating : string.Empty, (lpu != null) ? lpu.MachineGD : string.Empty, (lpu != null) ? lpu.MachineGDF : string.Empty,
                               (lpu != null) ? lpu.MachineCRRT : string.Empty, (lpu != null) ? lpu.Shift : string.Empty, (lpu != null) ? lpu.PatientGD : string.Empty,
                               (lpu != null) ? lpu.PatientPD : string.Empty, (lpu != null) ? lpu.PatientCRRT : string.Empty,
                               createdAuthor, createdDatetime, modifedAuthor, modifedDatetime };
                }
                else
                {
                    string phoneWithCode = (string.IsNullOrEmpty(organization.Phone)) ? string.Empty : string.Concat(phoneCode, organization.Phone);

                    row = new object[] { organization.ID, parentID, recordType, GetClientType(organization),
                               organization.Name, organization.ShortName, inn, kpp, realRegionName, city, postIndex, GetAddressWithDistrict(organization),
                               organization.Email, organization.WebSite, phoneWithCode, pharmacy, typeLPU, ownership, adminLevel, typeFin, mainSpec, subRegion,
                               (lpu != null) ? lpu.BedsTotal : string.Empty, (lpu != null) ? lpu.BedsIC : string.Empty, (lpu != null) ? lpu.BedsSurgical : string.Empty,
                               (lpu != null) ? lpu.Operating : string.Empty, (lpu != null) ? lpu.MachineGD : string.Empty, (lpu != null) ? lpu.MachineGDF : string.Empty,
                               (lpu != null) ? lpu.MachineCRRT : string.Empty, (lpu != null) ? lpu.Shift : string.Empty, (lpu != null) ? lpu.PatientGD : string.Empty,
                               (lpu != null) ? lpu.PatientPD : string.Empty, (lpu != null) ? lpu.PatientCRRT : string.Empty };
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
                return RecordType.RU_Institution.ToString();
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

        private int GetFormatTypeOrgID(Organization organization)
        {
            if ((organization.TypeOrg == TypeOrg.ЛПУ) && (organization.ParentOrganization == null))
                return 1;
            else if (organization.TypeOrg == TypeOrg.ЛПУ)
                return 2;
            else if (organization.TypeOrg == TypeOrg.Отделение)
                return 3;
            else if ((organization.TypeOrg == TypeOrg.Аптека) && (organization.ParentOrganization != null))
                return 4;
            else if (organization.TypeOrg == TypeOrg.Отдел)
                return 5;
            else if (organization.TypeOrg == TypeOrg.Аптека)
                return 6;
            else if (organization.TypeOrg == TypeOrg.Административное_Учреждение)
                return 7;
            else if (organization.TypeOrg == TypeOrg.Дистрибьютор)
                return 8;

            return 9;
        }

        private string GetClientType(Organization organization)
        {
            if (organization.TypeOrg == TypeOrg.Отдел)
                return _clientType[0];
            else if (organization.TypeOrg == TypeOrg.Административное_Учреждение)
                return _clientType[1];
            else if (organization.TypeOrg == TypeOrg.Дистрибьютор)
                return _clientType[2];

            return string.Empty;
        }

        private string GetAddressWithDistrict(Organization organization)
        {
            string district = string.Empty;
            string street = string.Empty;

            IHaveRegion mainOrganization = (organization is IHaveRegion) ? (organization as IHaveRegion) : (organization.ParentOrganization != null) ? organization.ParentOrganization as IHaveRegion : null;

            if (mainOrganization != null)
            {
                district = mainOrganization.District;
                street = mainOrganization.Street;
            }

            if (string.IsNullOrEmpty(district))
                return street;
            if (string.IsNullOrEmpty(street))
                return district;

            return district + ", " + street;
        }
    }
}
