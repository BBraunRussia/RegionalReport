using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public static class MyFile
    {
        public static bool Open(Files file)
        {
            if (File.Exists(file.ToString() + ".pdf"))
            {
                Process.Start(file.ToString() + ".pdf");
                return true;
            }

            return false;
        }
    }
}
