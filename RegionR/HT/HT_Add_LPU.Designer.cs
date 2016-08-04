namespace RegionR.HT
{
    partial class HT_Add_LPU
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
            this._dgv1 = new System.Windows.Forms.DataGridView();
            this.btnDelUserLPU = new System.Windows.Forms.Button();
            this.btnAddUserLPU = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._dgv2 = new System.Windows.Forms.DataGridView();
            this.cbRegions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).BeginInit();
            this.SuspendLayout();
            // 
            // _dgv1
            // 
            this._dgv1.AllowUserToAddRows = false;
            this._dgv1.AllowUserToDeleteRows = false;
            this._dgv1.AllowUserToResizeRows = false;
            this._dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._dgv1.BackgroundColor = System.Drawing.SystemColors.Window;
            this._dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dgv1.DefaultCellStyle = dataGridViewCellStyle1;
            this._dgv1.Location = new System.Drawing.Point(11, 80);
            this._dgv1.Name = "_dgv1";
            this._dgv1.ReadOnly = true;
            this._dgv1.RowHeadersVisible = false;
            this._dgv1.Size = new System.Drawing.Size(475, 392);
            this._dgv1.TabIndex = 92;
            this._dgv1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this._dgv1_CellMouseDoubleClick);
            // 
            // btnDelUserLPU
            // 
            this.btnDelUserLPU.Location = new System.Drawing.Point(492, 170);
            this.btnDelUserLPU.Name = "btnDelUserLPU";
            this.btnDelUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnDelUserLPU.TabIndex = 90;
            this.btnDelUserLPU.Text = "<<";
            this.btnDelUserLPU.UseVisualStyleBackColor = true;
            this.btnDelUserLPU.Click += new System.EventHandler(this.btnDelUserLPU_Click);
            // 
            // btnAddUserLPU
            // 
            this.btnAddUserLPU.Location = new System.Drawing.Point(492, 106);
            this.btnAddUserLPU.Name = "btnAddUserLPU";
            this.btnAddUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnAddUserLPU.TabIndex = 89;
            this.btnAddUserLPU.Text = ">>";
            this.btnAddUserLPU.UseVisualStyleBackColor = true;
            this.btnAddUserLPU.Click += new System.EventHandler(this.btnAddUserLPU_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(538, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 88;
            this.label5.Text = "ЛПУ ХТ:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 87;
            this.label4.Text = "ЛПУ региона:";
            // 
            // _dgv2
            // 
            this._dgv2.AllowUserToAddRows = false;
            this._dgv2.AllowUserToDeleteRows = false;
            this._dgv2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dgv2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._dgv2.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this._dgv2.ColumnHeadersHeight = 35;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dgv2.DefaultCellStyle = dataGridViewCellStyle3;
            this._dgv2.Location = new System.Drawing.Point(541, 80);
            this._dgv2.Name = "_dgv2";
            this._dgv2.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dgv2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this._dgv2.RowHeadersVisible = false;
            this._dgv2.RowHeadersWidth = 20;
            this._dgv2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._dgv2.Size = new System.Drawing.Size(493, 392);
            this._dgv2.TabIndex = 86;
            // 
            // cbRegions
            // 
            this.cbRegions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegions.FormattingEnabled = true;
            this.cbRegions.Location = new System.Drawing.Point(12, 27);
            this.cbRegions.Name = "cbRegions";
            this.cbRegions.Size = new System.Drawing.Size(196, 21);
            this.cbRegions.TabIndex = 80;
            this.cbRegions.SelectedIndexChanged += new System.EventHandler(this.cbRegions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 94;
            this.label1.Text = "Регион:";
            // 
            // HT_Add_LPU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 484);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._dgv1);
            this.Controls.Add(this.btnDelUserLPU);
            this.Controls.Add(this.btnAddUserLPU);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._dgv2);
            this.Controls.Add(this.cbRegions);
            this.Name = "HT_Add_LPU";
            this.Text = "Добавление ЛПУ в проект ХимиоТерапия";
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _dgv1;
        private System.Windows.Forms.Button btnDelUserLPU;
        private System.Windows.Forms.Button btnAddUserLPU;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView _dgv2;
        private System.Windows.Forms.ComboBox cbRegions;
        private System.Windows.Forms.Label label1;
    }
}