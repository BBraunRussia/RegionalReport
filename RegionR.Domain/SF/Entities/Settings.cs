using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF.Entities
{
    public class Settings : InitProvider
    {
        public bool GetEditMode()
        {
            int idEditMode;
            int.TryParse(_provider.SelectOne("SF_Settings"), out idEditMode);

            return Convert.ToBoolean(idEditMode);
        }

        public void SetEditMode(bool editMode)
        {
            _provider.Insert("SF_Settings", Convert.ToInt32(editMode));
        }
    }
}
