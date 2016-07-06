using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClassLibrary.Common
{
    public static class Logger
    {
        private const string FILENAME = "log.ini";

        public static void Write(string message)
        {
            if (!File.Exists(FILENAME))
            {
                File.Create(FILENAME);
            }
            
            

            using (StreamWriter stream = File.AppendText(FILENAME)) //new StreamWriter(FILENAME))
            {
                stream.WriteLine(string.Concat(DateTime.Now.ToString(), " ", message));
            }
        }
    }
}
