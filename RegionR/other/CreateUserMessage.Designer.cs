namespace RegionR.other
{
    partial class CreateUserMessage
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbHeader = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbText = new System.Windows.Forms.TextBox();
            this.cbRP = new System.Windows.Forms.CheckBox();
            this.cbRM = new System.Windows.Forms.CheckBox();
            this.cbRD = new System.Windows.Forms.CheckBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lbDate = new System.Windows.Forms.Label();
            this.cbEnd = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Заголовок:";
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new System.Drawing.Point(82, 139);
            this.tbHeader.MaxLength = 50;
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.Size = new System.Drawing.Size(358, 20);
            this.tbHeader.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Сообщение:";
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(15, 185);
            this.tbText.MaxLength = 500;
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(425, 105);
            this.tbText.TabIndex = 7;
            // 
            // cbRP
            // 
            this.cbRP.AutoSize = true;
            this.cbRP.Location = new System.Drawing.Point(6, 19);
            this.cbRP.Name = "cbRP";
            this.cbRP.Size = new System.Drawing.Size(179, 17);
            this.cbRP.TabIndex = 0;
            this.cbRP.Text = "Региональные представители";
            this.cbRP.UseVisualStyleBackColor = true;
            // 
            // cbRM
            // 
            this.cbRM.AutoSize = true;
            this.cbRM.Location = new System.Drawing.Point(191, 19);
            this.cbRM.Name = "cbRM";
            this.cbRM.Size = new System.Drawing.Size(163, 17);
            this.cbRM.TabIndex = 1;
            this.cbRM.Text = "Региональные менеджеры";
            this.cbRM.UseVisualStyleBackColor = true;
            // 
            // cbRD
            // 
            this.cbRD.AutoSize = true;
            this.cbRD.Location = new System.Drawing.Point(6, 42);
            this.cbRD.Name = "cbRD";
            this.cbRD.Size = new System.Drawing.Size(156, 17);
            this.cbRD.TabIndex = 2;
            this.cbRD.Text = "Региональные директора";
            this.cbRD.UseVisualStyleBackColor = true;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "HH-mm  dd-MM-yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(105, 113);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(133, 20);
            this.dtpDate.TabIndex = 5;
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Location = new System.Drawing.Point(11, 115);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(88, 13);
            this.lbDate.TabIndex = 9;
            this.lbDate.Text = "Показывать до:";
            // 
            // cbEnd
            // 
            this.cbEnd.AutoSize = true;
            this.cbEnd.Location = new System.Drawing.Point(12, 87);
            this.cbEnd.Name = "cbEnd";
            this.cbEnd.Size = new System.Drawing.Size(80, 17);
            this.cbEnd.TabIndex = 3;
            this.cbEnd.Text = "Бессрочно";
            this.cbEnd.UseVisualStyleBackColor = true;
            this.cbEnd.CheckedChanged += new System.EventHandler(this.cbEnd_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 301);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отменить";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(284, 301);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "Отправить";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbRP);
            this.groupBox1.Controls.Add(this.cbRM);
            this.groupBox1.Controls.Add(this.cbRD);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 69);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Получатели";
            // 
            // CreateUserMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(452, 336);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbEnd);
            this.Controls.Add(this.lbDate);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbHeader);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateUserMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Новое сообщение";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.CheckBox cbRP;
        private System.Windows.Forms.CheckBox cbRM;
        private System.Windows.Forms.CheckBox cbRD;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.CheckBox cbEnd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}