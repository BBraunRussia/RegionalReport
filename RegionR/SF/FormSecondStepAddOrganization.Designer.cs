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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvLpuRR = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvLPUCompetitors = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDeleteFilter = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnNextNotCopyLPU = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLpuRR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLPUCompetitors)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "1. Выберите ЛПУ из справочника Regional Report";
            // 
            // dgvLpuRR
            // 
            this.dgvLpuRR.AllowUserToAddRows = false;
            this.dgvLpuRR.AllowUserToDeleteRows = false;
            this.dgvLpuRR.AllowUserToResizeRows = false;
            this.dgvLpuRR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLpuRR.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLpuRR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLpuRR.Location = new System.Drawing.Point(12, 46);
            this.dgvLpuRR.MultiSelect = false;
            this.dgvLpuRR.Name = "dgvLpuRR";
            this.dgvLpuRR.ReadOnly = true;
            this.dgvLpuRR.RowHeadersVisible = false;
            this.dgvLpuRR.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvLpuRR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLpuRR.Size = new System.Drawing.Size(753, 134);
            this.dgvLpuRR.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(255, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "2. Подберите ЛПУ из базы CONAN";
            // 
            // dgvLPUCompetitors
            // 
            this.dgvLPUCompetitors.AllowUserToAddRows = false;
            this.dgvLPUCompetitors.AllowUserToDeleteRows = false;
            this.dgvLPUCompetitors.AllowUserToResizeRows = false;
            this.dgvLPUCompetitors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLPUCompetitors.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLPUCompetitors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLPUCompetitors.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvLPUCompetitors.Location = new System.Drawing.Point(12, 263);
            this.dgvLPUCompetitors.MultiSelect = false;
            this.dgvLPUCompetitors.Name = "dgvLPUCompetitors";
            this.dgvLPUCompetitors.ReadOnly = true;
            this.dgvLPUCompetitors.RowHeadersVisible = false;
            this.dgvLPUCompetitors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvLPUCompetitors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvLPUCompetitors.Size = new System.Drawing.Size(753, 243);
            this.dgvLPUCompetitors.TabIndex = 3;
            this.dgvLPUCompetitors.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.dgvLPUCompetitors_CellContextMenuStripNeeded);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterToolStripMenuItem,
            this.sortToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(261, 48);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.filterToolStripMenuItem.Text = "Фильтр по значению этой ячейки";
            this.filterToolStripMenuItem.Click += new System.EventHandler(this.filterToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.sortToolStripMenuItem.Text = "Сортировка";
            this.sortToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(41, 512);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Далее";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(371, 513);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(338, 236);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(159, 20);
            this.tbSearch.TabIndex = 7;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(290, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Поиск:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(292, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "(для копирования ИНН, КПП и официального названия)";
            // 
            // btnDeleteFilter
            // 
            this.btnDeleteFilter.Location = new System.Drawing.Point(15, 234);
            this.btnDeleteFilter.Name = "btnDeleteFilter";
            this.btnDeleteFilter.Size = new System.Drawing.Size(90, 23);
            this.btnDeleteFilter.TabIndex = 10;
            this.btnDeleteFilter.Text = "Снять фильтр";
            this.btnDeleteFilter.UseVisualStyleBackColor = true;
            this.btnDeleteFilter.Visible = false;
            this.btnDeleteFilter.Click += new System.EventHandler(this.btnDeleteFilter_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(12, 513);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "3.";
            // 
            // btnNextNotCopyLPU
            // 
            this.btnNextNotCopyLPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNextNotCopyLPU.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnNextNotCopyLPU.Location = new System.Drawing.Point(122, 513);
            this.btnNextNotCopyLPU.Name = "btnNextNotCopyLPU";
            this.btnNextNotCopyLPU.Size = new System.Drawing.Size(243, 23);
            this.btnNextNotCopyLPU.TabIndex = 12;
            this.btnNextNotCopyLPU.Text = "Далее (ЛПУ отсутствует в базе CONAN)";
            this.btnNextNotCopyLPU.UseVisualStyleBackColor = true;
            this.btnNextNotCopyLPU.Click += new System.EventHandler(this.btnNextNotCopyLPU_Click);
            // 
            // FormSecondStepAddOrganization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 547);
            this.Controls.Add(this.btnNextNotCopyLPU);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnDeleteFilter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.label3);
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
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDeleteFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnNextNotCopyLPU;
    }
}