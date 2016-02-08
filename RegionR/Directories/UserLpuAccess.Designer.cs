namespace RegionR.Directories
{
    partial class UserLpuAccess
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUsers = new System.Windows.Forms.ComboBox();
            this.cbRegions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._dgv1 = new System.Windows.Forms.DataGridView();
            this.cbSDiv = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this._dgv2 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddUserLPU = new System.Windows.Forms.Button();
            this.btnDelUserLPU = new System.Windows.Forms.Button();
            this.btnMoveSales = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(561, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "Пользователи:";
            // 
            // cbUsers
            // 
            this.cbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsers.FormattingEnabled = true;
            this.cbUsers.Location = new System.Drawing.Point(564, 21);
            this.cbUsers.Name = "cbUsers";
            this.cbUsers.Size = new System.Drawing.Size(211, 21);
            this.cbUsers.TabIndex = 65;
            this.cbUsers.SelectedIndexChanged += new System.EventHandler(this.cbUsers_SelectedIndexChanged);
            // 
            // cbRegions
            // 
            this.cbRegions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegions.FormattingEnabled = true;
            this.cbRegions.Location = new System.Drawing.Point(8, 21);
            this.cbRegions.Name = "cbRegions";
            this.cbRegions.Size = new System.Drawing.Size(196, 21);
            this.cbRegions.TabIndex = 64;
            this.cbRegions.SelectedIndexChanged += new System.EventHandler(this.cbRegions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 63;
            this.label1.Text = "Регион:";
            // 
            // _dgv1
            // 
            this._dgv1.AllowUserToAddRows = false;
            this._dgv1.AllowUserToDeleteRows = false;
            this._dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._dgv1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dgv1.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dgv1.DefaultCellStyle = dataGridViewCellStyle2;
            this._dgv1.Location = new System.Drawing.Point(8, 74);
            this._dgv1.Name = "_dgv1";
            this._dgv1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this._dgv1.RowHeadersVisible = false;
            this._dgv1.RowHeadersWidth = 20;
            this._dgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._dgv1.Size = new System.Drawing.Size(427, 392);
            this._dgv1.TabIndex = 68;
            this._dgv1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this._dgv1_CellMouseDoubleClick);
            this._dgv1.KeyDown += new System.Windows.Forms.KeyEventHandler(this._dgv1_KeyDown);
            // 
            // cbSDiv
            // 
            this.cbSDiv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSDiv.FormattingEnabled = true;
            this.cbSDiv.Items.AddRange(new object[] {
            "HC",
            "AE"});
            this.cbSDiv.Location = new System.Drawing.Point(488, 21);
            this.cbSDiv.Name = "cbSDiv";
            this.cbSDiv.Size = new System.Drawing.Size(57, 21);
            this.cbSDiv.TabIndex = 70;
            this.cbSDiv.SelectedIndexChanged += new System.EventHandler(this.cbSDiv_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(484, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 69;
            this.label3.Text = "Дивизион:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(224, 21);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 71;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // _dgv2
            // 
            this._dgv2.AllowUserToAddRows = false;
            this._dgv2.AllowUserToDeleteRows = false;
            this._dgv2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dgv2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this._dgv2.ColumnHeadersHeight = 35;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dgv2.DefaultCellStyle = dataGridViewCellStyle5;
            this._dgv2.Location = new System.Drawing.Point(490, 74);
            this._dgv2.Name = "_dgv2";
            this._dgv2.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv2.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this._dgv2.RowHeadersVisible = false;
            this._dgv2.RowHeadersWidth = 20;
            this._dgv2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._dgv2.Size = new System.Drawing.Size(423, 392);
            this._dgv2.TabIndex = 72;
            this._dgv2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._dgv2_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 73;
            this.label4.Text = "ЛПУ региона:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(487, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 74;
            this.label5.Text = "ЛПУ пользователя:";
            // 
            // btnAddUserLPU
            // 
            this.btnAddUserLPU.Location = new System.Drawing.Point(441, 99);
            this.btnAddUserLPU.Name = "btnAddUserLPU";
            this.btnAddUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnAddUserLPU.TabIndex = 75;
            this.btnAddUserLPU.Text = ">>";
            this.btnAddUserLPU.UseVisualStyleBackColor = true;
            this.btnAddUserLPU.Click += new System.EventHandler(this.btnAddUserLPU_Click);
            // 
            // btnDelUserLPU
            // 
            this.btnDelUserLPU.Location = new System.Drawing.Point(441, 163);
            this.btnDelUserLPU.Name = "btnDelUserLPU";
            this.btnDelUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnDelUserLPU.TabIndex = 76;
            this.btnDelUserLPU.Text = "<<";
            this.btnDelUserLPU.UseVisualStyleBackColor = true;
            this.btnDelUserLPU.Click += new System.EventHandler(this.btnDelUserLPU_Click);
            // 
            // btnMoveSales
            // 
            this.btnMoveSales.Location = new System.Drawing.Point(777, 45);
            this.btnMoveSales.Name = "btnMoveSales";
            this.btnMoveSales.Size = new System.Drawing.Size(136, 23);
            this.btnMoveSales.TabIndex = 77;
            this.btnMoveSales.Text = "Переместить продажи";
            this.btnMoveSales.UseVisualStyleBackColor = true;
            this.btnMoveSales.Visible = false;
            this.btnMoveSales.Click += new System.EventHandler(this.btnMoveSales_Click);
            // 
            // LPU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 478);
            this.Controls.Add(this.btnMoveSales);
            this.Controls.Add(this.btnDelUserLPU);
            this.Controls.Add(this.btnAddUserLPU);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._dgv2);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbSDiv);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._dgv1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbUsers);
            this.Controls.Add(this.cbRegions);
            this.Controls.Add(this.label1);
            this.Name = "LPU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список ЛПУ";
            this.Activated += new System.EventHandler(this.LPU_Activated);
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbUsers;
        private System.Windows.Forms.ComboBox cbRegions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView _dgv1;
        private System.Windows.Forms.ComboBox cbSDiv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView _dgv2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddUserLPU;
        private System.Windows.Forms.Button btnDelUserLPU;
        private System.Windows.Forms.Button btnMoveSales;
    }
}