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

        private string[] _columnNamesEng = { "Person ID", "Institution ID", "Last name", "First Name", "Second Name", "Title", "Position", "Main speciality",
                                               "Academic title", "Email", "Moble phone", "Office phone", "Comment", "", "" };

        private string[] _columnNamesRus = { "№ персоны", "№ организации", "Фамилия", "Имя", "Отчество", "Обращение", "Должность", "Специализация", "Учёная степень/звание",
                                               "Эл. почта", "Телефон мобильный", "Телефон офисный", "Примечание", "Автор", "Редактировал" };

        private string[] _titleRus = { "Господин", "Госпожа" };
        private string[] _titleEng = { "Mr.", "Mrs." };

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
                string author = (lang == Language.Rus) ? historyList.GetItemString(person, HistoryAction.Создал) : string.Empty;
                string editor = (lang == Language.Rus) ? historyList.GetItemString(person, HistoryAction.Редактировал) : string.Empty;
                
                object[] row = { person.ID, person.Organization.ID, person.LastName, person.FirstName,
                               person.SecondName, GetTitle(lang, person.Appeal), person.Position.GetName(lang),
                               person.MainSpecPerson.GetName(lang), person.AcademTitle.GetName(lang),
                               person.Email, person.Mobile, person.Phone, person.Comment, author, editor };

                dt.Rows.Add(row);
            }

            CreateExcel excel = new CreateExcel(dt);
            excel.Show();

        }

        private string GetTitle(Language lang, int appeal)
        {
            return (lang == Language.Eng) ? _titleEng[appeal] : _titleRus[appeal];
        }

        public void ExportIDs(Language lang = Language.Eng)
        {
            PersonList personList = PersonList.GetUniqueInstance();

            string[] columnNames = (lang == Language.Rus) ? _columnNamesRus : _columnNamesEng;

            DataTable dt = CreateDataTable(columnNames);

            foreach (var person in personList.GetList())
            {
                object[] row = { person.ID, person.Organization.ID, person.LastName, person.FirstName,
                               person.SecondName, person.Appeal, person.Position.ID,
                               person.MainSpecPerson.ID, person.AcademTitle.ID,
                               person.Email, person.Mobile, person.Phone, person.Comment };

                dt.Rows.Add(row);
            }

            CreateExcel excel = new CreateExcel(dt);
            excel.Show();
        }

        private DataTable CreateDataTable(string[] columnNames)
        {
            DataTable dt = new DataTable();

            foreach (var item in columnNames)
                dt.Columns.Add(item);

            return dt;
        }
    }
}
