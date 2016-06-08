using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClassLibrary.Common
{
    public class SettingsImportSF
    {
        private const string SETTINGS_FILENAME = "settings.ini";

        private string[] fileNames;

        public SettingsImportSF()
        {
            fileNames = new string[3];

            GetLines();
        }

        private void GetLines()
        {
            if (!File.Exists(SETTINGS_FILENAME))
            {
                throw new NullReferenceException("Отсутствует файл settings.ini");
            }
            
            string[] lines = File.ReadAllLines(SETTINGS_FILENAME);

            if (lines.Count() < 3)
            {
                throw new NullReferenceException("Файл settings.ini поверждён");
            }

            try
            {
                fileNames[(int)ImportFileType.Organization] = lines[0].Split('=')[1];
                fileNames[(int)ImportFileType.Person] = lines[1].Split('=')[1];
                fileNames[(int)ImportFileType.Relationship] = lines[2].Split('=')[1];
            }
            catch (IndexOutOfRangeException)
            {
                throw new NullReferenceException("Файл settings.ini поверждён, проверьте наличие разделителя");
            }
        }

        public string GetFileName(ImportFileType importFileType)
        {
            return fileNames[(int)importFileType];
        }
    }
}
