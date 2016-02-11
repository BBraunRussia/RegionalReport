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
        private SubRegionList _subRegionList;
        private TypeFinList _typeFinList;
        private HistoryList _historyList;
        private LpuRRList _lpuRRList;
        
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
            _subRegionList = SubRegionList.GetUniqueInstance();
            _typeFinList = TypeFinList.GetUniqueInstance();
            _historyList = HistoryList.GetUniqueInstance();
            _lpuRRList = LpuRRList.GetUniqueInstance();

            cbLpuRR.Enabled = (UserLogged.Get().RoleSF == RolesSF.Администратор);
        }

        private void FormAddLPU_Load(object sender, EventArgs e)
        {
            LoadDictionaries();

            LoadData();
        }

        private void LoadData()
        {
            this.Text = (_parentLPU == null) ? "Карточка Организации \"ЛПУ\"" : "Карточка Организации \"Филиал ЛПУ\"";
            lbKPP.Text = (_parentLPU == null) ? "КПП:" : "КПП*:";

            lbNumberSF.Text = (_lpu.NumberSF == string.Empty) ? "не присвоен" : _lpu.NumberSF;
            lbNumberLpuID.Text = (_lpu.ID == 0) ? "не присвоен" : _lpu.ID.ToString();
            lbTypeOrg.Text = _lpu.TypeOrg.ToString();

            if (_lpu.TypeLPU != null)
                cbTypeLpu.SelectedValue = _lpu.TypeLPU.ID;
            if (_lpu.Ownership != null)
                cbOwnership.SelectedValue = _lpu.Ownership.ID;
            if (_lpu.AdmLevel != null)
                cbAdmLevel.SelectedValue = _lpu.AdmLevel.ID;
            if (_lpu.MainSpec != null)
                cbMainSpec.SelectedValue = _lpu.MainSpec.ID;
            if (_lpu.TypeFin != null)
                cbTypeFin.SelectedValue = _lpu.TypeFin.ID;
                        
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
            }

            if (_parentLPU == null)
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

            if (_parentLPU == null)
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

            if (_lpu.SubRegion != null)
            {
                cbSubRegion.SelectedValue = _lpu.SubRegion.ID;
            }
            else
            {
                RealRegion realRegion = (_parentLPU == null) ? _lpu.RealRegion : _parentLPU.RealRegion;
                if (realRegion != null)
                    cbSubRegion.SelectedValue = _subRegionList.GetItem(realRegion).ID;
                else
                {
                    int idRealRegion;
                    int.TryParse(cbRealRegion.SelectedValue.ToString(), out idRealRegion);
                    if (idRealRegion != 0)
                    {
                        realRegion = _realRegionList.GetItem(idRealRegion) as RealRegion;
                        cbSubRegion.SelectedValue = _subRegionList.GetItem(realRegion).ID;
                    }
                }
            }
            
            tbStreet.Text = _lpu.Street;

            tbBedsTotal.Text = _lpu.BedsTotal;
            tbBedsIC.Text = _lpu.BedsIC;
            tbBedsSurgical.Text = _lpu.BedsSurgical;
            tbOperating.Text = _lpu.Operating;
            tbMachineGD.Text = _lpu.MachineGD;
            tbMachineGDF.Text = _lpu.MachineGDF;
            tbMachineCRRT.Text = _lpu.MachineCRRT;
            tbShift.Text = _lpu.Shift;
            tbPatientGD.Text = _lpu.PatientGD;
            tbPatientPD.Text = _lpu.PatientPD;
            tbPatientCRRT.Text = _lpu.PatientCRRT;

            LoadTree();

            SetPhoneCodeMask();

            ShowHistory();
        }

        private void ShowHistory()
        {
            lbAutor.Text = _historyList.GetItemString(_lpu, ClassLibrary.SF.Action.Создал);
            lbEditor.Text = _historyList.GetItemString(_lpu, ClassLibrary.SF.Action.Редактировал);
        }

        private void SetPhoneCodeMask()
        {
            if (tbPhoneCode.Text != string.Empty)
            {
                tbPhone.MaxLength = 10 - tbPhoneCode.Text.Length;
            }
        }

        private void LoadDictionaries()
        {
            ClassForForm.LoadDictionary(cbLpuRR, _lpuRRList.ToDataTable(_lpu.LpuRR));
            ClassForForm.LoadDictionary(cbTypeLpu, _typeLPUList.ToDataTable());
            ClassForForm.LoadDictionary(cbOwnership, _ownershipList.ToDataTable());
            ClassForForm.LoadDictionary(cbAdmLevel, _admLevelList.ToDataTable());
            ClassForForm.LoadDictionary(cbMainSpec, _mainSpecList.ToDataTable());
            ClassForForm.LoadDictionary(cbTypeFin, _typeFinList.ToDataTable());
            ClassForForm.LoadDictionary(cbSubRegion, _subRegionList.ToDataTable());
            _isLoad = false;

            if (UserLogged.Get().RoleSF == RolesSF.Администратор)
                ClassForForm.LoadDictionary(cbRealRegion, _realRegionList.ToDataTable());
            else
                ClassForForm.LoadDictionary(cbRealRegion, _realRegionList.ToDataTable(UserLogged.Get()));

            _isLoad = true;
            LoadCity();
        }

        private void cbRealRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoad)
            {
                LoadCity();
                ChangeSalesDistrict();
            }
        }

        private void LoadCity()
        {
            int idRealRegion = Convert.ToInt32(cbRealRegion.SelectedValue);
            RealRegion realRegion = _realRegionList.GetItem(idRealRegion) as RealRegion;

            DataTable dt = _cityList.ToDataTable(realRegion);
            ClassForForm.LoadDictionary(cbCity, dt);
        }

        private void ChangeSalesDistrict()
        {
            int idRealRegion;
            int.TryParse(cbRealRegion.SelectedValue.ToString(), out idRealRegion);
            RealRegion realRegion = _realRegionList.GetItem(idRealRegion) as RealRegion;
            cbSubRegion.SelectedValue = _subRegionList.GetItem(realRegion).ID;
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
                if (!IsHaveChanges())
                    return true;

                if (!CopyFields())
                    return false;
                
                _lpu.Save();

                History.Save(_lpu, UserLogged.Get());
                ShowHistory();

                return true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }
                
        private bool CopyFields()
        {
            int idTypeLPU = Convert.ToInt32(cbTypeLpu.SelectedValue);
            _lpu.TypeLPU = _typeLPUList.GetItem(idTypeLPU) as TypeLPU;

            int idOwnership = Convert.ToInt32(cbOwnership.SelectedValue);
            _lpu.Ownership = _ownershipList.GetItem(idOwnership) as Ownership;

            int idAdmLevel = Convert.ToInt32(cbAdmLevel.SelectedValue);
            _lpu.AdmLevel = _admLevelList.GetItem(idAdmLevel) as AdmLevel;

            int idMainSpec = Convert.ToInt32(cbMainSpec.SelectedValue);
            _lpu.MainSpec = _mainSpecList.GetItem(idMainSpec) as MainSpec;

            int idTypeFin = Convert.ToInt32(cbTypeFin.SelectedValue);
            _lpu.TypeFin = _typeFinList.GetItem(idTypeFin) as TypeFin;

            int idSubRegion = Convert.ToInt32(cbSubRegion.SelectedValue);
            _lpu.SubRegion = _subRegionList.GetItem(idSubRegion) as SubRegion;

            ClassForForm.CheckFilled(tbName.Text, "Официальное название");
            ClassForForm.CheckFilled(tbShortName.Text, "Сокращенное название");
            ClassForForm.CheckFilled(tbINN.Text, "ИНН");
            if (_parentLPU != null)
                ClassForForm.CheckFilled(tbKPP.Text, "КПП");

            ClassForForm.CheckFilled(tbStreet.Text, "Уличный адрес");

            ClassForForm.CheckINN(_lpu, tbINN.Text);

            _lpu.Name = tbName.Text;
            _lpu.ShortName = tbShortName.Text;

            int idCity = Convert.ToInt32(cbCity.SelectedValue);
            _lpu.City = _cityList.GetItem(idCity) as City;

            _lpu.INN = (_parentLPU == null) ? tbINN.Text : string.Empty;

            if (!_lpu.IsBelongsINNToRealRegion())
            {
                if (MessageBox.Show("ИНН организации принадлежит другому региону, продолжить сохранение?", "ИНН другого региона", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }
            }

            _lpu.KPP = tbKPP.Text;
            _lpu.PostIndex = tbPostIndex.Text;

            string email = _lpu.Email;
            _lpu.Email = tbEmail.Text;

            if (!ClassForForm.IsEmail(tbEmail.Text))
            {
                MessageBox.Show("Ошибка в электронном адресе. Пожалуйста, исправьте.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _lpu.Email = email;
                return false;
            }

            string website = _lpu.WebSite;
            _lpu.WebSite = tbWebSite.Text;

            if (!ClassForForm.IsWebSite(tbWebSite.Text))
            {
                MessageBox.Show("Ошибка в адресе веб-сайта. Пожалуйста, исправьте.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _lpu.WebSite = website;
                return false;
            }

            _lpu.Phone = tbPhone.Text;
            
            _lpu.District = tbDistrict.Text;
            _lpu.Street = tbStreet.Text;

            string bedsTotal, bedsIC, bedsSurgical;

            bedsTotal = _lpu.BedsTotal;
            bedsIC = _lpu.BedsIC;
            bedsSurgical = _lpu.BedsSurgical;

            _lpu.BedsTotal = tbBedsTotal.Text;
            _lpu.BedsIC = tbBedsIC.Text;
            _lpu.BedsSurgical = tbBedsSurgical.Text;

            if (_lpu.IsTotalLessThenSum())
            {
                MessageBox.Show("Общее количество коек меньше, чем сумма реанимационных и хирургических коек.\nПожалуйста, исправьте.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                _lpu.BedsTotal = bedsTotal;
                _lpu.BedsIC = bedsIC;
                _lpu.BedsSurgical = bedsSurgical;

                return false;
            }

            _lpu.Operating = tbOperating.Text;
            _lpu.MachineGD = tbMachineGD.Text;
            _lpu.MachineGDF = tbMachineGDF.Text;
            _lpu.MachineCRRT = tbMachineCRRT.Text;
            _lpu.Shift = tbShift.Text;
            _lpu.PatientGD = tbPatientGD.Text;
            _lpu.PatientPD = tbPatientPD.Text;
            _lpu.PatientCRRT = tbPatientCRRT.Text;

            if (cbLpuRR.Enabled)
            {
                int idLpuRR;
                int.TryParse(cbLpuRR.SelectedValue.ToString(), out idLpuRR);

                LpuRR lpuRR = _lpuRRList.GetItem(idLpuRR) as LpuRR;

                if (lpuRR != null)
                    _lpu.LpuRR = lpuRR;
            }

            return true;
        }
        
        private void btnShowRules_Click(object sender, EventArgs e)
        {
            if (!MyFile.Open(Files.rules_lpu))
                MessageBox.Show("Файл не найден", "Файл отсутствует", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void tbShortName_TextChanged(object sender, EventArgs e)
        {
            if (_parentLPU == null)
            {
                lbLPUName.Text = tbShortName.Text.ToUpper();
                lbBranchName.Visible = false;
                lbBranch.Visible = false;
            }
            else
            {
                lbLPUName.Text = _parentLPU.ShortName;
                lbBranchName.Text = _lpu.ShortName;
            }
        }

        private void btnAddSubOrganization_Click(object sender, EventArgs e)
        {
            if (TrySave())
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
            if ((node == null) || (node.BackColor == Color.Green))
            {
                return;
            }

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

            int idTypeFin = Convert.ToInt32(cbTypeFin.SelectedValue);
            if (_lpu.TypeFin != (_typeFinList.GetItem(idTypeFin) as TypeFin))
                return true;

            int idSubRegion = Convert.ToInt32(cbSubRegion.SelectedValue);
            if (_lpu.SubRegion != (_subRegionList.GetItem(idSubRegion) as SubRegion))
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

            if (cbLpuRR.Enabled)
            {
                int idLpuRR;
                int.TryParse(cbLpuRR.SelectedValue.ToString(), out idLpuRR);

                LpuRR lpuRR = _lpuRRList.GetItem(idLpuRR) as LpuRR;

                if (lpuRR != null)
                    return true;
            }

            if (_lpu.District != tbDistrict.Text)
                return true;
            if (_lpu.Street != tbStreet.Text)
                return true;

            if (_lpu.BedsTotal != tbBedsTotal.Text)
                return true;
            if (_lpu.BedsIC != tbBedsIC.Text)
                return true;
            if (_lpu.BedsSurgical != tbBedsSurgical.Text)
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

            if ((_currentNode == null) || (_currentNode.Text == string.Empty))
            {
                return;
            }

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

        private void cbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCity;
            int.TryParse(cbCity.SelectedValue.ToString(), out idCity);

            if (idCity != 0)
            {
                City city = _cityList.GetItem(idCity) as City;
                tbPhoneCode.Text = city.PhoneCode;
            }

            SetPhoneCodeMask();
        }

        private void cbLpuRR_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idLpuRR;
            int.TryParse(cbLpuRR.SelectedValue.ToString(), out idLpuRR);

            LpuRR lpuRR = _lpuRRList.GetItem(idLpuRR) as LpuRR;

            if (lpuRR != null)
                lbRegionRR.Text = lpuRR.RegionRR.Name;
        }
    }
}
