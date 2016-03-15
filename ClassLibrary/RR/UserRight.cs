using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public class UserRight
    {
        private RegionRR _regionRR;
        private User _user;
        
        public UserRight(DataRow row)
        {
            int idUser;
            int.TryParse(row[0].ToString(), out idUser);
            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(idUser) as User;

            int idRegionRR;
            int.TryParse(row[1].ToString(), out idRegionRR);
            RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
            _regionRR = regionRRList.GetItem(idRegionRR) as RegionRR;
        }

        public RegionRR RegionRR { get { return _regionRR; } }
        public User User { get { return _user; } }
    }
}
