namespace RegionR.addedit
{
    partial class AddMA_Name
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this._cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьМестоПроведенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просмотрМестПроведенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbMAType = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this._cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToDeleteRows = false;
            this.dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.ContextMenuStrip = this._cms;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv1.Location = new System.Drawing.Point(3, 113);
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            this.dgv1.Size = new System.Drawing.Size(668, 380);
            this.dgv1.TabIndex = 0;
            this.dgv1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellContentClick);
            this.dgv1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellContentDoubleClick);
            this.dgv1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellDoubleClick);
            // 
            // _cms
            // 
            this._cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьМестоПроведенияToolStripMenuItem,
            this.просмотрМестПроведенияToolStripMenuItem,
            this.редактироватьToolStripMenuItem});
            this._cms.Name = "_cms";
            this._cms.Size = new System.Drawing.Size(231, 92);
            // 
            // добавитьМестоПроведенияToolStripMenuItem
            // 
            this.добавитьМестоПроведенияToolStripMenuItem.Name = "добавитьМестоПроведенияToolStripMenuItem";
            this.добавитьМестоПроведенияToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.добавитьМестоПроведенияToolStripMenuItem.Text = "Добавить место проведения";
            this.добавитьМестоПроведенияToolStripMenuItem.Click += new System.EventHandler(this.добавитьМестоПроведенияToolStripMenuItem_Click);
            // 
            // просмотрМестПроведенияToolStripMenuItem
            // 
            this.просмотрМестПроведенияToolStripMenuItem.Name = "просмотрМестПроведенияToolStripMenuItem";
            this.просмотрМестПроведенияToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.просмотрМестПроведенияToolStripMenuItem.Text = "Просмотр мест проведения";
            this.просмотрМестПроведенияToolStripMenuItem.Click += new System.EventHandler(this.просмотрМестПроведенияToolStripMenuItem_Click);
            // 
            // cbMAType
            // 
            this.cbMAType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMAType.FormattingEnabled = true;
            this.cbMAType.Location = new System.Drawing.Point(6, 50);
            this.cbMAType.Name = "cbMAType";
            this.cbMAType.Size = new System.Drawing.Size(246, 21);
            this.cbMAType.TabIndex = 35;
            this.cbMAType.SelectedIndexChanged += new System.EventHandler(this.cbMAType_SelectedIndexChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(258, 50);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(241, 21);
            this.comboBox1.TabIndex = 36;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 34);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(159, 13);
            this.label15.TabIndex = 37;
            this.label15.Text = "Маркетинговое мероприятие:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(255, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Тематика:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(411, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 39;
            this.button1.Text = "Назад";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(311, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 40;
            this.button2.Text = "Добавить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(284, 26);
            this.label2.TabIndex = 41;
            this.label2.Text = "Щёлкните 2 раза по ячейке с выбранным названием, \r\nесли такого нет, то нажмите кн" +
    "опку \"Добавить\"";
            this.label2.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 87);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(399, 20);
            this.textBox1.TabIndex = 42;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(411, 85);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 43;
            this.button3.Text = "Найти";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Найти по названию:";
            // 
            // редактироватьToolStripMenuItem
            // 
            this.редактироватьToolStripMenuItem.Name = "редактироватьToolStripMenuItem";
            this.редактироватьToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.редактироватьToolStripMenuItem.Text = "Редактировать";
            this.редактироватьToolStripMenuItem.Visible = false;
            this.редактироватьToolStripMenuItem.Click += new System.EventHandler(this.редактироватьToolStripMenuItem_Click);
            // 
            // AddMA_Name
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 499);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cbMAType);
            this.Controls.Add(this.dgv1);
            this.Name = "AddMA_Name";
            this.Text = "Выберите название мероприятия";
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this._cms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.ComboBox cbMAType;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip _cms;
        private System.Windows.Forms.ToolStripMenuItem добавитьМестоПроведенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просмотрМестПроведенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
    }
}