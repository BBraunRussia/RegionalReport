using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace ClassLibraryBBAuto
{
    public class WordDoc : OfficeDoc, IDisposable
    {
        private Word.Application wordApp;
        private Word.Document wordDoc;

        public WordDoc(string name) :
            base(name)
        {
            Init();
        }

        private void Init()
        {
            wordApp = new Word.Application();
            wordDoc = wordApp.Documents.Open(name);
        }

        public void Show()
        {
            wordApp.Visible = true;
        }

        public void Dispose()
        {
            //wordApp.Quit();

            releaseObject(wordDoc);
            releaseObject(wordApp);
        }

        public void setValue(string search, string replace)
        {
            Word.Range myRange;
            object wMissing = Type.Missing;
            object textToFind = search;
            object replaceWith = replace;
            object replaceType = Word.WdReplace.wdReplaceAll;

            for (int i = 1; i <= wordApp.ActiveDocument.Sections.Count; i++)
            {
                myRange = wordDoc.Sections[i].Range;

                myRange.Find.Execute(ref textToFind, ref wMissing, ref wMissing, ref wMissing, ref wMissing, ref wMissing, ref wMissing, ref wMissing, ref wMissing,
                    ref replaceWith, ref replaceType, ref wMissing, ref wMissing, ref wMissing, ref wMissing);
            }
        }
    }

    public class ExcelDoc : OfficeDoc, IDisposable
    {
        private Excel.Application xlApp;
        private Excel.Workbook xlWorkBook;
        private Excel.Worksheet xlSh;

        public ExcelDoc(string name)
            : base(name)
        {
            Init();
        }

        private void Init()
        {
            xlApp = new Excel.Application();

            xlWorkBook = xlApp.Workbooks.Open(name, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        }

        public void setValue(int rowIndex, int columnIndex, string value)
        {
            xlSh.Cells[rowIndex, columnIndex] = value;
        }

        public object getValue(string rowCell, string columnCell)
        {
            return xlSh.get_Range(rowCell, columnCell).Value2;
        }

        public void Show()
        {
            xlApp.Visible = true;
        }

        public void Dispose()
        {
            xlApp.Quit();

            releaseObject(xlSh);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }
    }

    public class OfficeDoc
    {
        protected string name;

        protected OfficeDoc(string name)
        {
            this.name = name;
        }

        protected void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
