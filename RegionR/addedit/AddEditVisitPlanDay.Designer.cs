namespace RegionR.addedit
{
    partial class AddEditVisitPlanDay
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
            this.cbULPU = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPlan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbFact = new System.Windows.Forms.TextBox();
            this.tbNext = new System.Windows.Forms.TextBox();
            this.cbActivity = new System.Windows.Forms.ComboBox();
            this.commRD = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dateNewVisit = new System.Windows.Forms.DateTimePicker();
            this.enabledNewVisit = new System.Windows.Forms.CheckBox();
            this.createNewVisit = new System.Windows.Forms.Button();
            this.tip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbULPU
            // 
            this.cbULPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbULPU.FormattingEnabled = true;
            this.cbULPU.Location = new System.Drawing.Point(124, 23);
            this.cbULPU.Name = "cbULPU";
            this.cbULPU.Size = new System.Drawing.Size(171, 21);
            this.cbULPU.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "1 - План действий:";
            // 
            // tbPlan
            // 
            this.tbPlan.Location = new System.Drawing.Point(16, 70);
            this.tbPlan.MaxLength = 500;
            this.tbPlan.Multiline = true;
            this.tbPlan.Name = "tbPlan";
            this.tbPlan.Size = new System.Drawing.Size(258, 150);
            this.tbPlan.TabIndex = 0;
            this.tbPlan.TextChanged += new System.EventHandler(this.validText);
            this.tbPlan.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbPlan_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(277, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "2 - Выполнение:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(541, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "3 - Последующие шаги:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(646, 315);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(727, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbFact
            // 
            this.tbFact.Location = new System.Drawing.Point(280, 70);
            this.tbFact.MaxLength = 500;
            this.tbFact.Multiline = true;
            this.tbFact.Name = "tbFact";
            this.tbFact.Size = new System.Drawing.Size(258, 150);
            this.tbFact.TabIndex = 1;
            this.tbFact.TextChanged += new System.EventHandler(this.validText);
            this.tbFact.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbFact_MouseDoubleClick);
            // 
            // tbNext
            // 
            this.tbNext.Location = new System.Drawing.Point(544, 70);
            this.tbNext.MaxLength = 500;
            this.tbNext.Multiline = true;
            this.tbNext.Name = "tbNext";
            this.tbNext.Size = new System.Drawing.Size(258, 105);
            this.tbNext.TabIndex = 3;
            this.tbNext.TextChanged += new System.EventHandler(this.validText);
            this.tbNext.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbNext_MouseDoubleClick);
            // 
            // cbActivity
            // 
            this.cbActivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActivity.FormattingEnabled = true;
            this.cbActivity.Location = new System.Drawing.Point(301, 23);
            this.cbActivity.Name = "cbActivity";
            this.cbActivity.Size = new System.Drawing.Size(171, 21);
            this.cbActivity.TabIndex = 6;
            // 
            // commRD
            // 
            this.commRD.Location = new System.Drawing.Point(16, 243);
            this.commRD.MaxLength = 500;
            this.commRD.Multiline = true;
            this.commRD.Name = "commRD";
            this.commRD.Size = new System.Drawing.Size(522, 97);
            this.commRD.TabIndex = 7;
            this.commRD.TextChanged += new System.EventHandler(this.validText);
            this.commRD.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbCom_MouseDoubleClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Комментарии руководителя:";
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(12, 23);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(105, 21);
            this.cbStatus.TabIndex = 4;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.validText);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(298, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Вид деятельности:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Название ЛПУ:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Статус:";
            // 
            // dateNewVisit
            // 
            this.dateNewVisit.Location = new System.Drawing.Point(544, 200);
            this.dateNewVisit.Name = "dateNewVisit";
            this.dateNewVisit.Size = new System.Drawing.Size(139, 20);
            this.dateNewVisit.TabIndex = 18;
            this.dateNewVisit.Visible = false;
            this.dateNewVisit.ValueChanged += new System.EventHandler(this.dateNewVisit_ValueChanged);
            // 
            // enabledNewVisit
            // 
            this.enabledNewVisit.AutoSize = true;
            this.enabledNewVisit.Location = new System.Drawing.Point(544, 181);
            this.enabledNewVisit.Name = "enabledNewVisit";
            this.enabledNewVisit.Size = new System.Drawing.Size(157, 17);
            this.enabledNewVisit.TabIndex = 19;
            this.enabledNewVisit.Text = "Назначена новая встреча";
            this.enabledNewVisit.UseVisualStyleBackColor = true;
            this.enabledNewVisit.CheckedChanged += new System.EventHandler(this.enabledNewVisit_CheckedChanged);
            // 
            // createNewVisit
            // 
            this.createNewVisit.Location = new System.Drawing.Point(689, 197);
            this.createNewVisit.Name = "createNewVisit";
            this.createNewVisit.Size = new System.Drawing.Size(113, 23);
            this.createNewVisit.TabIndex = 20;
            this.createNewVisit.Text = "Создать визит";
            this.createNewVisit.UseVisualStyleBackColor = true;
            this.createNewVisit.Visible = false;
            this.createNewVisit.Click += new System.EventHandler(this.createNewVisit_Click);
            // 
            // tip
            // 
            this.tip.AutoSize = true;
            this.tip.ForeColor = System.Drawing.Color.Red;
            this.tip.Location = new System.Drawing.Point(516, 9);
            this.tip.Name = "tip";
            this.tip.Size = new System.Drawing.Size(286, 13);
            this.tip.TabIndex = 21;
            this.tip.Text = "Заполните поля \"Выполнение\" и \"Последующий шаги\"";
            this.tip.Visible = false;
            // 
            // AddEditVisitPlanDay
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(814, 352);
            this.Controls.Add(this.tip);
            this.Controls.Add(this.createNewVisit);
            this.Controls.Add(this.enabledNewVisit);
            this.Controls.Add(this.dateNewVisit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.commRD);
            this.Controls.Add(this.cbActivity);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbNext);
            this.Controls.Add(this.tbFact);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbPlan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbULPU);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditVisitPlanDay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Мой визит";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbULPU;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPlan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbFact;
        private System.Windows.Forms.TextBox tbNext;
        private System.Windows.Forms.ComboBox cbActivity;
        private System.Windows.Forms.TextBox commRD;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateNewVisit;
        private System.Windows.Forms.CheckBox enabledNewVisit;
        private System.Windows.Forms.Button createNewVisit;
        private System.Windows.Forms.Label tip;
    }
}