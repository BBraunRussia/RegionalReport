using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public class UserRightList : InitProvider
    {
        private static UserRightList _uniqueInstance;
        private List<UserRight> _list;
        private string _tableName;

        private UserRightList(string tableName)
        {
            _list = new List<UserRight>();

            _tableName = tableName;
            
            LoadFromDataBase();
        }

        private void LoadFromDataBase()
        {
            DataTable dt = _provider.Select(_tableName);

            foreach (DataRow row in dt.Rows)
            {
                UserRight userRight = new UserRight(row);

                Add(userRight);
            }
        }

        private void Add(UserRight userRight)
        {
            if (!_list.Exists(item => item == userRight))
                _list.Add(userRight);
        }

        public void Reload()
        {
            _list.Clear();

            LoadFromDataBase();
        }

        public static UserRightList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new UserRightList("SF_UserRight");

            return _uniqueInstance;
        }

        public List<RegionRR> ToList(User user)
        {
            return _list.Where(item => (item as UserRight).User == user).Select(item => (item as UserRight).RegionRR).ToList();
        }

        public bool IsInList(User user, RegionRR regionRR)
        {
            return _list.Exists(item => (item as UserRight).User == user && (item as UserRight).RegionRR == regionRR);
        }
    }
}
