namespace RegionR.addedit
{
    partial class AddEditNomForAcc
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
            this.dtYear2 = new System.Windows.Forms.DateTimePicker();
            this.dtYear1 = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbSeq = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tbDilCost = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbNomGroup = new System.Windows.Forms.ComboBox();
            this.btnUpdateDilCost = new System.Windows.Forms.Button();
            this.tbYearDilCost = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dtYear2
            // 
            this.dtYear2.CustomFormat = "yyyy";
            this.dtYear2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtYear2.Location = new System.Drawing.Point(203, 184);
            this.dtYear2.Name = "dtYear2";
            this.dtYear2.Size = new System.Drawing.Size(80, 20);
            this.dtYear2.TabIndex = 18;
            // 
            // dtYear1
            // 
            this.dtYear1.CustomFormat = "yyyy";
            this.dtYear1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtYear1.Location = new System.Drawing.Point(98, 184);
            this.dtYear1.Name = "dtYear1";
            this.dtYear1.Size = new System.Drawing.Size(75, 20);
            this.dtYear1.TabIndex = 17;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(424, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(324, 220);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(178, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "по";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Отчетность с:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Итоговая номенклатура:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(179, 13);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(323, 20);
            this.tbName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Наименование:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Единицы измерения:";
            // 
            // tbSeq
            // 
            this.tbSeq.Location = new System.Drawing.Point(179, 153);
            this.tbSeq.Name = "tbSeq";
            this.tbSeq.Size = new System.Drawing.Size(74, 20);
            this.tbSeq.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Порядок отображения:";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "шт",
            "euro"});
            this.cbType.Location = new System.Drawing.Point(179, 83);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(74, 21);
            this.cbType.TabIndex = 25;
            // 
            // tbDilCost
            // 
            this.tbDilCost.Location = new System.Drawing.Point(179, 114);
            this.tbDilCost.Name = "tbDilCost";
            this.tbDilCost.Size = new System.Drawing.Size(74, 20);
            this.tbDilCost.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Дилерская цена:";
            // 
            // cbNomGroup
            // 
            this.cbNomGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNomGroup.FormattingEnabled = true;
            this.cbNomGroup.Location = new System.Drawing.Point(179, 45);
            this.cbNomGroup.Name = "cbNomGroup";
            this.cbNomGroup.Size = new System.Drawing.Size(323, 21);
            this.cbNomGroup.TabIndex = 26;
            // 
            // btnUpdateDilCost
            // 
            this.btnUpdateDilCost.Location = new System.Drawing.Point(381, 112);
            this.btnUpdateDilCost.Name = "btnUpdateDilCost";
            this.btnUpdateDilCost.Size = new System.Drawing.Size(98, 23);
            this.btnUpdateDilCost.TabIndex = 27;
            this.btnUpdateDilCost.Text = "Обновить цену";
            this.btnUpdateDilCost.UseVisualStyleBackColor = true;
            this.btnUpdateDilCost.Click += new System.EventHandler(this.btnUpdateDilCost_Click);
            // 
            // tbYearDilCost
            // 
            this.tbYearDilCost.Location = new System.Drawing.Point(301, 114);
            this.tbYearDilCost.Name = "tbYearDilCost";
            this.tbYearDilCost.Size = new System.Drawing.Size(74, 20);
            this.tbYearDilCost.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(267, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Год:";
            // 
            // AddEditNomForAcc
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(511, 255);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbYearDilCost);
            this.Controls.Add(this.btnUpdateDilCost);
            this.Controls.Add(this.cbNomGroup);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.tbSeq);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDilCost);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtYear2);
            this.Controls.Add(this.dtYear1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditNomForAcc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактирование номенклатуры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtYear2;
        private System.Windows.Forms.DateTimePicker dtYear1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSeq;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox tbDilCost;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbNomGroup;
        private System.Windows.Forms.Button btnUpdateDilCost;
        private System.Windows.Forms.TextBox tbYearDilCost;
        private System.Windows.Forms.Label label8;
    }
}