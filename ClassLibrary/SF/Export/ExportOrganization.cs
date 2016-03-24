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

        private string[] _columnNamesEng = { "ID", "Parent ID", "CRM ID", "Record type", "Institution type", "Official name", "Short name", "INN", "KPP",
            "Russian Region", "District", "City", "Post code", "Address", "E-mail", "Web site", "City phone code", "Phone number", "Category", "Hospital type", "Ownership",
            "Administrative level", "Financing channel", "Main speciality", "Sales District", "LPU-RR id", "Number of beds total", "Number of beds resuscitation",
            "Number of surgical beds", "Number of operating", "Number of GD machines", "Number of GDF machines", "Number of CRRT machines", "Number of shifts",
            "Number of GD patients", "Number of PD patients", "Number of CRRT patients" };

        private string[] _columnNamesRus = { "ID", "Parent ID", "CRM ID", "Тип записи", "Тип организации", "Официальное название организации", "Сокращённое название",
            "ИНН", "КПП", "Регион России", "Район ", "Город", "Индекс", "Уличный адрес", "Адрес эл. почты", "Веб-сайт", "Телефонный код города", "Телефонный номер",
            "Категория коммерческой аптеки", "Тип ЛПУ", "Форма собственности", "Административное подчинение", "Тип финансирования", "Основная специализация",
            "Sales District", "Номер ЛПУ-RR", "Кол-во коек общее", "Кол-во коек реанимационных", "Кол-во коек хирургических", "Кол-во операционных", "Кол-во ГД машин",
            "Кол-во ГДФ машин", "Кол-во CRRT машин", "Кол-во смен", "Кол-во ГД пациентов", "Кол-во ПД пациентов", "Кол-во CRRT пациентов" };

        private string[] _typeOrgRus = {"ЛПУ", "Филиал ЛПУ", "Отделение ЛПУ", "Аптека ЛПУ", "Отдел ЛПУ", "Аптека коммерческая", "Административное учреждение",
                                       "Дистрибьютор", "Дистрибьютор (пока не покупал)"};
        private string[] _typeOrgEng = {"Hospital", "Hospital branch", "Hospital department (medical)", "Hospital pharmacy", "Hospital department (non-medical)",
                                       "Pharmacy", "Governmental-administrative establishment", "Distributor (Buying)", "Distributor (Non-Buying)"};

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

            foreach (var item in organizationList.List)
            {
                Organization organization = item.Value;

                IHaveRegion mainOrganization = (organization is IHaveRegion) ? (organization as IHaveRegion) : null;
                LPU lpu = (organization.TypeOrg == TypeOrg.ЛПУ) ? (organization as LPU) : null;

                int parentID = (organization.ParentOrganization == null) ? organization.ID : organization.ParentOrganization.ID;
                string recordType = GetRecordType(organization);
                string inn = (mainOrganization != null) ? mainOrganization.INN : string.Empty;
                string kpp = (mainOrganization != null) ? mainOrganization.KPP : string.Empty;
                string realRegionName = (mainOrganization != null) ? mainOrganization.RealRegion.Name : string.Empty;
                string distinct = (mainOrganization != null) ? mainOrganization.District : string.Empty;
                string city = (mainOrganization != null) ? mainOrganization.City.Name : string.Empty;
                string postIndex = (mainOrganization != null) ? mainOrganization.PostIndex : string.Empty;
                string street = (mainOrganization != null) ? mainOrganization.Street : string.Empty;
                string phoneCode = (mainOrganization != null) ? mainOrganization.City.PhoneCode : (organization.ParentOrganization as IHaveRegion).City.PhoneCode;
                string pharmacy = ((organization.TypeOrg == TypeOrg.Аптека) && (organization.ParentOrganization == null)) ? (organization as OtherOrganization).Pharmacy : string.Empty;
                string typeLPU = (lpu != null) ? lpu.TypeLPU.GetName(lang) : string.Empty;
                string ownership = (lpu != null) ? lpu.Ownership.GetName(lang) : string.Empty;
                string adminLevel = (lpu != null) ? lpu.AdmLevel.GetName(lang) : string.Empty;
                string typeFin = (lpu != null) ? lpu.TypeFin.GetName(lang) : string.Empty;
                string mainSpec = (organization.MainSpec != null) ? organization.MainSpec.GetName(lang) : string.Empty;
                string subRegion = (lpu != null) ? lpu.SubRegion.Name.Split(' ')[0] : string.Empty;
                string idLpuRR = ((lpu != null) && (lpu.ParentOrganization == null)) ? lpu.LpuRR.ID.ToString() : string.Empty;
                
                object[] row = { organization.ID, parentID, CRM_ID, recordType,
                               organization.TypeOrg.ToString(), organization.Name, organization.ShortName,
                               inn, kpp, realRegionName, distinct, city, postIndex, street, organization.Email, organization.WebSite,
                               phoneCode, organization.Phone, pharmacy, typeLPU, ownership, adminLevel, typeFin, mainSpec, subRegion, idLpuRR,
                               (lpu != null) ? lpu.BedsTotal : string.Empty, (lpu != null) ? lpu.BedsIC : string.Empty, (lpu != null) ? lpu.BedsSurgical : string.Empty,
                               (lpu != null) ? lpu.Operating : string.Empty, (lpu != null) ? lpu.MachineGD : string.Empty, (lpu != null) ? lpu.MachineGDF : string.Empty,
                               (lpu != null) ? lpu.MachineCRRT : string.Empty, (lpu != null) ? lpu.Shift : string.Empty, (lpu != null) ? lpu.PatientGD : string.Empty,
                               (lpu != null) ? lpu.PatientPD : string.Empty, (lpu != null) ? lpu.PatientCRRT : string.Empty
                                };

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

        public string GetFormatTypeOrgRus(Organization organization)
        {
            return GetFormatTypeOrg(organization, Language.Rus);
        }

        public string GetFormatTypeOrgEng(Organization organization)
        {
            return GetFormatTypeOrg(organization, Language.Eng);
        }

        private string GetFormatTypeOrg(Organization organization, Language lang)
        {
            string[] typeOrg = (lang == Language.Rus) ? _typeOrgRus : _typeOrgEng;

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
                return typeOrg[8];

            return typeOrg[9];
        }
    }
}
