using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;

namespace RegionR.SF
{
    public partial class FormAddLPU : Form
    {
        private LPU _lpu;

        private TypeLPUList _typeLPUList;
        private OwnershipList _ownershipList;
        private AdmLevelList _admLevelList;
        private MainSpecList _mainSpecList;
        private RealRegionList _realRegionList;
        private CityList _cityList;

        private bool _isLoad;
        
        public FormAddLPU(LPU lpu)
        {
            InitializeComponent();

            _lpu = lpu;

            _isLoad = false;

            _typeLPUList = TypeLPUList.GetUniqueInstance();
            _ownershipList = OwnershipList.GetUniqueInstance();
            _admLevelList = AdmLevelList.GetUniqueInstance();
            _mainSpecList = MainSpecList.GetUniqueInstance();
            _realRegionList = RealRegionList.GetUniqueInstance();
            _cityList = CityList.GetUniqueInstance();
        }

        private void FormAddLPU_Load(object sender, EventArgs e)
        {
            LoadDictionaries();

            LoadData();
        }

        private void LoadData()
        {
            lbNumberSF.Text = (_lpu.NumberSF == string.Empty) ? "не присвоен" : _lpu.NumberSF;
            lbTypeOrg.Text = _lpu.TypeOrg.ToString();

            if (_lpu.TypeLPU != null)
                cbTypeLpu.SelectedValue = _lpu.TypeLPU.ID;
            if (_lpu.Ownership != null)
                cbOwnership.SelectedValue = _lpu.Ownership.ID;
            if (_lpu.AdmLevel != null)
                cbAdmLevel.SelectedValue = _lpu.AdmLevel.ID;
            if (_lpu.MainSpec != null)
                cbMainSpec.SelectedValue = _lpu.MainSpec.ID;
            
            tbName.Text = _lpu.Name;
            tbShortName.Text = _lpu.ShortName;
            
            if (_lpu.LpuRR != null)
            {
                cbLpuRR.SelectedValue = _lpu.LpuRR.ID;
                lbRegionRR.Text = _lpu.LpuRR.RegionRR.Name;
                lbRegionRRSalesDistrict.Text = _lpu.LpuRR.RegionRR.SalesDistrict;
            }

            tbINN.Text = _lpu.INN;
            tbKPP.Text = _lpu.KPP;
            tbPostIndex.Text = _lpu.PostIndex;
            tbEmail.Text = _lpu.Email;
            tbWebSite.Text = _lpu.WebSite;
            tbPhone.Text = _lpu.Phone;
            
            if (_lpu.RealRegion != null)
                cbRealRegion.SelectedValue = _lpu.RealRegion.ID;

            if (_lpu.City != null)
            {
                cbCity.SelectedValue = _lpu.City.ID;
                tbPhoneCode.Text = _lpu.City.PhoneCode;
            }
            
            tbStreet.Text = _lpu.Street;

            tbBedsTotal.Text = _lpu.BedsTotal.ToString();
            tbBedsIC.Text = _lpu.BedsIC.ToString();
        }

        private void LoadDictionaries()
        {
            LpuRRList _lpuList = LpuRRList.GetUniqueInstance();

            LoadDictionary(cbLpuRR, _lpuList.ToDataTable());
            LoadDictionary(cbTypeLpu, _typeLPUList.ToDataTable());
            LoadDictionary(cbOwnership, _ownershipList.ToDataTable());
            LoadDictionary(cbAdmLevel, _admLevelList.ToDataTable());
            LoadDictionary(cbMainSpec, _mainSpecList.ToDataTable());
            _isLoad = false;
            LoadDictionary(cbRealRegion, _realRegionList.ToDataTable());
            _isLoad = true;
            LoadCity();
        }

        private void LoadCity()
        {
            int idRealRegion = Convert.ToInt32(cbRealRegion.SelectedValue);
            RealRegion realRegion = _realRegionList.GetItem(idRealRegion) as RealRegion;

            DataTable dt = _cityList.ToDataTable(realRegion);
            LoadDictionary(cbCity, dt);
        }

        private void LoadDictionary(ComboBox combo, DataTable dt)
        {
            combo.DataSource = dt;

            if (dt == null)
            {
                MessageBox.Show("Отсутствуют данные в зависимых ячейках", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            combo.ValueMember = dt.Columns[0].ColumnName;
            combo.DisplayMember = dt.Columns[1].ColumnName;
        }

        private void cbRealRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoad)
                LoadCity();
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            TrySave();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private void TrySave()
        {
            try
            {
                CopyFields();

                _lpu.Save();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Обязательное поле", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyFields()
        {
            int idTypeLPU = Convert.ToInt32(cbTypeLpu.SelectedValue);
            _lpu.TypeLPU = _typeLPUList.GetItem(idTypeLPU) as TypeLPU;

            int idOwnership = Convert.ToInt32(cbOwnership.SelectedValue);
            _lpu.Ownership = _ownershipList.GetItem(idOwnership) as Ownership;

            int idAdmLevel = Convert.ToInt32(cbAdmLevel.SelectedValue);
            _lpu.AdmLevel = _admLevelList.GetItem(idAdmLevel) as AdmLevel;

            int idMainSpec = Convert.ToInt32(cbMainSpec.SelectedValue);
            _lpu.MainSpec = _mainSpecList.GetItem(idMainSpec) as MainSpec;

            CheckedClass.CheckFilled(tbName.Text, "Официальное название");
            CheckedClass.CheckFilled(tbShortName.Text, "Сокращенное название");
            CheckedClass.CheckFilled(tbINN.Text, "ИНН");

            _lpu.Name = tbName.Text;
            _lpu.ShortName = tbShortName.Text;
            _lpu.INN = tbINN.Text;
            _lpu.KPP = tbKPP.Text;
            _lpu.PostIndex = tbPostIndex.Text;
            _lpu.Email = tbEmail.Text;
            _lpu.WebSite = tbWebSite.Text;
            _lpu.Phone = tbPhone.Text;

            int idCity = Convert.ToInt32(cbCity.SelectedValue);
            _lpu.City = _cityList.GetItem(idCity) as City;
            
            _lpu.Street = tbStreet.Text;


            _lpu.BedsTotal = tbBedsTotal.Text;
            _lpu.BedsIC = tbBedsIC.Text;
            _lpu.Surgical = tbSurgical.Text;
            _lpu.Operating = tbOperating.Text;
            _lpu.MachineGD = tbMachineGD.Text;
            _lpu.MachineGDF = tbMachineGDF.Text;
            _lpu.MachineCRRT = tbMachineCRRT.Text;
            _lpu.Shift = tbShift.Text;
            _lpu.PatientGD = tbPatientGD.Text;
            _lpu.PatientPD = tbPatientPD.Text;
            _lpu.PatientCRRT = tbPatientCRRT.Text;
        }
        
        private void btnShowRules_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnShowEmployees_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddSubOrganization_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbShortName_TextChanged(object sender, EventArgs e)
        {
            if (_lpu.ParentLpu == null)
            {
                lbName.Text = tbShortName.Text.ToUpper();
                lbBranch.Text = string.Empty;
            }
            else
            {
                lbName.Text = _lpu.ParentLpu.ShortName;
                lbBranch.Text = tbShortName.Text.ToUpper();
            }
        }
    }
}
