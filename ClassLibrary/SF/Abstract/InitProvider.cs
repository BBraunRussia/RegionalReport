using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public abstract class InitProvider
    {
        protected IProvider _provider;

        public InitProvider()
        {
            _provider = Provider.GetProvider();
        }
    }
}
