using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Interfaces;

namespace ClassLibrary.SF.Models
{
    public class History : BaseDictionary
    {
        private User _user;
        private DateTime _datetime;

        public History(DataRow row)
            : base(row)
        {
            int idType;
            int.TryParse(row[2].ToString(), out idType);
            Type = (HistoryType)idType;

            int idUser;
            int.TryParse(row[3].ToString(), out idUser);
            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(idUser) as User;

            int idAction;
            int.TryParse(row[4].ToString(), out idAction);
            Action = (HistoryAction)idAction;

            DateTime.TryParse(row[5].ToString(), out _datetime);
        }

        public static void Save(IHistory orgHistory, User user, HistoryAction action = HistoryAction.Создал)
        {
            HistoryList historyList = HistoryList.GetUniqueInstance();

            History history;
            
            if (action == HistoryAction.Удалил)
            {
                history = new History(orgHistory, user);
                history.Action = action;
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
                    history.Action = (list.Count == 0) ? HistoryAction.Создал : HistoryAction.Редактировал;
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
            Type = history.Type;
            _user = user;
        }

        public HistoryType Type { get; private set; }
        public HistoryAction Action { get; private set; }
        public string Author { get { return _user.Name; } }
        public string datetime { get { return _datetime.ToString(); } }
                
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Action);
            sb.Append(": ");
            sb.Append(_user.Name);
            sb.Append(" ");
            sb.Append(_datetime);

            return sb.ToString();
        }

        private void Save()
        {
            _provider.Insert("SF_History", ID, Name, Type, _user.ID, Action);
        }
    }
}
