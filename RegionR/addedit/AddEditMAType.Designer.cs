namespace RegionR.addedit
{
    partial class AddEditMAType
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbSite = new System.Windows.Forms.TextBox();
            this.tbSname = new System.Windows.Forms.TextBox();
            this.tbComm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpMA = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cbTheme = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbTypeMA = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbTheme = new System.Windows.Forms.TextBox();
            this.btnTheme = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCountry = new System.Windows.Forms.Button();
            this.cbCountry = new System.Windows.Forms.ComboBox();
            this.tbCountry = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCity = new System.Windows.Forms.Button();
            this.cbCity = new System.Windows.Forms.ComboBox();
            this.tbCity = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(162, 81);
            this.tbName.Multiline = true;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(256, 53);
            this.tbName.TabIndex = 0;
            this.tbName.Click += new System.EventHandler(this.tbName_Click);
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tbSite
            // 
            this.tbSite.Location = new System.Drawing.Point(162, 223);
            this.tbSite.Name = "tbSite";
            this.tbSite.Size = new System.Drawing.Size(256, 20);
            this.tbSite.TabIndex = 1;
            this.tbSite.Click += new System.EventHandler(this.tbSite_Click);
            this.tbSite.TextChanged += new System.EventHandler(this.tbSite_TextChanged);
            // 
            // tbSname
            // 
            this.tbSname.Location = new System.Drawing.Point(162, 140);
            this.tbSname.Name = "tbSname";
            this.tbSname.Size = new System.Drawing.Size(256, 20);
            this.tbSname.TabIndex = 2;
            this.tbSname.Click += new System.EventHandler(this.tbSname_Click);
            this.tbSname.TextChanged += new System.EventHandler(this.tbSname_TextChanged);
            // 
            // tbComm
            // 
            this.tbComm.Location = new System.Drawing.Point(162, 166);
            this.tbComm.Multiline = true;
            this.tbComm.Name = "tbComm";
            this.tbComm.Size = new System.Drawing.Size(256, 51);
            this.tbComm.TabIndex = 3;
            this.tbComm.Click += new System.EventHandler(this.tbComm_Click);
            this.tbComm.TextChanged += new System.EventHandler(this.tbComm_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "Полное название\r\nконференции (без номера)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 26);
            this.label2.TabIndex = 5;
            this.label2.Text = "Сокращенное название\r\nконференции";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Сайт";
            // 
            // dtpMA
            // 
            this.dtpMA.CustomFormat = "MM - yyyy";
            this.dtpMA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMA.Location = new System.Drawing.Point(162, 19);
            this.dtpMA.Name = "dtpMA";
            this.dtpMA.Size = new System.Drawing.Size(71, 20);
            this.dtpMA.TabIndex = 52;
            this.dtpMA.Value = new System.DateTime(2016, 1, 1, 15, 27, 0, 0);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 53;
            this.label5.Text = "Месяц проведения";
            // 
            // cbTheme
            // 
            this.cbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTheme.FormattingEnabled = true;
            this.cbTheme.Location = new System.Drawing.Point(4, 19);
            this.cbTheme.Name = "cbTheme";
            this.cbTheme.Size = new System.Drawing.Size(396, 21);
            this.cbTheme.TabIndex = 54;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 26);
            this.label7.TabIndex = 57;
            this.label7.Text = "Тип маркетингового\r\nмероприятия";
            // 
            // cbTypeMA
            // 
            this.cbTypeMA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeMA.FormattingEnabled = true;
            this.cbTypeMA.Location = new System.Drawing.Point(162, 54);
            this.cbTypeMA.Name = "cbTypeMA";
            this.cbTypeMA.Size = new System.Drawing.Size(256, 21);
            this.cbTypeMA.TabIndex = 56;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 39);
            this.label4.TabIndex = 7;
            this.label4.Text = "Примечание\r\n(название конф. с\r\nномером)";
            // 
            // tbTheme
            // 
            this.tbTheme.Location = new System.Drawing.Point(4, 48);
            this.tbTheme.Name = "tbTheme";
            this.tbTheme.Size = new System.Drawing.Size(245, 20);
            this.tbTheme.TabIndex = 58;
            // 
            // btnTheme
            // 
            this.btnTheme.Location = new System.Drawing.Point(275, 46);
            this.btnTheme.Name = "btnTheme";
            this.btnTheme.Size = new System.Drawing.Size(125, 23);
            this.btnTheme.TabIndex = 59;
            this.btnTheme.Text = "Добавить тематику";
            this.btnTheme.UseVisualStyleBackColor = true;
            this.btnTheme.Click += new System.EventHandler(this.btnTheme_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTheme);
            this.groupBox1.Controls.Add(this.cbTheme);
            this.groupBox1.Controls.Add(this.tbTheme);
            this.groupBox1.Location = new System.Drawing.Point(12, 249);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 84);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тематика";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(343, 531);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 61;
            this.btnClear.Text = "Назад";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(262, 531);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 62;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCountry);
            this.groupBox2.Controls.Add(this.cbCountry);
            this.groupBox2.Controls.Add(this.tbCountry);
            this.groupBox2.Location = new System.Drawing.Point(12, 339);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 84);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Страна";
            // 
            // btnCountry
            // 
            this.btnCountry.Location = new System.Drawing.Point(275, 46);
            this.btnCountry.Name = "btnCountry";
            this.btnCountry.Size = new System.Drawing.Size(125, 23);
            this.btnCountry.TabIndex = 59;
            this.btnCountry.Text = "Добавить страну";
            this.btnCountry.UseVisualStyleBackColor = true;
            this.btnCountry.Click += new System.EventHandler(this.btnCountry_Click);
            // 
            // cbCountry
            // 
            this.cbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCountry.FormattingEnabled = true;
            this.cbCountry.Location = new System.Drawing.Point(4, 19);
            this.cbCountry.Name = "cbCountry";
            this.cbCountry.Size = new System.Drawing.Size(396, 21);
            this.cbCountry.TabIndex = 54;
            this.cbCountry.SelectedIndexChanged += new System.EventHandler(this.cbCountry_SelectedIndexChanged);
            // 
            // tbCountry
            // 
            this.tbCountry.Location = new System.Drawing.Point(4, 48);
            this.tbCountry.Name = "tbCountry";
            this.tbCountry.Size = new System.Drawing.Size(245, 20);
            this.tbCountry.TabIndex = 58;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCity);
            this.groupBox3.Controls.Add(this.cbCity);
            this.groupBox3.Controls.Add(this.tbCity);
            this.groupBox3.Location = new System.Drawing.Point(12, 429);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(406, 84);
            this.groupBox3.TabIndex = 61;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Город";
            // 
            // btnCity
            // 
            this.btnCity.Location = new System.Drawing.Point(275, 46);
            this.btnCity.Name = "btnCity";
            this.btnCity.Size = new System.Drawing.Size(125, 23);
            this.btnCity.TabIndex = 59;
            this.btnCity.Text = "Добавить город";
            this.btnCity.UseVisualStyleBackColor = true;
            this.btnCity.Click += new System.EventHandler(this.btnCity_Click);
            // 
            // cbCity
            // 
            this.cbCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCity.FormattingEnabled = true;
            this.cbCity.Location = new System.Drawing.Point(4, 19);
            this.cbCity.Name = "cbCity";
            this.cbCity.Size = new System.Drawing.Size(396, 21);
            this.cbCity.TabIndex = 54;
            // 
            // tbCity
            // 
            this.tbCity.Location = new System.Drawing.Point(4, 48);
            this.tbCity.Name = "tbCity";
            this.tbCity.Size = new System.Drawing.Size(245, 20);
            this.tbCity.TabIndex = 58;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(162, 531);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 63;
            this.button1.Text = "Редактировать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AddEditMAType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 563);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbTypeMA);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpMA);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbComm);
            this.Controls.Add(this.tbSname);
            this.Controls.Add(this.tbSite);
            this.Controls.Add(this.tbName);
            this.Name = "AddEditMAType";
            this.Text = "Добавление в план мероприятий";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbSite;
        private System.Windows.Forms.TextBox tbSname;
        private System.Windows.Forms.TextBox tbComm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpMA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbTheme;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbTypeMA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbTheme;
        private System.Windows.Forms.Button btnTheme;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCountry;
        private System.Windows.Forms.ComboBox cbCountry;
        private System.Windows.Forms.TextBox tbCountry;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCity;
        private System.Windows.Forms.ComboBox cbCity;
        private System.Windows.Forms.TextBox tbCity;
        private System.Windows.Forms.Button button1;
    }
}