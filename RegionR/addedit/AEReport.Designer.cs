namespace RegionR.addedit
{
    partial class AEReport
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
            this.cbReg = new System.Windows.Forms.ComboBox();
            this.cbSubReg = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.cbSDiv = new System.Windows.Forms.ComboBox();
            this.lbCheck = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbReg
            // 
            this.cbReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReg.FormattingEnabled = true;
            this.cbReg.Location = new System.Drawing.Point(12, 85);
            this.cbReg.Name = "cbReg";
            this.cbReg.Size = new System.Drawing.Size(274, 21);
            this.cbReg.TabIndex = 2;
            // 
            // cbSubReg
            // 
            this.cbSubReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubReg.FormattingEnabled = true;
            this.cbSubReg.Location = new System.Drawing.Point(12, 112);
            this.cbSubReg.Name = "cbSubReg";
            this.cbSubReg.Size = new System.Drawing.Size(274, 21);
            this.cbSubReg.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(211, 139);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(130, 139);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Сохранить";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "MM-yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 32);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(78, 20);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // cbSDiv
            // 
            this.cbSDiv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSDiv.FormattingEnabled = true;
            this.cbSDiv.Location = new System.Drawing.Point(12, 58);
            this.cbSDiv.Name = "cbSDiv";
            this.cbSDiv.Size = new System.Drawing.Size(59, 21);
            this.cbSDiv.TabIndex = 7;
            // 
            // lbCheck
            // 
            this.lbCheck.AutoSize = true;
            this.lbCheck.Location = new System.Drawing.Point(9, 13);
            this.lbCheck.Name = "lbCheck";
            this.lbCheck.Size = new System.Drawing.Size(35, 13);
            this.lbCheck.TabIndex = 8;
            this.lbCheck.Text = "label1";
            // 
            // AEReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 174);
            this.Controls.Add(this.lbCheck);
            this.Controls.Add(this.cbSDiv);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbSubReg);
            this.Controls.Add(this.cbReg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AEReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактирование продажи";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbReg;
        private System.Windows.Forms.ComboBox cbSubReg;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox cbSDiv;
        private System.Windows.Forms.Label lbCheck;
    }
}