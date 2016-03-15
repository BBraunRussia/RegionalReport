using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegionR.SF
{
    public static class Phone
    {
        public static int GetPhoneLenght(string phoneCode)
        {
            return (phoneCode == string.Empty) ? 10 : 10 - phoneCode.Length;
        }
    }
}
