namespace RegionR.Directories
{
    partial class RegionForm
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
            this.dgvRegion = new System.Windows.Forms.DataGridView();
            this.add = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegion)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRegion
            // 
            this.dgvRegion.AllowUserToAddRows = false;
            this.dgvRegion.AllowUserToDeleteRows = false;
            this.dgvRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRegion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegion.Location = new System.Drawing.Point(12, 40);
            this.dgvRegion.MultiSelect = false;
            this.dgvRegion.Name = "dgvRegion";
            this.dgvRegion.ReadOnly = true;
            this.dgvRegion.RowHeadersVisible = false;
            this.dgvRegion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRegion.Size = new System.Drawing.Size(572, 369);
            this.dgvRegion.TabIndex = 1;
            this.dgvRegion.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRegion_CellDoubleClick);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(12, 11);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(75, 23);
            this.add.TabIndex = 2;
            this.add.Text = "Добавить";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // RegionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 421);
            this.Controls.Add(this.add);
            this.Controls.Add(this.dgvRegion);
            this.Name = "RegionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник \"Регионы\"";
            this.Activated += new System.EventHandler(this.RegionForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRegion;
        private System.Windows.Forms.Button add;
    }
}