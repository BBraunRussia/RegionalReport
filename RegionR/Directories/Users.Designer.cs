namespace RegionR.Directories
{
    partial class Users
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
            this._dgv1 = new System.Windows.Forms.DataGridView();
            this.cbRegions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this._dgv2 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelUserLPU = new System.Windows.Forms.Button();
            this.btnAddUserLPU = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRoles = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSDiv = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
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
            this._dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgv1.Location = new System.Drawing.Point(10, 71);
            this._dgv1.Name = "_dgv1";
            this._dgv1.ReadOnly = true;
            this._dgv1.RowHeadersVisible = false;
            this._dgv1.Size = new System.Drawing.Size(278, 398);
            this._dgv1.TabIndex = 0;
            this._dgv1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // cbRegions
            // 
            this.cbRegions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegions.FormattingEnabled = true;
            this.cbRegions.Location = new System.Drawing.Point(343, 22);
            this.cbRegions.Name = "cbRegions";
            this.cbRegions.Size = new System.Drawing.Size(219, 21);
            this.cbRegions.TabIndex = 1;
            this.cbRegions.SelectedIndexChanged += new System.EventHandler(this.cbRegions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(340, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Регион:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(213, 18);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            this._dgv2.Location = new System.Drawing.Point(343, 71);
            this._dgv2.Name = "_dgv2";
            this._dgv2.ReadOnly = true;
            this._dgv2.RowHeadersVisible = false;
            this._dgv2.Size = new System.Drawing.Size(354, 398);
            this._dgv2.TabIndex = 6;
            this._dgv2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dgv2_CellDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Пользователи:";
            // 
            // btnDelUserLPU
            // 
            this.btnDelUserLPU.Location = new System.Drawing.Point(294, 158);
            this.btnDelUserLPU.Name = "btnDelUserLPU";
            this.btnDelUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnDelUserLPU.TabIndex = 78;
            this.btnDelUserLPU.Text = "<<";
            this.btnDelUserLPU.UseVisualStyleBackColor = true;
            this.btnDelUserLPU.Click += new System.EventHandler(this.btnDelUserLPU_Click);
            // 
            // btnAddUserLPU
            // 
            this.btnAddUserLPU.Location = new System.Drawing.Point(294, 94);
            this.btnAddUserLPU.Name = "btnAddUserLPU";
            this.btnAddUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnAddUserLPU.TabIndex = 77;
            this.btnAddUserLPU.Text = ">>";
            this.btnAddUserLPU.UseVisualStyleBackColor = true;
            this.btnAddUserLPU.Click += new System.EventHandler(this.btnAddUserLPU_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 79;
            this.label3.Text = "Роль:";
            // 
            // cbRoles
            // 
            this.cbRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRoles.FormattingEnabled = true;
            this.cbRoles.Location = new System.Drawing.Point(10, 20);
            this.cbRoles.Name = "cbRoles";
            this.cbRoles.Size = new System.Drawing.Size(197, 21);
            this.cbRoles.TabIndex = 80;
            this.cbRoles.SelectedIndexChanged += new System.EventHandler(this.cbRoles_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(340, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 81;
            this.label4.Text = "Пользователи региона:";
            // 
            // cbSDiv
            // 
            this.cbSDiv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSDiv.FormattingEnabled = true;
            this.cbSDiv.Items.AddRange(new object[] {
            "HC",
            "AE",
            "OM"});
            this.cbSDiv.Location = new System.Drawing.Point(581, 22);
            this.cbSDiv.Name = "cbSDiv";
            this.cbSDiv.Size = new System.Drawing.Size(57, 21);
            this.cbSDiv.TabIndex = 83;
            this.cbSDiv.SelectedIndexChanged += new System.EventHandler(this.cbSDiv_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(577, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 82;
            this.label5.Text = "Дивизион:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(633, 484);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 85;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // Users
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 519);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbSDiv);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbRoles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDelUserLPU);
            this.Controls.Add(this.btnAddUserLPU);
            this.Controls.Add(this._dgv2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRegions);
            this.Controls.Add(this._dgv1);
            this.Name = "Users";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник пользователей";
            this.Activated += new System.EventHandler(this.Users_Activated);
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _dgv1;
        private System.Windows.Forms.ComboBox cbRegions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView _dgv2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDelUserLPU;
        private System.Windows.Forms.Button btnAddUserLPU;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRoles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbSDiv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClose;
    }
}