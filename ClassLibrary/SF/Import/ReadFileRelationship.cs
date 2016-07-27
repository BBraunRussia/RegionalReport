using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Models;

namespace ClassLibrary.SF.Import
{
    public class ReadFileRelationship
    {
        public void Start()
        {
            ReadFile readFile = new ReadFile(ImportFileType.Relationship);

            if (readFile.dt == null)
                return;

            foreach (DataRow row in readFile)
            {
                Update(new RelationshipModel(row));
            }
        }

        private void Update(RelationshipModel model)
        {
            //Удалили связь персоны с ЛПУ
            if (model.Deleted)
                return;

            Person person = GetPerson(model);
            if (person == null)
                return;

            person.Organization = GetOrganization(model);
            
            person.Save();
        }

        private Person GetPerson(RelationshipModel model)
        {
            PersonList personList = PersonList.GetUniqueInstance();
            return personList.GetItem(model.PersonNumberSF);
        }

        private Organization GetOrganization(RelationshipModel model)
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(model.OrganizationNumberSF);
        }
    }
}
