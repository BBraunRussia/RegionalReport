using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.SF.Models;

namespace RegionR.SF
{
    class ControlEditMode
    {
        private bool _editMode;
        public event EventHandler<EditModeEventArgs> EditModeChanged;
        private Control.ControlCollection _controls;
        private Button _btnClose;

        public ControlEditMode(Control.ControlCollection controls, Button btnClose)
        {
            _controls = controls;
            _btnClose = btnClose;
            EditModeChanged += DefaulEditModeChanged;

            GetSettings();
        }

        protected virtual void OnEditModeChanged(EditModeEventArgs e)
        {
            EventHandler<EditModeEventArgs> temp = EditModeChanged;

            if (temp != null)
                temp(this, e);
        }

        private void GetSettings()
        {
            Settings settings = new Settings();
            if (UserLogged.Get().RoleSF == RolesSF.Администратор)
                SetEditMode(true);
            else
                SetEditMode(settings.GetEditMode());
        }

        private void DefaulEditModeChanged(Object sender, EditModeEventArgs e)
        {
            SetEnable(_controls);
            SetEnableValue(_btnClose, true);
        }

        public void SetEnable(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is DataGridView)
                    continue;

                if (control.Controls.Count > 0)
                    SetEnable(control.Controls);
                else
                    SetEnableValue(control, _editMode);
            }
        }

        public void SetEnableValue(Control control, bool value)
        {
            if ((control is DataGridView) || (control is Label))
                control.Enabled = true;
            else if (control is TextBoxBase)
                SetReadlyOnlyValue(control as TextBoxBase, !value);
            else
                control.Enabled = value;
        }

        private void SetReadlyOnlyValue(TextBoxBase textbox, bool value)
        {
            textbox.ReadOnly = value;
        }

        public bool IsEditMode()
        {
            return _editMode;
        }

        public void SetEditMode(bool enabled)
        {
            _editMode = enabled;

            EditModeEventArgs e = new EditModeEventArgs(enabled);

            OnEditModeChanged(e);
        }
    }

    public class EditModeEventArgs : EventArgs
    {
        private readonly bool _enabled;

        public EditModeEventArgs(bool enabled)
        {
            _enabled = enabled;
        }

        public bool Enabled { get { return _enabled; } }
    }
}
