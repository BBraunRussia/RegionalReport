namespace RegionR.SF
{
    partial class FormSecondStepAddOrganization
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
            this.label1 = new System.Windows.Forms.Label();
            this.dgvLpuRR = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvLPUCompetitors = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLpuRR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLPUCompetitors)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(315, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выбор ЛПУ из справочника Regional Report";
            // 
            // dgvLpuRR
            // 
            this.dgvLpuRR.AllowUserToAddRows = false;
            this.dgvLpuRR.AllowUserToDeleteRows = false;
            this.dgvLpuRR.AllowUserToResizeRows = false;
            this.dgvLpuRR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLpuRR.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLpuRR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLpuRR.Location = new System.Drawing.Point(12, 30);
            this.dgvLpuRR.MultiSelect = false;
            this.dgvLpuRR.Name = "dgvLpuRR";
            this.dgvLpuRR.ReadOnly = true;
            this.dgvLpuRR.RowHeadersVisible = false;
            this.dgvLpuRR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLpuRR.Size = new System.Drawing.Size(545, 150);
            this.dgvLpuRR.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Выбор ЛПУ из базы CONAN";
            // 
            // dgvLPUCompetitors
            // 
            this.dgvLPUCompetitors.AllowUserToAddRows = false;
            this.dgvLPUCompetitors.AllowUserToDeleteRows = false;
            this.dgvLPUCompetitors.AllowUserToResizeRows = false;
            this.dgvLPUCompetitors.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLPUCompetitors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLPUCompetitors.Location = new System.Drawing.Point(12, 214);
            this.dgvLPUCompetitors.MultiSelect = false;
            this.dgvLPUCompetitors.Name = "dgvLPUCompetitors";
            this.dgvLPUCompetitors.ReadOnly = true;
            this.dgvLPUCompetitors.RowHeadersVisible = false;
            this.dgvLPUCompetitors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLPUCompetitors.Size = new System.Drawing.Size(545, 150);
            this.dgvLPUCompetitors.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(401, 383);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Далее";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(482, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormSecondStepAddOrganization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 418);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvLPUCompetitors);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvLpuRR);
            this.Controls.Add(this.label1);
            this.Name = "FormSecondStepAddOrganization";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавление Организации-SF, тип \"ЛПУ\"";
            this.Load += new System.EventHandler(this.FormSecondStepAddOrganization_Load);
            this.Resize += new System.EventHandler(this.FormSecondStepAddOrganization_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLpuRR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLPUCompetitors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvLpuRR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvLPUCompetitors;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}