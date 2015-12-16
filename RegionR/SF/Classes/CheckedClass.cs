using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary.SF;

namespace RegionR.SF
{
    public static class CheckedClass
    {
        public static void CheckFilled(string text, string fieldName)
        {
            if (text == string.Empty)
                throw new NullReferenceException(string.Concat("Не заполнено поле \"", fieldName, "\""));
        }
    }
}
