using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class UserRight : BaseDictionary
    {
        private RegionRR _regionRR;
        private User _user;

        public UserRight(DataRow row)
            : base(row)
        {
            int idRegionRR;
            int.TryParse(row[1].ToString(), out idRegionRR);

            RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
            _regionRR = regionRRList.GetItem(idRegionRR) as RegionRR;
            
            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(ID) as User;
        }

        public RegionRR RegionRR { get { return _regionRR; } }
        public User User { get { return _user; } }
    }
}
