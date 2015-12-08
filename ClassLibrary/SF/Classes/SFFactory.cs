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
                case "SF_LPU":
                    return new LPU(row);
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
                default:
                    throw new NotImplementedException("Фабрика не может создать экземпляр данного класса");
            }
        }
    }
}
