using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class HistoryOrganization : BaseDictionary
    {
        private Organization _organization;
        private User _user;
        private Action _action;
        private DateTime _datetime;

        public HistoryOrganization(DataRow row)
            : base(row)
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            _organization = organizationList.GetItem(ID);

            int idUser;
            int.TryParse(row[2].ToString(), out idUser);
            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(idUser) as User;

            int idAction;
            int.TryParse(row[3].ToString(), out idAction);
            _action = (Action)idAction;

            DateTime.TryParse(row[4].ToString(), out _datetime);
        }

        public static void Create(Organization organization, User user, Action action)
        {
            HistoryOrganizationList historyOrganizationList = HistoryOrganizationList.GetUniqueInstance();
            historyOrganizationList.Reload();

            HistoryOrganization historyOrganization;

            if (action == Action.Удалил)
            {
                historyOrganization = new HistoryOrganization(organization, user);
                historyOrganization._action = action;
            }
            else
            {
                var list = historyOrganizationList.GetList(organization);
                
                if (list.Count == 2)
                {
                    historyOrganization = historyOrganizationList.GetItem(organization, Action.Редактировал);
                    historyOrganization._user = user;
                }
                else
                {
                    historyOrganization = new HistoryOrganization(organization, user);
                    historyOrganization._action = (list.Count == 0) ? Action.Создал : Action.Редактировал;
                }
            }

            historyOrganization._datetime = DateTime.Now;

            historyOrganization.Save();

            historyOrganizationList.Add(historyOrganization);
        }

        private HistoryOrganization(Organization organization, User user)
        {
            _organization = organization;
            _user = user;
        }

        public Organization Organization { get { return _organization; } }
        public Action Action { get { return _action; } }
                
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_action);
            sb.Append(": ");
            sb.Append(_user.Name);
            sb.Append(" ");
            sb.Append(_datetime);

            return sb.ToString();
        }

        private void Save()
        {
            _provider.Insert("SF_HistoryOrganization", _organization.ID, _organization.ShortName, _user.ID, _action);
        }
    }
}
