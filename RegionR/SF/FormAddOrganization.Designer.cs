namespace RegionR.SF
{
    partial class FormAddOrganization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbBranch = new System.Windows.Forms.Label();
            this.lbLPU = new System.Windows.Forms.Label();
            this.tbShortName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cbMainSpec = new System.Windows.Forms.ComboBox();
            this.lbMainSpec = new System.Windows.Forms.Label();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.tbWebSite = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.lbTypeOrg = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbCrmID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.btnShowEmployees = new System.Windows.Forms.Button();
            this.btnAddEmployee = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbTypeOrgName = new System.Windows.Forms.Label();
            this.btnShowRules = new System.Windows.Forms.Button();
            this.lbEditor = new System.Windows.Forms.Label();
            this.lbAutor = new System.Windows.Forms.Label();
            this.lbID = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.gbAvitum = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbPatientCRRT = new System.Windows.Forms.TextBox();
            this.tbMachineGD = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbPatientPD = new System.Windows.Forms.TextBox();
            this.tbMachineGDF = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbPatientGD = new System.Windows.Forms.TextBox();
            this.tbMachineCRRT = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tbShift = new System.Windows.Forms.TextBox();
            this.gbAvitum.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbBranch
            // 
            this.lbBranch.AutoSize = true;
            this.lbBranch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbBranch.Location = new System.Drawing.Point(81, 32);
            this.lbBranch.Name = "lbBranch";
            this.lbBranch.Size = new System.Drawing.Size(85, 18);
            this.lbBranch.TabIndex = 85;
            this.lbBranch.Text = "Отделение";
            // 
            // lbLPU
            // 
            this.lbLPU.AutoSize = true;
            this.lbLPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbLPU.Location = new System.Drawing.Point(68, 10);
            this.lbLPU.Name = "lbLPU";
            this.lbLPU.Size = new System.Drawing.Size(43, 20);
            this.lbLPU.TabIndex = 84;
            this.lbLPU.Text = "ЛПУ";
            // 
            // tbShortName
            // 
            this.tbShortName.Location = new System.Drawing.Point(147, 159);
            this.tbShortName.Name = "tbShortName";
            this.tbShortName.Size = new System.Drawing.Size(328, 20);
            this.tbShortName.TabIndex = 89;
            this.tbShortName.TextChanged += new System.EventHandler(this.tbShortName_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(10, 162);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(135, 13);
            this.label20.TabIndex = 88;
            this.label20.Text = "Сокращенное название*:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(147, 131);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(483, 20);
            this.tbName.TabIndex = 87;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 134);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(135, 13);
            this.label19.TabIndex = 86;
            this.label19.Text = "Официальное название*:";
            // 
            // cbMainSpec
            // 
            this.cbMainSpec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMainSpec.FormattingEnabled = true;
            this.cbMainSpec.Location = new System.Drawing.Point(146, 263);
            this.cbMainSpec.Name = "cbMainSpec";
            this.cbMainSpec.Size = new System.Drawing.Size(193, 21);
            this.cbMainSpec.TabIndex = 92;
            // 
            // lbMainSpec
            // 
            this.lbMainSpec.AutoSize = true;
            this.lbMainSpec.Location = new System.Drawing.Point(9, 266);
            this.lbMainSpec.Name = "lbMainSpec";
            this.lbMainSpec.Size = new System.Drawing.Size(107, 13);
            this.lbMainSpec.TabIndex = 91;
            this.lbMainSpec.Text = "Основной профиль:";
            // 
            // tbPhone
            // 
            this.tbPhone.Location = new System.Drawing.Point(146, 237);
            this.tbPhone.MaxLength = 12;
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(193, 20);
            this.tbPhone.TabIndex = 99;
            this.tbPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPhone_KeyPress);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(7, 240);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(96, 13);
            this.label25.TabIndex = 98;
            this.label25.Text = "Номер телефона:";
            // 
            // tbWebSite
            // 
            this.tbWebSite.Location = new System.Drawing.Point(147, 211);
            this.tbWebSite.Name = "tbWebSite";
            this.tbWebSite.Size = new System.Drawing.Size(192, 20);
            this.tbWebSite.TabIndex = 96;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(10, 214);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(54, 13);
            this.label26.TabIndex = 95;
            this.label26.Text = "веб-сайт:";
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(147, 185);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(192, 20);
            this.tbEmail.TabIndex = 94;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(10, 188);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(58, 13);
            this.label27.TabIndex = 93;
            this.label27.Text = "эл. адрес:";
            // 
            // lbTypeOrg
            // 
            this.lbTypeOrg.AutoSize = true;
            this.lbTypeOrg.Location = new System.Drawing.Point(143, 107);
            this.lbTypeOrg.Name = "lbTypeOrg";
            this.lbTypeOrg.Size = new System.Drawing.Size(31, 13);
            this.lbTypeOrg.TabIndex = 104;
            this.lbTypeOrg.Text = "ЛПУ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 103;
            this.label4.Text = "Тип организации:";
            // 
            // lbCrmID
            // 
            this.lbCrmID.AutoSize = true;
            this.lbCrmID.Location = new System.Drawing.Point(143, 59);
            this.lbCrmID.Name = "lbCrmID";
            this.lbCrmID.Size = new System.Drawing.Size(70, 13);
            this.lbCrmID.TabIndex = 102;
            this.lbCrmID.Text = "не присвоен";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 101;
            this.label2.Text = "CRM id:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(487, 336);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 23);
            this.btnSave.TabIndex = 107;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(760, 336);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 106;
            this.btnCancel.Text = "Закрыть";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAndClose.Location = new System.Drawing.Point(587, 336);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Size = new System.Drawing.Size(167, 23);
            this.btnSaveAndClose.TabIndex = 105;
            this.btnSaveAndClose.Text = "Сохранить и закрыть";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // btnShowEmployees
            // 
            this.btnShowEmployees.Location = new System.Drawing.Point(482, 199);
            this.btnShowEmployees.Name = "btnShowEmployees";
            this.btnShowEmployees.Size = new System.Drawing.Size(148, 43);
            this.btnShowEmployees.TabIndex = 109;
            this.btnShowEmployees.Text = "Показать сотрудников этой организации";
            this.btnShowEmployees.UseVisualStyleBackColor = true;
            this.btnShowEmployees.Click += new System.EventHandler(this.btnShowEmployees_Click);
            // 
            // btnAddEmployee
            // 
            this.btnAddEmployee.Location = new System.Drawing.Point(482, 260);
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.Size = new System.Drawing.Size(148, 43);
            this.btnAddEmployee.TabIndex = 108;
            this.btnAddEmployee.Text = "Добавить сотрудника";
            this.btnAddEmployee.UseVisualStyleBackColor = true;
            this.btnAddEmployee.Click += new System.EventHandler(this.btnAddEmployee_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 110;
            this.label1.Text = "ЛПУ:";
            // 
            // lbTypeOrgName
            // 
            this.lbTypeOrgName.AutoSize = true;
            this.lbTypeOrgName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbTypeOrgName.Location = new System.Drawing.Point(12, 32);
            this.lbTypeOrgName.Name = "lbTypeOrgName";
            this.lbTypeOrgName.Size = new System.Drawing.Size(63, 18);
            this.lbTypeOrgName.TabIndex = 111;
            this.lbTypeOrgName.Text = "Отдел:";
            // 
            // btnShowRules
            // 
            this.btnShowRules.Location = new System.Drawing.Point(482, 157);
            this.btnShowRules.Name = "btnShowRules";
            this.btnShowRules.Size = new System.Drawing.Size(148, 23);
            this.btnShowRules.TabIndex = 122;
            this.btnShowRules.Text = "Правила наименования";
            this.btnShowRules.UseVisualStyleBackColor = true;
            this.btnShowRules.Visible = false;
            this.btnShowRules.Click += new System.EventHandler(this.btnShowRules_Click);
            // 
            // lbEditor
            // 
            this.lbEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbEditor.AutoSize = true;
            this.lbEditor.Location = new System.Drawing.Point(7, 349);
            this.lbEditor.Name = "lbEditor";
            this.lbEditor.Size = new System.Drawing.Size(53, 13);
            this.lbEditor.TabIndex = 137;
            this.lbEditor.Text = "Изменил";
            // 
            // lbAutor
            // 
            this.lbAutor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbAutor.AutoSize = true;
            this.lbAutor.Location = new System.Drawing.Point(7, 329);
            this.lbAutor.Name = "lbAutor";
            this.lbAutor.Size = new System.Drawing.Size(44, 13);
            this.lbAutor.TabIndex = 136;
            this.lbAutor.Text = "Создал";
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Location = new System.Drawing.Point(143, 83);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(70, 13);
            this.lbID.TabIndex = 139;
            this.lbID.Text = "не присвоен";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(10, 83);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(60, 13);
            this.label34.TabIndex = 138;
            this.label34.Text = "Номер SF:";
            // 
            // gbAvitum
            // 
            this.gbAvitum.Controls.Add(this.label12);
            this.gbAvitum.Controls.Add(this.tbPatientCRRT);
            this.gbAvitum.Controls.Add(this.tbMachineGD);
            this.gbAvitum.Controls.Add(this.label18);
            this.gbAvitum.Controls.Add(this.label13);
            this.gbAvitum.Controls.Add(this.tbPatientPD);
            this.gbAvitum.Controls.Add(this.tbMachineGDF);
            this.gbAvitum.Controls.Add(this.label17);
            this.gbAvitum.Controls.Add(this.label14);
            this.gbAvitum.Controls.Add(this.tbPatientGD);
            this.gbAvitum.Controls.Add(this.tbMachineCRRT);
            this.gbAvitum.Controls.Add(this.label16);
            this.gbAvitum.Controls.Add(this.label15);
            this.gbAvitum.Controls.Add(this.tbShift);
            this.gbAvitum.Location = new System.Drawing.Point(651, 12);
            this.gbAvitum.Name = "gbAvitum";
            this.gbAvitum.Size = new System.Drawing.Size(183, 202);
            this.gbAvitum.TabIndex = 142;
            this.gbAvitum.TabStop = false;
            this.gbAvitum.Text = "Авитум";
            this.gbAvitum.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(28, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "ГД машин:";
            // 
            // tbPatientCRRT
            // 
            this.tbPatientCRRT.Location = new System.Drawing.Point(130, 175);
            this.tbPatientCRRT.MaxLength = 4;
            this.tbPatientCRRT.Name = "tbPatientCRRT";
            this.tbPatientCRRT.Size = new System.Drawing.Size(35, 20);
            this.tbPatientCRRT.TabIndex = 38;
            // 
            // tbMachineGD
            // 
            this.tbMachineGD.Location = new System.Drawing.Point(130, 19);
            this.tbMachineGD.MaxLength = 4;
            this.tbMachineGD.Name = "tbMachineGD";
            this.tbMachineGD.Size = new System.Drawing.Size(35, 20);
            this.tbMachineGD.TabIndex = 26;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(28, 178);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(96, 13);
            this.label18.TabIndex = 37;
            this.label18.Text = "CRRT пациентов:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "ГДФ машин:";
            // 
            // tbPatientPD
            // 
            this.tbPatientPD.Location = new System.Drawing.Point(130, 149);
            this.tbPatientPD.MaxLength = 4;
            this.tbPatientPD.Name = "tbPatientPD";
            this.tbPatientPD.Size = new System.Drawing.Size(35, 20);
            this.tbPatientPD.TabIndex = 36;
            // 
            // tbMachineGDF
            // 
            this.tbMachineGDF.Location = new System.Drawing.Point(130, 45);
            this.tbMachineGDF.MaxLength = 4;
            this.tbMachineGDF.Name = "tbMachineGDF";
            this.tbMachineGDF.Size = new System.Drawing.Size(35, 20);
            this.tbMachineGDF.TabIndex = 28;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 152);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "ПД пациентов:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(28, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "CRRT машин:";
            // 
            // tbPatientGD
            // 
            this.tbPatientGD.Location = new System.Drawing.Point(130, 123);
            this.tbPatientGD.MaxLength = 4;
            this.tbPatientGD.Name = "tbPatientGD";
            this.tbPatientGD.Size = new System.Drawing.Size(35, 20);
            this.tbPatientGD.TabIndex = 34;
            // 
            // tbMachineCRRT
            // 
            this.tbMachineCRRT.Location = new System.Drawing.Point(130, 71);
            this.tbMachineCRRT.MaxLength = 4;
            this.tbMachineCRRT.Name = "tbMachineCRRT";
            this.tbMachineCRRT.Size = new System.Drawing.Size(35, 20);
            this.tbMachineCRRT.TabIndex = 30;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(28, 126);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(81, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "ГД пациентов:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(28, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "Смен:";
            // 
            // tbShift
            // 
            this.tbShift.Location = new System.Drawing.Point(130, 97);
            this.tbShift.MaxLength = 4;
            this.tbShift.Name = "tbShift";
            this.tbShift.Size = new System.Drawing.Size(35, 20);
            this.tbShift.TabIndex = 32;
            // 
            // FormAddOrganization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 371);
            this.Controls.Add(this.gbAvitum);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.lbEditor);
            this.Controls.Add(this.lbAutor);
            this.Controls.Add(this.btnShowRules);
            this.Controls.Add(this.lbTypeOrgName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowEmployees);
            this.Controls.Add(this.btnAddEmployee);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveAndClose);
            this.Controls.Add(this.lbTypeOrg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbCrmID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.tbWebSite);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.cbMainSpec);
            this.Controls.Add(this.lbMainSpec);
            this.Controls.Add(this.tbShortName);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lbBranch);
            this.Controls.Add(this.lbLPU);
            this.MinimumSize = new System.Drawing.Size(739, 409);
            this.Name = "FormAddOrganization";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Карточка";
            this.Load += new System.EventHandler(this.FormAddOrganization_Load);
            this.gbAvitum.ResumeLayout(false);
            this.gbAvitum.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbBranch;
        private System.Windows.Forms.Label lbLPU;
        private System.Windows.Forms.TextBox tbShortName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cbMainSpec;
        private System.Windows.Forms.Label lbMainSpec;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox tbWebSite;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label lbTypeOrg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbCrmID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveAndClose;
        private System.Windows.Forms.Button btnShowEmployees;
        private System.Windows.Forms.Button btnAddEmployee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTypeOrgName;
        private System.Windows.Forms.Button btnShowRules;
        private System.Windows.Forms.Label lbEditor;
        private System.Windows.Forms.Label lbAutor;
        private System.Windows.Forms.Label lbID;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.GroupBox gbAvitum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbPatientCRRT;
        private System.Windows.Forms.TextBox tbMachineGD;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbPatientPD;
        private System.Windows.Forms.TextBox tbMachineGDF;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbPatientGD;
        private System.Windows.Forms.TextBox tbMachineCRRT;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbShift;
    }
}