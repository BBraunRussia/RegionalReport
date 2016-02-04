namespace RegionR.SF
{
    partial class FormOrganizationWithLpuRRList
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
            this.btnReload = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btnExportInExcel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fiterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDeleteFilter
            // 
            this.btnDeleteFilter.Location = new System.Drawing.Point(133, 6);
            this.btnDeleteFilter.Name = "btnDeleteFilter";
            this.btnDeleteFilter.Size = new System.Drawing.Size(100, 23);
            this.btnDeleteFilter.TabIndex = 15;
            this.btnDeleteFilter.Text = "Снять фильтры";
            this.btnDeleteFilter.UseVisualStyleBackColor = true;
            this.btnDeleteFilter.Visible = false;
            this.btnDeleteFilter.Click += new System.EventHandler(this.btnDeleteFilter_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(915, 6);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 14;
            this.btnReload.Text = "Обновить";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(310, 8);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(159, 20);
            this.tbSearch.TabIndex = 13;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(262, 11);
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
            this.dgv.Location = new System.Drawing.Point(12, 36);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(978, 364);
            this.dgv.TabIndex = 9;
            this.dgv.Sorted += new System.EventHandler(this.dgv_Sorted);
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
            this.statusStrip1.Size = new System.Drawing.Size(1002, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
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
            // FormOrganizationWithLpuRRList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 425);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExportInExcel);
            this.Controls.Add(this.btnDeleteFilter);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv);
            this.Name = "FormOrganizationWithLpuRRList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Отчёт: Сопоставление ЛПУ RR и SF";
            this.Load += new System.EventHandler(this.FormOrganizationWithLpuRRList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDeleteFilter;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnExportInExcel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fiterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
    }
}