using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public static class SFFactory
    {
        public static BaseDictionary CreateItem(string tableName, DataRow row)
        {
            switch (tableName)
            {
                case "SF_LPU_Child":
                    return new LPUChild(row);
                case "SF_AdmLevel":
                    return new AdmLevel(row);
                case "SF_City":
                    return new City(row);
                case "SF_District":
                    return new District(row);
                case "SF_MainSpec":
                    return new MainSpec(row);
                case "SF_Ownership":
                    return new Ownership(row);
                case "SF_TypeLPU":
                    return new TypeLPU(row);
                case "SF_RealRegion":
                    return new RealRegion(row);
                case "SF_LpuRR":
                    return new LpuRR(row);
                case "SF_Role":
                    return new Role(row);
                case "SF_RegionRR":
                    return new RegionRR(row);
                case "SF_User":
                    return new User(row);
                case "SF_UserLpuRR":
                    return new UserLpuRR(row);
                default:
                    throw new NotImplementedException("Фабрика не может создать экземпляр данного класса");
            }
        }
    }
}
