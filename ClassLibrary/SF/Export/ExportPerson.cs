using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class ExportPerson
    {
        private const int CRM_ID = 0;

        private string[] _columnNamesEng = { "Z_RU_RR_Person__c", "Z_RU_RR_Institution__c", "Last name", "First Name", "Middle Name", "Salutation",
                                               "Z_ES_Function__c", "Z_Major_Specialties_RU__pc", "Z_Cd_Academic_Title__pc", "Z_Email__c", "Z_Mobile_Phone__c",
                                               "Z_Tel_No__c", "Z_Description__c" };

        private string[] _columnNamesRus = { "№ персоны", "№ организации", "Фамилия", "Имя", "Отчество", "Обращение", "Организация", "Подразделение", "Должность",
                                               "Специализация", "Учёная степень/звание", "Эл. почта", "Телефон мобильный", "Телефон офисный", "Примечание",
                                               "Создал", "Дата и время создания", "Изменил", "Дата и время изменения" };

        private string[] _titleRus = { "Господин", "Госпожа" };
        private string[] _titleEng = { "1", "3" };

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
            PersonList personList = PersonList.GetUniqueInstance();

            string[] columnNames = (lang == Language.Rus) ? _columnNamesRus : _columnNamesEng;

            DataTable dt = CreateDataTable(columnNames);

            HistoryList historyList = HistoryList.GetUniqueInstance();
            
            foreach (var person in personList.GetList())
            {
                History created = historyList.GetItem(person, HistoryAction.Создал);
                History modifed = historyList.GetItem(person, HistoryAction.Редактировал);
                string modifedAuthor = (modifed != null) ? modifed.Author : string.Empty;
                string modifedDatetime = (modifed != null) ? modifed.datetime : string.Empty;

                object[] row;

                if (lang == Language.Rus)
                {
                    row = new object[] { person.ID, person.Organization.ID, person.LastName, person.FirstName,
                        person.SecondName, GetTitle(lang, person.Appeal), person.GetOrganizationName(), person.GetSubOrganizationName(),
                        person.Position.GetName(lang), person.MainSpecPerson.GetName(lang), person.AcademTitle.GetName(lang),
                        person.Email, person.Mobile, person.Phone, person.Comment, created.Author, created.datetime,
                        modifedAuthor, modifedDatetime };
                }
                else
                {
                    row = new object[] { person.ID, person.Organization.ID, person.LastName, person.FirstName, person.SecondName,
                        GetTitle(lang, person.Appeal), person.Position.GetName(lang), person.MainSpecPerson.GetName(lang),
                        person.AcademTitle.GetName(lang), person.Email, person.Mobile, GetPhoneWithCode(person), person.Comment };
                }

                dt.Rows.Add(row);
            }

            CreateExcel excel = new CreateExcel(dt);
            excel.Show();
        }

        private string GetTitle(Language lang, int appeal)
        {
            return (lang == Language.Eng) ? _titleEng[appeal] : _titleRus[appeal];
        }

        private DataTable CreateDataTable(string[] columnNames)
        {
            DataTable dt = new DataTable();

            foreach (var item in columnNames)
            {
                dt.Columns.Add(item);
            }

            return dt;
        }

        private string GetPhoneWithCode(Person person)
        {
            string phone = string.Empty;

            if (!string.IsNullOrEmpty(person.Phone))
            {
                IHaveRegion organization = ((person.Organization is IHaveRegion) ? person.Organization : person.Organization.ParentOrganization) as IHaveRegion;

                phone = organization.City.PhoneCode + person.Phone;
            }

            return phone;
        }
    }
}
