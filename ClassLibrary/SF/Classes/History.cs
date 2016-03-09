using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class History : BaseDictionary
    {
        private HistoryType _type;

        private User _user;
        private HistoryAction _action;
        private DateTime _datetime;

        public History(DataRow row)
            : base(row)
        {
            int idType;
            int.TryParse(row[2].ToString(), out idType);
            _type = (HistoryType)idType;

            int idUser;
            int.TryParse(row[3].ToString(), out idUser);
            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(idUser) as User;

            int idAction;
            int.TryParse(row[4].ToString(), out idAction);
            _action = (HistoryAction)idAction;

            DateTime.TryParse(row[5].ToString(), out _datetime);
        }

        public static void Save(IHistory orgHistory, User user, HistoryAction action = HistoryAction.Создал)
        {
            HistoryList historyList = HistoryList.GetUniqueInstance();

            History history;
            
            if (action == HistoryAction.Удалил)
            {
                history = new History(orgHistory, user);
                history._action = action;
            }
            else
            {
                var list = historyList.GetList(orgHistory);
                
                if (list.Count == 2)
                {
                    history = historyList.GetItem(orgHistory, HistoryAction.Редактировал);
                    history._user = user;
                }
                else
                {
                    history = new History(orgHistory, user);
                    history._action = (list.Count == 0) ? HistoryAction.Создал : HistoryAction.Редактировал;
                }
            }

            history._datetime = DateTime.Now;

            history.Save();

            historyList.Add(history);
        }

        private History(IHistory history, User user)
        {
            ID = history.ID;
            Name = history.ShortName;
            _type = history.Type;
            _user = user;
        }

        public HistoryType Type { get { return _type; } }
        public HistoryAction Action { get { return _action; } }
                
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
            _provider.Insert("SF_History", ID, Name, _type, _user.ID, _action);
        }
    }
}
