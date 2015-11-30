namespace RegionR.addedit
{
    partial class AddEditCust
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
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbCustName = new System.Windows.Forms.TextBox();
            this.tbCustCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbShipTo = new System.Windows.Forms.TextBox();
            this.tbPayer = new System.Windows.Forms.TextBox();
            this.tbPlant = new System.Windows.Forms.TextBox();
            this.tbDistChan = new System.Windows.Forms.TextBox();
            this.cbReg = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 44;
            this.label9.Text = "ShipTo:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(227, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 43;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(146, 202);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbCustName
            // 
            this.tbCustName.Location = new System.Drawing.Point(94, 31);
            this.tbCustName.MaxLength = 50;
            this.tbCustName.Name = "tbCustName";
            this.tbCustName.Size = new System.Drawing.Size(205, 20);
            this.tbCustName.TabIndex = 35;
            // 
            // tbCustCode
            // 
            this.tbCustCode.Location = new System.Drawing.Point(94, 5);
            this.tbCustCode.MaxLength = 10;
            this.tbCustCode.Name = "tbCustCode";
            this.tbCustCode.Size = new System.Drawing.Size(205, 20);
            this.tbCustCode.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "distChan:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "plant:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "payer:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Название:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Номер:";
            // 
            // tbShipTo
            // 
            this.tbShipTo.Location = new System.Drawing.Point(94, 57);
            this.tbShipTo.MaxLength = 10;
            this.tbShipTo.Name = "tbShipTo";
            this.tbShipTo.Size = new System.Drawing.Size(205, 20);
            this.tbShipTo.TabIndex = 45;
            // 
            // tbPayer
            // 
            this.tbPayer.Location = new System.Drawing.Point(94, 83);
            this.tbPayer.MaxLength = 10;
            this.tbPayer.Name = "tbPayer";
            this.tbPayer.Size = new System.Drawing.Size(205, 20);
            this.tbPayer.TabIndex = 46;
            // 
            // tbPlant
            // 
            this.tbPlant.Location = new System.Drawing.Point(94, 111);
            this.tbPlant.MaxLength = 4;
            this.tbPlant.Name = "tbPlant";
            this.tbPlant.Size = new System.Drawing.Size(205, 20);
            this.tbPlant.TabIndex = 47;
            // 
            // tbDistChan
            // 
            this.tbDistChan.Location = new System.Drawing.Point(94, 138);
            this.tbDistChan.MaxLength = 2;
            this.tbDistChan.Name = "tbDistChan";
            this.tbDistChan.Size = new System.Drawing.Size(205, 20);
            this.tbDistChan.TabIndex = 48;
            // 
            // cbReg
            // 
            this.cbReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReg.FormattingEnabled = true;
            this.cbReg.Location = new System.Drawing.Point(94, 165);
            this.cbReg.Name = "cbReg";
            this.cbReg.Size = new System.Drawing.Size(205, 21);
            this.cbReg.TabIndex = 49;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 50;
            this.label6.Text = "Регион:";
            // 
            // AddEditCust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(314, 237);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbReg);
            this.Controls.Add(this.tbDistChan);
            this.Controls.Add(this.tbPlant);
            this.Controls.Add(this.tbPayer);
            this.Controls.Add(this.tbShipTo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbCustName);
            this.Controls.Add(this.tbCustCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditCust";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddEditCust";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tbCustName;
        private System.Windows.Forms.TextBox tbCustCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbShipTo;
        private System.Windows.Forms.TextBox tbPayer;
        private System.Windows.Forms.TextBox tbPlant;
        private System.Windows.Forms.TextBox tbDistChan;
        private System.Windows.Forms.ComboBox cbReg;
        private System.Windows.Forms.Label label6;
    }
}