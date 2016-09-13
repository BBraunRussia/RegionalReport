using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.Common;
using RegionReport.Domain;

namespace ClassLibrary.SF.Import
{
    internal class ReadFile : IEnumerable
    {
        private const char SPLITER = ';';
        private List<string[]> splitedLines;
        private string fileName;
        private int index;

        public DataTable dt { get; private set; }

        public ReadFile(ImportFileType importFileType)
        {
            SettingsImportSF settings = new SettingsImportSF();

            fileName = settings.GetFileName(importFileType);

            if (!File.Exists(fileName))
            {
                LogManager.Logger.Information("File " + importFileType.ToString() + " not found");
                return;
            }
            
            string[] lines = File.ReadAllLines(fileName);

            SplitLines(lines);
        }

        public void Move()
        {
            string path = Path.GetDirectoryName(fileName) + @"\processed";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string curDate = GetCurrentDateToString();

            File.Move(fileName, string.Concat(path, @"\", curDate, " ", Path.GetFileName(fileName)));
        }

        private string GetCurrentDateToString()
        {
            string month = DateTime.Today.Month.ToString();
            if (DateTime.Today.Month < 10)
                month = "0" + DateTime.Today.Month.ToString();

            string day = DateTime.Today.Day.ToString();
            if (DateTime.Today.Day < 10)
                day = "0" + DateTime.Today.Day.ToString();

            return DateTime.Today.Year + "." + month + "." + day;
        }

        private void SplitLines(string[] lines)
        {
            if (splitedLines != null)
                return;
            
            splitedLines = new List<string[]>();

            int count = lines[0].Split(',').Count();
            string[] columns = lines[0].Remove(lines[0].Count() - 1, 1).Remove(0, 1).Replace("\",\"", ";").Split(';');
            
            dt = new DataTable();

            foreach (string columnName in columns)
            {
                dt.Columns.Add(new DataColumn(columnName));
            }
            
            index = 1;
            while (index < lines.Count())
            {
                string[] line = SplitLine(lines, count);

                if (line != null)
                {
                    splitedLines.Add(line);
                    dt.Rows.Add(line);
                }

                index++;
            }
        }

        private string[] SplitLine(string[] lines, int count)
        {
            string[] newLine = GetSplitedLine(lines[index]);

            if (newLine.Count() != count)
            {
                StringBuilder sb = new StringBuilder();
                int endIndex = GetEndLineIndex(lines, count, index);
                while (index < endIndex)
                {
                    sb.Append(lines[index]);
                    index++;
                }

                index--;
                newLine = GetSplitedLine(sb.ToString());
            }

            if (newLine.Count() != count)
            {
                LogManager.Logger.Error("Can't recognize the string №{index} in the file {file}", (index + 1), Path.GetFileName(fileName));
                return null;
            }

            return newLine;
        }

        private string[] GetSplitedLine(string line)
        {
            try
            {
                //в файле разделитель запятая, заменяем её на точку с запятой
                string newLine = line.Remove(line.Count() - 1, 1).Remove(0, 1).Replace(";", ",").Replace("\",\"", ";");

                return newLine.Split(SPLITER);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new string[]{};
            }
        }

        private int GetEndLineIndex(string[] lines, int count, int currentIndex)
        {            
            string[] newLine;

            do
            {
                newLine = GetSplitedLine(lines[currentIndex]);
                currentIndex++;
            }
            while (newLine.Count() != count);

            return currentIndex - 1;
        }
        
        public IEnumerator GetEnumerator()
        {
            foreach (var item in dt.Rows)
            {
                yield return item;
            }
        }
    }
}
