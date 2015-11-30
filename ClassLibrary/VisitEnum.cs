using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class VisitEnum : IEnumerator
    {
        private List<Visit> visitList;
        private int position = -1;

        public VisitEnum(List<Visit> visitList)
        {
            this.visitList = visitList;
        }

        public bool MoveNext()
        {
            position++;
            return (position < visitList.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        Object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Visit Current
        {
            get
            {
                try
                {
                    return visitList[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
