using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;
using ClassLibrary.SF.Entities;

namespace ClassLibrary
{
    public static class Factory
    {
        public static BaseDictionary CreateItem(string tableName, DataRow row)
        {
            switch (tableName)
            {
                case "SF_AdmLevel":
                    return new AdmLevel(row);
                case "SF_MainSpec":
                    return new MainSpec(row);
                case "SF_Ownership":
                    return new Ownership(row);
                case "SF_TypeLPU":
                    return new TypeLPU(row);
                case "SF_RealRegion":
                    return new RealRegion(row);
                case "LPU":
                    return new LpuRR(row);
                case "SF_Role":
                    return new Role(row);
                case "SF_RegionRR":
                    return new RegionRR(row);
                case "SF_User":
                    return new User(row);
                case "SF_Person":
                    return new Person(row);
                case "SF_Position":
                    return new Position(row);
                case "SF_MainSpecPerson":
                    return new MainSpecPerson(row);
                case "SF_AcademTitle":
                    return new AcademTitle(row);
                case "SF_UserRoleSF":
                    return new UserRoleSF(row);
                case "SF_SubRegion":
                    return new SubRegion(row);
                case "SF_TypeFin":
                    return new TypeFin(row);
                case "UserLpu":
                    return new UserLpuRR(row);
                default:
                    throw new NotImplementedException("Фабрика не может создать экземпляр данного класса");
            }
        }
    }
}
