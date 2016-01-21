namespace RegionR.SF
{
    partial class FormFirstStepAddOrganization
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
            this.btnOK = new System.Windows.Forms.Button();
            this.rbLPU = new System.Windows.Forms.RadioButton();
            this.rbPharmacy = new System.Windows.Forms.RadioButton();
            this.rbAdminOrganization = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(94, 137);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Далее";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rbLPU
            // 
            this.rbLPU.AutoSize = true;
            this.rbLPU.Location = new System.Drawing.Point(18, 36);
            this.rbLPU.Name = "rbLPU";
            this.rbLPU.Size = new System.Drawing.Size(49, 17);
            this.rbLPU.TabIndex = 1;
            this.rbLPU.Text = "ЛПУ";
            this.rbLPU.UseVisualStyleBackColor = true;
            this.rbLPU.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbPharmacy
            // 
            this.rbPharmacy.AutoSize = true;
            this.rbPharmacy.Location = new System.Drawing.Point(17, 59);
            this.rbPharmacy.Name = "rbPharmacy";
            this.rbPharmacy.Size = new System.Drawing.Size(145, 17);
            this.rbPharmacy.TabIndex = 2;
            this.rbPharmacy.Text = "Аптека (коммерческая)";
            this.rbPharmacy.UseVisualStyleBackColor = true;
            this.rbPharmacy.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // rbAdminOrganization
            // 
            this.rbAdminOrganization.AutoSize = true;
            this.rbAdminOrganization.Location = new System.Drawing.Point(17, 82);
            this.rbAdminOrganization.Name = "rbAdminOrganization";
            this.rbAdminOrganization.Size = new System.Drawing.Size(185, 17);
            this.rbAdminOrganization.TabIndex = 3;
            this.rbAdminOrganization.Text = "Административное учреждение";
            this.rbAdminOrganization.UseVisualStyleBackColor = true;
            this.rbAdminOrganization.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbLPU);
            this.groupBox1.Controls.Add(this.rbAdminOrganization);
            this.groupBox1.Controls.Add(this.rbPharmacy);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 119);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип организации";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(175, 137);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormFirstStepAddOrganization
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(262, 171);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFirstStepAddOrganization";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавление Institutions-SF";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbLPU;
        private System.Windows.Forms.RadioButton rbPharmacy;
        private System.Windows.Forms.RadioButton rbAdminOrganization;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;



    }
}