﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF.Models
{
    public class AdmLevel : BaseDictionary
    {
        public AdmLevel(DataRow row)
            : base(row)
        { }
    }
}