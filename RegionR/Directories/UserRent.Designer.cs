namespace RegionR.Directories
{
    partial class UserRent
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelUserBC = new System.Windows.Forms.Button();
            this.btnAddUserBC = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this._dgv2 = new System.Windows.Forms.DataGridView();
            this._dgv1 = new System.Windows.Forms.DataGridView();
            this.dateRent = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.сделатьНеактивнымToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flag_enable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(319, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 13);
            this.label1.TabIndex = 100;
            this.label1.Text = "Список пользователей, для которых доступен маркетинг 1:";
            // 
            // btnDelUserBC
            // 
            this.btnDelUserBC.Location = new System.Drawing.Point(273, 163);
            this.btnDelUserBC.Name = "btnDelUserBC";
            this.btnDelUserBC.Size = new System.Drawing.Size(43, 23);
            this.btnDelUserBC.TabIndex = 99;
            this.btnDelUserBC.Text = "<<";
            this.btnDelUserBC.UseVisualStyleBackColor = true;
            this.btnDelUserBC.Click += new System.EventHandler(this.btnDelUserBC_Click);
            // 
            // btnAddUserBC
            // 
            this.btnAddUserBC.Location = new System.Drawing.Point(273, 120);
            this.btnAddUserBC.Name = "btnAddUserBC";
            this.btnAddUserBC.Size = new System.Drawing.Size(43, 23);
            this.btnAddUserBC.TabIndex = 98;
            this.btnAddUserBC.Text = ">>";
            this.btnAddUserBC.UseVisualStyleBackColor = true;
            this.btnAddUserBC.Click += new System.EventHandler(this.btnAddUserBC_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 97;
            this.label3.Text = "Список пользователей:";
            // 
            // _dgv2
            // 
            this._dgv2.AllowUserToAddRows = false;
            this._dgv2.AllowUserToDeleteRows = false;
            this._dgv2.AllowUserToResizeRows = false;
            this._dgv2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dgv2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgv2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.user_id,
            this.user_name,
            this.flag_enable});
            this._dgv2.Location = new System.Drawing.Point(325, 88);
            this._dgv2.Name = "_dgv2";
            this._dgv2.ReadOnly = true;
            this._dgv2.RowHeadersVisible = false;
            this._dgv2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._dgv2.Size = new System.Drawing.Size(305, 308);
            this._dgv2.TabIndex = 96;
            this._dgv2.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this._dgv2_CellMouseDown);
            this._dgv2.MouseDown += new System.Windows.Forms.MouseEventHandler(this._dgv2_MouseDown);
            // 
            // _dgv1
            // 
            this._dgv1.AllowUserToAddRows = false;
            this._dgv1.AllowUserToDeleteRows = false;
            this._dgv1.AllowUserToResizeRows = false;
            this._dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgv1.Location = new System.Drawing.Point(13, 88);
            this._dgv1.Name = "_dgv1";
            this._dgv1.ReadOnly = true;
            this._dgv1.RowHeadersVisible = false;
            this._dgv1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._dgv1.Size = new System.Drawing.Size(254, 308);
            this._dgv1.TabIndex = 95;
            // 
            // dateRent
            // 
            this.dateRent.CustomFormat = "yyyy-MM-dd";
            this.dateRent.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateRent.Location = new System.Drawing.Point(194, 9);
            this.dateRent.Name = "dateRent";
            this.dateRent.Size = new System.Drawing.Size(134, 20);
            this.dateRent.TabIndex = 101;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 102;
            this.label2.Text = "Дата отчета по маркетингу 1";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(382, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(248, 23);
            this.button1.TabIndex = 103;
            this.button1.Text = "Применить ко всем";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(255, 21);
            this.comboBox1.TabIndex = 104;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сделатьНеактивнымToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(190, 26);
            // 
            // сделатьНеактивнымToolStripMenuItem
            // 
            this.сделатьНеактивнымToolStripMenuItem.Name = "сделатьНеактивнымToolStripMenuItem";
            this.сделатьНеактивнымToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.сделатьНеактивнымToolStripMenuItem.Text = "Сделать неактивным";
            this.сделатьНеактивнымToolStripMenuItem.Click += new System.EventHandler(this.сделатьНеактивнымToolStripMenuItem_Click);
            // 
            // user_id
            // 
            this.user_id.HeaderText = "user_id";
            this.user_id.Name = "user_id";
            this.user_id.ReadOnly = true;
            this.user_id.Visible = false;
            // 
            // user_name
            // 
            this.user_name.HeaderText = "Пользователь";
            this.user_name.Name = "user_name";
            this.user_name.ReadOnly = true;
            // 
            // flag_enable
            // 
            this.flag_enable.HeaderText = "flag_enable";
            this.flag_enable.Name = "flag_enable";
            this.flag_enable.ReadOnly = true;
            this.flag_enable.Visible = false;
            // 
            // UserRent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 408);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateRent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelUserBC);
            this.Controls.Add(this.btnAddUserBC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._dgv2);
            this.Controls.Add(this._dgv1);
            this.Name = "UserRent";
            this.Text = "Добавление РП в маркетинг1";
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDelUserBC;
        private System.Windows.Forms.Button btnAddUserBC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView _dgv2;
        private System.Windows.Forms.DataGridView _dgv1;
        private System.Windows.Forms.DateTimePicker dateRent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem сделатьНеактивнымToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn flag_enable;

    }
}