namespace RegionR.SF
{
    partial class FormAddBranch
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.rbBranch = new System.Windows.Forms.RadioButton();
            this.rbDepartment = new System.Windows.Forms.RadioButton();
            this.rbDivision = new System.Windows.Forms.RadioButton();
            this.rbPharmacy = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbPharmacy);
            this.groupBox1.Controls.Add(this.rbDivision);
            this.groupBox1.Controls.Add(this.rbDepartment);
            this.groupBox1.Controls.Add(this.rbBranch);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 164);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип подразделения";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(12, 193);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Далее";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(121, 193);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Отмена";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // rbBranch
            // 
            this.rbBranch.AutoSize = true;
            this.rbBranch.Location = new System.Drawing.Point(18, 31);
            this.rbBranch.Name = "rbBranch";
            this.rbBranch.Size = new System.Drawing.Size(66, 17);
            this.rbBranch.TabIndex = 0;
            this.rbBranch.TabStop = true;
            this.rbBranch.Text = "Филиал";
            this.rbBranch.UseVisualStyleBackColor = true;
            // 
            // rbDepartment
            // 
            this.rbDepartment.AutoSize = true;
            this.rbDepartment.Location = new System.Drawing.Point(18, 63);
            this.rbDepartment.Name = "rbDepartment";
            this.rbDepartment.Size = new System.Drawing.Size(80, 17);
            this.rbDepartment.TabIndex = 1;
            this.rbDepartment.TabStop = true;
            this.rbDepartment.Text = "Отделение";
            this.rbDepartment.UseVisualStyleBackColor = true;
            // 
            // rbDivision
            // 
            this.rbDivision.AutoSize = true;
            this.rbDivision.Location = new System.Drawing.Point(18, 97);
            this.rbDivision.Name = "rbDivision";
            this.rbDivision.Size = new System.Drawing.Size(56, 17);
            this.rbDivision.TabIndex = 2;
            this.rbDivision.TabStop = true;
            this.rbDivision.Text = "Отдел";
            this.rbDivision.UseVisualStyleBackColor = true;
            // 
            // rbPharmacy
            // 
            this.rbPharmacy.AutoSize = true;
            this.rbPharmacy.Location = new System.Drawing.Point(18, 131);
            this.rbPharmacy.Name = "rbPharmacy";
            this.rbPharmacy.Size = new System.Drawing.Size(150, 17);
            this.rbPharmacy.TabIndex = 3;
            this.rbPharmacy.TabStop = true;
            this.rbPharmacy.Text = "Аптека (отделение ЛПУ)";
            this.rbPharmacy.UseVisualStyleBackColor = true;
            // 
            // FormAddBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 233);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddBranch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавление Institusions-SF";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbBranch;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RadioButton rbPharmacy;
        private System.Windows.Forms.RadioButton rbDivision;
        private System.Windows.Forms.RadioButton rbDepartment;
    }
}