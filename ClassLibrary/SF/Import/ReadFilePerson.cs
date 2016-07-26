using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.Common;

namespace ClassLibrary.SF.Import
{
    public class ReadFilePerson
    {
        private AcademTitleList academTitleList;
        private MainSpecPersonList mainSpecPersonList;
        private PositionList positionList;

        public ReadFilePerson()
        {
            academTitleList = AcademTitleList.GetUniqueInstance();
            mainSpecPersonList = MainSpecPersonList.GetUniqueInstance();
            positionList = PositionList.GetUniqueInstance();
        }

        public void Start()
        {
            ReadFile readFile = new ReadFile(ImportFileType.Person);

            if (readFile.dt == null)
            {
                return;
            }

            foreach (DataRow row in readFile)
            {
                Update(new PersonModel(row));
            }
        }

        private void Update(PersonModel model)
        {
            Person person = GetPerson(model);
            if (person == null)
            {
                Logger.Write("Не удалось создать персону " + model.NumberSF);
                return;
            }

            person.NumberSF = model.NumberSF;
            person.CrmID = model.CrmID;
            person.LastName = model.LastName;
            person.FirstName = model.FirstName;
            person.SecondName = model.SecondName;
            person.Appeal = model.Appeal;
            person.AcademTitle = GetItem(academTitleList, model.AcademTitle, "AcademTitle", model.NumberSF) as AcademTitle;
            person.MainSpecPerson = GetItem(mainSpecPersonList, model.MainSpecPerson, "MainSpecPerson", model.NumberSF) as MainSpecPerson;
            person.Position = GetItem(positionList, model.Position, "Position", model.NumberSF) as Position;
            person.Email = model.Email;
            person.Mobile = model.Mobile;
            person.Phone = model.Phone;
            person.Comment = model.Comment;
            person.Deleted = model.Deleted;

            person.Save();
        }

        private Person GetPerson(PersonModel model)
        {
            PersonList personList = PersonList.GetUniqueInstance();
            
            //create person if not find it
            return (personList.GetItem(model.NumberSF) as Person) ?? new Person();
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
    }
}
