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
            
            using (StreamWriter stream = File.AppendText(FILENAME))
            {
                stream.WriteLine(string.Concat(DateTime.Now.ToString(), " ", message));
            }
        }

        public static void WriteNotFound(string name, string nameHeader, string numberSF)
        {
            Write(string.Format("{0} не найден: \"{1}\", SFNumber: {2}", nameHeader, name, numberSF));
        }
    }
}
