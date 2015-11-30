namespace RegionR.Directories
{
    partial class userManager
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
            this.btnDelUserLPU = new System.Windows.Forms.Button();
            this.btnAddUserLPU = new System.Windows.Forms.Button();
            this._dgv2 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this._dgv1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.manager = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelUserLPU
            // 
            this.btnDelUserLPU.Location = new System.Drawing.Point(296, 146);
            this.btnDelUserLPU.Name = "btnDelUserLPU";
            this.btnDelUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnDelUserLPU.TabIndex = 83;
            this.btnDelUserLPU.Text = "<<";
            this.btnDelUserLPU.UseVisualStyleBackColor = true;
            this.btnDelUserLPU.Click += new System.EventHandler(this.btnDelUserLPU_Click);
            // 
            // btnAddUserLPU
            // 
            this.btnAddUserLPU.Location = new System.Drawing.Point(296, 82);
            this.btnAddUserLPU.Name = "btnAddUserLPU";
            this.btnAddUserLPU.Size = new System.Drawing.Size(43, 23);
            this.btnAddUserLPU.TabIndex = 82;
            this.btnAddUserLPU.Text = ">>";
            this.btnAddUserLPU.UseVisualStyleBackColor = true;
            this.btnAddUserLPU.Click += new System.EventHandler(this.btnAddUserLPU_Click);
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
            this._dgv2.Location = new System.Drawing.Point(345, 23);
            this._dgv2.Name = "_dgv2";
            this._dgv2.ReadOnly = true;
            this._dgv2.RowHeadersVisible = false;
            this._dgv2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._dgv2.Size = new System.Drawing.Size(347, 437);
            this._dgv2.TabIndex = 80;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 81;
            this.label2.Text = "Менеджер:";
            // 
            // _dgv1
            // 
            this._dgv1.AllowUserToAddRows = false;
            this._dgv1.AllowUserToDeleteRows = false;
            this._dgv1.AllowUserToResizeRows = false;
            this._dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgv1.Location = new System.Drawing.Point(12, 71);
            this._dgv1.Name = "_dgv1";
            this._dgv1.ReadOnly = true;
            this._dgv1.RowHeadersVisible = false;
            this._dgv1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._dgv1.Size = new System.Drawing.Size(278, 389);
            this._dgv1.TabIndex = 79;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(342, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 84;
            this.label1.Text = "Пользователи менеджера:";
            // 
            // manager
            // 
            this.manager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manager.FormattingEnabled = true;
            this.manager.Location = new System.Drawing.Point(13, 23);
            this.manager.Name = "manager";
            this.manager.Size = new System.Drawing.Size(277, 21);
            this.manager.TabIndex = 85;
            this.manager.SelectedIndexChanged += new System.EventHandler(this.manager_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 86;
            this.label3.Text = "Список пользователей:";
            // 
            // userManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 472);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.manager);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelUserLPU);
            this.Controls.Add(this.btnAddUserLPU);
            this.Controls.Add(this._dgv2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._dgv1);
            this.Name = "userManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Пользователи Менеджера";
            ((System.ComponentModel.ISupportInitialize)(this._dgv2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelUserLPU;
        private System.Windows.Forms.Button btnAddUserLPU;
        private System.Windows.Forms.DataGridView _dgv2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView _dgv1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox manager;
        private System.Windows.Forms.Label label3;
    }
}