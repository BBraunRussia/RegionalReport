namespace RegionR.addedit
{
    partial class AddRowReport
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCust = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbRegCust = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSubReg = new System.Windows.Forms.ComboBox();
            this.cbLPU = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.tbDist = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbPriceEuro = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbComp = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tbPriceRub = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tbRate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbUserName = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.cbNom = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Наименование";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "дистрибьютора:";
            // 
            // cbCust
            // 
            this.cbCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCust.FormattingEnabled = true;
            this.cbCust.Location = new System.Drawing.Point(157, 152);
            this.cbCust.Name = "cbCust";
            this.cbCust.Size = new System.Drawing.Size(274, 21);
            this.cbCust.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Регион";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "дистрибьютора:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Наименование";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "промежуточного";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 266);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "дистрибьютора:";
            // 
            // tbCount
            // 
            this.tbCount.Location = new System.Drawing.Point(157, 562);
            this.tbCount.MaxLength = 9;
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(274, 20);
            this.tbCount.TabIndex = 11;
            this.tbCount.TextChanged += new System.EventHandler(this.tbCount_TextChanged);
            this.tbCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 306);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Регион ЛПУ:";
            // 
            // cbRegCust
            // 
            this.cbRegCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegCust.Enabled = false;
            this.cbRegCust.FormattingEnabled = true;
            this.cbRegCust.ItemHeight = 13;
            this.cbRegCust.Location = new System.Drawing.Point(157, 203);
            this.cbRegCust.Name = "cbRegCust";
            this.cbRegCust.Size = new System.Drawing.Size(274, 21);
            this.cbRegCust.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 341);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "ЛПУ:";
            // 
            // cbSubReg
            // 
            this.cbSubReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubReg.FormattingEnabled = true;
            this.cbSubReg.Location = new System.Drawing.Point(157, 303);
            this.cbSubReg.Name = "cbSubReg";
            this.cbSubReg.Size = new System.Drawing.Size(274, 21);
            this.cbSubReg.TabIndex = 5;
            this.cbSubReg.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // cbLPU
            // 
            this.cbLPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLPU.FormattingEnabled = true;
            this.cbLPU.Location = new System.Drawing.Point(157, 343);
            this.cbLPU.Name = "cbLPU";
            this.cbLPU.Size = new System.Drawing.Size(274, 21);
            this.cbLPU.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(256, 603);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 22);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Заполнить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(359, 603);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 22);
            this.button2.TabIndex = 13;
            this.button2.Text = "Закрыть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 565);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(91, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Количество (шт):";
            // 
            // tbDist
            // 
            this.tbDist.Location = new System.Drawing.Point(157, 251);
            this.tbDist.MaxLength = 50;
            this.tbDist.Name = "tbDist";
            this.tbDist.Size = new System.Drawing.Size(274, 20);
            this.tbDist.TabIndex = 4;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(16, 475);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(135, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "Общая стоимость (евро):";
            // 
            // tbPriceEuro
            // 
            this.tbPriceEuro.Location = new System.Drawing.Point(157, 477);
            this.tbPriceEuro.Name = "tbPriceEuro";
            this.tbPriceEuro.Size = new System.Drawing.Size(274, 20);
            this.tbPriceEuro.TabIndex = 9;
            this.tbPriceEuro.TextChanged += new System.EventHandler(this.tbPriceEuro_TextChanged);
            this.tbPriceEuro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPriceEuro_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(18, 114);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "Компания:";
            // 
            // cbComp
            // 
            this.cbComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComp.FormattingEnabled = true;
            this.cbComp.Location = new System.Drawing.Point(157, 111);
            this.cbComp.Name = "cbComp";
            this.cbComp.Size = new System.Drawing.Size(274, 21);
            this.cbComp.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(16, 516);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(140, 13);
            this.label19.TabIndex = 39;
            this.label19.Text = "Общая стоимость (рубли):";
            // 
            // tbPriceRub
            // 
            this.tbPriceRub.Location = new System.Drawing.Point(157, 518);
            this.tbPriceRub.Name = "tbPriceRub";
            this.tbPriceRub.Size = new System.Drawing.Size(274, 20);
            this.tbPriceRub.TabIndex = 10;
            this.tbPriceRub.TextChanged += new System.EventHandler(this.tbPriceRub_TextChanged);
            this.tbPriceRub.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPriceRub_KeyPress);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(211, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(34, 13);
            this.label20.TabIndex = 41;
            this.label20.Text = "Курс:";
            // 
            // tbRate
            // 
            this.tbRate.Location = new System.Drawing.Point(256, 45);
            this.tbRate.Name = "tbRate";
            this.tbRate.ReadOnly = true;
            this.tbRate.Size = new System.Drawing.Size(52, 20);
            this.tbRate.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Пользователь:";
            // 
            // lbUserName
            // 
            this.lbUserName.AutoSize = true;
            this.lbUserName.Location = new System.Drawing.Point(105, 22);
            this.lbUserName.Name = "lbUserName";
            this.lbUserName.Size = new System.Drawing.Size(41, 13);
            this.lbUserName.TabIndex = 44;
            this.lbUserName.Text = "label13";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 51);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 13);
            this.label13.TabIndex = 45;
            this.label13.Text = "Дата:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(18, 385);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(89, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Группа товаров:";
            // 
            // cbGroup
            // 
            this.cbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Location = new System.Drawing.Point(157, 387);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(274, 21);
            this.cbGroup.TabIndex = 7;
            this.cbGroup.SelectedIndexChanged += new System.EventHandler(this.cbGroup_SelectedIndexChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(18, 432);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(84, 13);
            this.label22.TabIndex = 2;
            this.label22.Text = "Номенклатура:";
            // 
            // cbNom
            // 
            this.cbNom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNom.FormattingEnabled = true;
            this.cbNom.Location = new System.Drawing.Point(157, 434);
            this.cbNom.Name = "cbNom";
            this.cbNom.Size = new System.Drawing.Size(274, 21);
            this.cbNom.TabIndex = 8;
            this.cbNom.SelectedIndexChanged += new System.EventHandler(this.cbNom_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "MM-yyyy";
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(108, 45);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(77, 20);
            this.dateTimePicker1.TabIndex = 48;
            this.dateTimePicker1.TabStop = false;
            // 
            // AddRowReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 647);
            this.Controls.Add(this.cbNom);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.cbGroup);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lbUserName);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPriceEuro);
            this.Controls.Add(this.tbRate);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.cbComp);
            this.Controls.Add(this.tbPriceRub);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tbDist);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbLPU);
            this.Controls.Add(this.cbSubReg);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbRegCust);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbCust);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddRowReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddRowReport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCust;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbRegCust;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbSubReg;
        private System.Windows.Forms.ComboBox cbLPU;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbDist;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbPriceEuro;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbComp;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbPriceRub;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cbGroup;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox cbNom;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}