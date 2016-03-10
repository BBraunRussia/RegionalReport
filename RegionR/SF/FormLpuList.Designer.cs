namespace RegionR
{
    partial class FormLpuList
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
            this.btnDeleteFilter = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fiterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportInExcel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnShowLPUForEdit = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnShowUserLPU = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDeleteFilter
            // 
            this.btnDeleteFilter.Location = new System.Drawing.Point(341, 5);
            this.btnDeleteFilter.Name = "btnDeleteFilter";
            this.btnDeleteFilter.Size = new System.Drawing.Size(100, 23);
            this.btnDeleteFilter.TabIndex = 15;
            this.btnDeleteFilter.Text = "Снять фильтры";
            this.btnDeleteFilter.UseVisualStyleBackColor = true;
            this.btnDeleteFilter.Visible = false;
            this.btnDeleteFilter.Click += new System.EventHandler(this.btnDeleteFilter_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(676, 8);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(159, 20);
            this.tbSearch.TabIndex = 13;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(628, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Поиск:";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv.Location = new System.Drawing.Point(12, 36);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(1064, 364);
            this.dgv.TabIndex = 9;
            this.dgv.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.dgv_CellContextMenuStripNeeded);
            this.dgv.Sorted += new System.EventHandler(this.dgv_Sorted);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fiterToolStripMenuItem,
            this.sortToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(261, 48);
            // 
            // fiterToolStripMenuItem
            // 
            this.fiterToolStripMenuItem.Name = "fiterToolStripMenuItem";
            this.fiterToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.fiterToolStripMenuItem.Text = "Фильтр по значению этой ячейки";
            this.fiterToolStripMenuItem.Click += new System.EventHandler(this.fiterToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.sortToolStripMenuItem.Text = "Сортировка";
            this.sortToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // btnExportInExcel
            // 
            this.btnExportInExcel.Location = new System.Drawing.Point(12, 6);
            this.btnExportInExcel.Name = "btnExportInExcel";
            this.btnExportInExcel.Size = new System.Drawing.Size(115, 23);
            this.btnExportInExcel.TabIndex = 16;
            this.btnExportInExcel.Text = "Экспорт в Excel";
            this.btnExportInExcel.UseVisualStyleBackColor = true;
            this.btnExportInExcel.Click += new System.EventHandler(this.btnExportInExcel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 403);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1088, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnShowLPUForEdit
            // 
            this.btnShowLPUForEdit.Location = new System.Drawing.Point(133, 7);
            this.btnShowLPUForEdit.Name = "btnShowLPUForEdit";
            this.btnShowLPUForEdit.Size = new System.Drawing.Size(115, 23);
            this.btnShowLPUForEdit.TabIndex = 18;
            this.btnShowLPUForEdit.Text = "Редактировать";
            this.btnShowLPUForEdit.UseVisualStyleBackColor = true;
            this.btnShowLPUForEdit.Click += new System.EventHandler(this.btnShowLPUForEdit_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(841, 7);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(125, 23);
            this.btnContinue.TabIndex = 19;
            this.btnContinue.Text = "Продолжить поиск...";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // btnShowUserLPU
            // 
            this.btnShowUserLPU.BackColor = System.Drawing.Color.Transparent;
            this.btnShowUserLPU.Location = new System.Drawing.Point(254, 6);
            this.btnShowUserLPU.Name = "btnShowUserLPU";
            this.btnShowUserLPU.Size = new System.Drawing.Size(81, 23);
            this.btnShowUserLPU.TabIndex = 20;
            this.btnShowUserLPU.Text = "ЛПУ РП";
            this.btnShowUserLPU.UseVisualStyleBackColor = false;
            this.btnShowUserLPU.Click += new System.EventHandler(this.btnShowUserLPU_Click);
            // 
            // FormLpuList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 425);
            this.Controls.Add(this.btnShowUserLPU);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.btnShowLPUForEdit);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExportInExcel);
            this.Controls.Add(this.btnDeleteFilter);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv);
            this.Name = "FormLpuList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список ЛПУ, на которые производится распределения продаж";
            this.Load += new System.EventHandler(this.FormOrganizationWithLpuRRList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDeleteFilter;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnExportInExcel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fiterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.Button btnShowLPUForEdit;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnShowUserLPU;
    }
}