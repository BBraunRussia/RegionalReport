using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.Common;

namespace ClassLibrary.SF.Import
{
    internal class ReadFile : IEnumerable
    {
        private const char SPLITER = ';';
        private List<string[]> splitedLines;
        private string fileName;

        public DataTable dt { get; private set; }

        public ReadFile(ImportFileType importFileType)
        {
            SettingsImportSF settings = new SettingsImportSF();

            fileName = settings.GetFileName(importFileType);

            if (!File.Exists(fileName))
                return;
            
            string[] lines = File.ReadAllLines(fileName);

            SplitLines(lines);

            MoveFile(fileName);
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
            
            for (int i = 1; i < lines.Count(); i++)
            {
                string[] line = SplitLine(lines[i], count, i);

                if (line != null)
                {
                    splitedLines.Add(line);
                    dt.Rows.Add(line);
                }
            }
        }

        private string[] SplitLine(string line, int count, int currentIndex)
        {
            //в файле разделитель запятая, заменяем её на точку с запятой
            string newLine = line.Remove(line.Count() - 1, 1).Remove(0, 1).Replace(";", ",").Replace("\",\"", ";");
            
            string[] tempLine = newLine.Split(SPLITER);
            
            if (tempLine.Count() != count)
            {
                Logger.Write(string.Concat("Не удалось распознать строку №", (currentIndex + 1), " в файле", Path.GetFileName(fileName)));
                return null;
            }
            
            return tempLine;
        }
        
        private void MoveFile(string fileName)
        {
            string path = Path.GetDirectoryName(fileName) + @"\processed";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.Move(fileName, string.Concat(path, @"\", DateTime.Today.ToShortDateString(), " ", Path.GetFileName(fileName)));
        }

        public IEnumerator GetEnumerator()
        {
            return new ReadFileEnumerator(this);
        }

        private class ReadFileEnumerator : IEnumerator
        {
            private ReadFile readFile;
            private int index;

            public ReadFileEnumerator(ReadFile readFile)
            {
                this.readFile = readFile;
                index = -1;
            }

            public object Current { get { return readFile.dt.Rows[index]; } }

            public bool MoveNext()
            {
                if (index + 1 < readFile.dt.Rows.Count)
                {
                    index++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                index = -1;
            }
        }
    }
}
