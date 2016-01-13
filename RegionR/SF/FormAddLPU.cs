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
        public static TypeOrg typeOrg;

        private LPU _lpu;
        private LPU _parentLPU;

        private TypeLPUList _typeLPUList;
        private OwnershipList _ownershipList;
        private AdmLevelList _admLevelList;
        private MainSpecList _mainSpecList;
        private RealRegionList _realRegionList;
        private CityList _cityList;
        
        private bool _isLoad;

        private TreeNode _currentNode;
        
        public FormAddLPU(LPU lpu)
        {
            InitializeComponent();

            _lpu = lpu;

            if (_lpu.ParentOrganization != null)
                _parentLPU = (_lpu.ParentOrganization as LPU);

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
            this.Text = (_lpu.ParentOrganization == null) ? "Карточка Организации \"ЛПУ\"" : "Карточка Организации \"Филиал ЛПУ\"";
            lbKPP.Text = (_lpu.ParentOrganization == null) ? "КПП:" : "КПП*:";

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

            if (_lpu.ShortName == string.Empty)
            {
                lbBranchName.Text = string.Empty;
                lbLPUName.Text = string.Empty;
            }
            
            if (_lpu.LpuRR != null)
            {
                cbLpuRR.SelectedValue = _lpu.LpuRR.ID;
                lbRegionRR.Text = _lpu.LpuRR.RegionRR.Name;
                lbRegionRRSalesDistrict.Text = _lpu.LpuRR.RegionRR.SalesDistrict;
            }

            if (_lpu.ParentOrganization == null)
                tbINN.Text = _lpu.INN;
            else
            {
                tbINN.Text = _parentLPU.INN;
                tbINN.ReadOnly = true;
            }

            tbKPP.Text = _lpu.KPP;
            tbDistrict.Text = _lpu.District;
            tbPostIndex.Text = _lpu.PostIndex;
            tbEmail.Text = _lpu.Email;
            tbWebSite.Text = _lpu.WebSite;
            tbPhone.Text = _lpu.Phone;

            if (_lpu.ParentOrganization == null)
            {
                if (_lpu.RealRegion != null)
                    cbRealRegion.SelectedValue = _lpu.RealRegion.ID;

                if (_lpu.City != null)
                {
                    cbCity.SelectedValue = _lpu.City.ID;
                    tbPhoneCode.Text = _lpu.City.PhoneCode;
                }
            }
            else
            {
                cbRealRegion.SelectedValue = _parentLPU.RealRegion.ID;
                cbCity.SelectedValue = _parentLPU.City.ID;
                tbPhoneCode.Text = _parentLPU.City.PhoneCode;
            }
            
            tbStreet.Text = _lpu.Street;

            tbBedsTotal.Text = _lpu.BedsTotal;
            tbBedsIC.Text = _lpu.BedsIC;
            tbSurgical.Text = _lpu.Surgical;
            tbOperating.Text = _lpu.Operating;
            tbMachineGD.Text = _lpu.MachineGD;
            tbMachineGDF.Text = _lpu.MachineGDF;
            tbMachineCRRT.Text = _lpu.MachineCRRT;
            tbShift.Text = _lpu.Shift;
            tbPatientGD.Text = _lpu.PatientGD;
            tbPatientPD.Text = _lpu.PatientPD;
            tbPatientCRRT.Text = _lpu.PatientCRRT;

            LoadTree();
        }

        private void LoadDictionaries()
        {
            LpuRRList _lpuRRList = LpuRRList.GetUniqueInstance();

            ClassForForm.LoadDictionary(cbLpuRR, _lpuRRList.ToDataTable());
            ClassForForm.LoadDictionary(cbTypeLpu, _typeLPUList.ToDataTable());
            ClassForForm.LoadDictionary(cbOwnership, _ownershipList.ToDataTable());
            ClassForForm.LoadDictionary(cbAdmLevel, _admLevelList.ToDataTable());
            ClassForForm.LoadDictionary(cbMainSpec, _mainSpecList.ToDataTable());
            _isLoad = false;
            ClassForForm.LoadDictionary(cbRealRegion, _realRegionList.ToDataTable());
            _isLoad = true;
            LoadCity();
        }

        private void LoadCity()
        {
            int idRealRegion = Convert.ToInt32(cbRealRegion.SelectedValue);
            RealRegion realRegion = _realRegionList.GetItem(idRealRegion) as RealRegion;

            DataTable dt = _cityList.ToDataTable(realRegion);
            ClassForForm.LoadDictionary(cbCity, dt);
        }

        private void cbRealRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoad)
                LoadCity();
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            if (TrySave())
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private bool TrySave()
        {
            try
            {
                CopyFields();

                _lpu.Save();

                return true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
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

            ClassForForm.CheckFilled(tbName.Text, "Официальное название");
            ClassForForm.CheckFilled(tbShortName.Text, "Сокращенное название");
            ClassForForm.CheckFilled(tbINN.Text, "ИНН");
            if (_parentLPU != null)
                ClassForForm.CheckFilled(tbKPP.Text, "КПП");

            ClassForForm.CheckFilled(tbStreet.Text, "Уличный адрес");

            if ((tbINN.Text.Length != 10) && (tbINN.Text.Length != 12))
                throw new NullReferenceException("Поле ИНН должно содержать 10 или 12 цифр");

            if ((tbINN.Text != _lpu.INN) && (tbINN.Text.Length == 12))
            {
                if (MessageBox.Show("Данная организация является ИП?", "ИНН содержит 12 цифр", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    throw new NullReferenceException("Перед сохранением необходимо исправить поле ИНН");
            }

            _lpu.Name = tbName.Text;
            _lpu.ShortName = tbShortName.Text;
            _lpu.INN = (_parentLPU == null) ? tbINN.Text : string.Empty;
            _lpu.KPP = tbKPP.Text;
            _lpu.PostIndex = tbPostIndex.Text;
            _lpu.Email = tbEmail.Text;
            _lpu.WebSite = tbWebSite.Text;
            _lpu.Phone = tbPhone.Text;

            int idCity = Convert.ToInt32(cbCity.SelectedValue);
            _lpu.City = _cityList.GetItem(idCity) as City;

            _lpu.District = tbDistrict.Text;
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
        
        private void tbShortName_TextChanged(object sender, EventArgs e)
        {
            if (_lpu.ParentOrganization == null)
            {
                lbLPUName.Text = tbShortName.Text.ToUpper();
                lbBranchName.Visible = false;
                lbBranch.Visible = false;
            }
            else
            {
                lbLPUName.Text = _lpu.ParentOrganization.ShortName;
                lbBranchName.Text = _lpu.ShortName;
            }
        }

        private void btnAddSubOrganization_Click(object sender, EventArgs e)
        {
            FormAddBranch formAddBranch = new FormAddBranch(_parentLPU == null);
            if (formAddBranch.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Organization organization = Organization.CreateItem(typeOrg, _lpu);

                if (organization is LPU)
                    (organization as LPU).LpuRR = _lpu.LpuRR;

                ShowFormSubLPU(organization);
            }
        }
        
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OpenSubOrganization(e.Node);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSubOrganization(treeView1.SelectedNode);
        }

        private void OpenSubOrganization(TreeNode node)
        {
            if (node.BackColor == Color.Green)
                return;

            Organization organization = GetOrganization(node);

            ShowFormSubLPU(organization);
        }

        private Organization GetOrganization(TreeNode node)
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(Convert.ToInt32(node.Name));
        }

        private void ShowFormSubLPU(Organization organization)
        {
            bool result = false;

            if (organization is LPU)
            {
                FormAddLPU formAddLPU = new FormAddLPU(organization as LPU);
                result = (formAddLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK);
            }
            else
            {
                FormAddOrganization formAddOrganization = new FormAddOrganization(organization);
                result = (formAddOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK);
            }

            if (result)
                LoadTree();
        }

        private void LoadTree()
        {
            treeView1.Nodes.Clear();

            Organization current = _lpu;

            treeView1.Nodes.Add(TreeLPU.GetRoot(current));

            ImageList imageList = new ImageList();

            imageList.Images.Add(global::RegionR.Properties.Resources.file);
            imageList.Images.Add(global::RegionR.Properties.Resources.folder);

            treeView1.ImageList = imageList;
            
            TreeNode[] list = treeView1.Nodes.Find(_lpu.ID.ToString(), true);

            if (list.Count() > 0)
            {
                list.First().BackColor = Color.Green;
                list.First().Expand();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsHaveChanges())
            {
                if (MessageBox.Show("На форме имеются не сохранёные изменения, сохранить перед закрытием?", "Сохраненить изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (TrySave())
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        return;
                    }
                    else
                        return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private bool IsHaveChanges()
        {
            int idTypeLPU = Convert.ToInt32(cbTypeLpu.SelectedValue);
            if (_lpu.TypeLPU != (_typeLPUList.GetItem(idTypeLPU) as TypeLPU))
                return true;

            int idOwnership = Convert.ToInt32(cbOwnership.SelectedValue);
            if (_lpu.Ownership != (_ownershipList.GetItem(idOwnership) as Ownership))
                return true;

            int idAdmLevel = Convert.ToInt32(cbAdmLevel.SelectedValue);
            if (_lpu.AdmLevel != (_admLevelList.GetItem(idAdmLevel) as AdmLevel))
                return true;

            int idMainSpec = Convert.ToInt32(cbMainSpec.SelectedValue);
            if (_lpu.MainSpec != (_mainSpecList.GetItem(idMainSpec) as MainSpec))
                return true;
            
            if (_lpu.Name != tbName.Text)
                return true;
            if (_lpu.ShortName != tbShortName.Text)
                return true;

            if (_parentLPU == null)
            {
                if (_lpu.INN != tbINN.Text)
                    return true;
            }
            
            if (_lpu.KPP != tbKPP.Text)
                return true;
            if (_lpu.PostIndex != tbPostIndex.Text)
                return true;
            if (_lpu.Email != tbEmail.Text)
                return true;
            if (_lpu.WebSite != tbWebSite.Text)
                return true;
            if (_lpu.Phone != tbPhone.Text)
                return true;

            int idCity = Convert.ToInt32(cbCity.SelectedValue);
            if (_lpu.City != (_cityList.GetItem(idCity) as City))
                return true;

            if (_lpu.District != tbDistrict.Text)
                return true;
            if (_lpu.Street != tbStreet.Text)
                return true;

            if (_lpu.BedsTotal != tbBedsTotal.Text)
                return true;
            if (_lpu.BedsIC != tbBedsIC.Text)
                return true;
            if (_lpu.Surgical != tbSurgical.Text)
                return true;
            if (_lpu.Operating != tbOperating.Text)
                return true;
            if (_lpu.MachineGD != tbMachineGD.Text)
                return true;
            if (_lpu.MachineGDF != tbMachineGDF.Text)
                return true;
            if (_lpu.MachineCRRT != tbMachineCRRT.Text)
                return true;
            if (_lpu.Shift != tbShift.Text)
                return true;
            if (_lpu.PatientGD != tbPatientGD.Text)
                return true;
            if (_lpu.PatientPD != tbPatientPD.Text)
                return true;
            if (_lpu.PatientCRRT != tbPatientCRRT.Text)
                return true;

            return false;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (_lpu.ID == 0)
            {
                if (!TrySave())
                    return;
            }

            Person person = new Person();
            person.Organization = _lpu;

            FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(person);
            formSecondStepAddPerson.ShowDialog();
        }
        
        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
                return;

            e.Handled = !char.IsDigit(e.KeyChar);
        }

        private void btnShowPerson_Click(object sender, EventArgs e)
        {
            FormPersonList formPersonList = new FormPersonList(_lpu);
            formPersonList.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Organization organization = GetOrganization(_currentNode);

            if (ClassForForm.DeleteOrganization(organization))
                LoadTree();
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            treeView1.SelectedNode = _currentNode;

            conMenuTree.Items["deleteToolStripMenuItem"].Enabled = (_currentNode != treeView1.Nodes[0]);

            Organization organization = GetOrganization(_currentNode);
            conMenuTree.Items["expandToolStripMenuItem"].Enabled = (organization is LPU);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _currentNode = e.Node;
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentNode.Expand();
        }

        private void btnDeleteSubOrganization_Click(object sender, EventArgs e)
        {
            if ((treeView1.SelectedNode != null) && (treeView1.SelectedNode != treeView1.Nodes[0]))
            {
                Organization organization = GetOrganization(_currentNode);

                if (ClassForForm.DeleteOrganization(organization))
                    LoadTree();
            }
        }
    }
}
