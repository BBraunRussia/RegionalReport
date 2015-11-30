namespace RegionR.Directories
{
    partial class Material2
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
            this.ss1 = new System.Windows.Forms.StatusStrip();
            this.cbCustomer = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.tbMat = new System.Windows.Forms.TextBox();
            this.lbArt = new System.Windows.Forms.Label();
            this._dgv1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // ss1
            // 
            this.ss1.Location = new System.Drawing.Point(0, 388);
            this.ss1.Name = "ss1";
            this.ss1.Size = new System.Drawing.Size(805, 22);
            this.ss1.TabIndex = 119;
            this.ss1.Text = "statusStrip1";
            // 
            // cbCustomer
            // 
            this.cbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCustomer.FormattingEnabled = true;
            this.cbCustomer.Location = new System.Drawing.Point(12, 7);
            this.cbCustomer.Name = "cbCustomer";
            this.cbCustomer.Size = new System.Drawing.Size(265, 21);
            this.cbCustomer.TabIndex = 118;
            this.cbCustomer.SelectedIndexChanged += new System.EventHandler(this.cbCustomer_SelectedIndexChanged);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(468, 5);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 116;
            this.btnFind.Text = "Поиск";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbMat
            // 
            this.tbMat.Location = new System.Drawing.Point(362, 7);
            this.tbMat.Name = "tbMat";
            this.tbMat.Size = new System.Drawing.Size(100, 20);
            this.tbMat.TabIndex = 115;
            this.tbMat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMat_KeyDown);
            // 
            // lbArt
            // 
            this.lbArt.AutoSize = true;
            this.lbArt.Location = new System.Drawing.Point(305, 10);
            this.lbArt.Name = "lbArt";
            this.lbArt.Size = new System.Drawing.Size(51, 13);
            this.lbArt.TabIndex = 114;
            this.lbArt.Text = "Артикул:";
            // 
            // _dgv1
            // 
            this._dgv1.AllowUserToAddRows = false;
            this._dgv1.AllowUserToDeleteRows = false;
            this._dgv1.AllowUserToResizeRows = false;
            this._dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgv1.Location = new System.Drawing.Point(12, 34);
            this._dgv1.Name = "_dgv1";
            this._dgv1.ReadOnly = true;
            this._dgv1.RowHeadersVisible = false;
            this._dgv1.Size = new System.Drawing.Size(781, 351);
            this._dgv1.TabIndex = 113;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Material2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 410);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.cbCustomer);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.tbMat);
            this.Controls.Add(this.lbArt);
            this.Controls.Add(this._dgv1);
            this.Name = "Material2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Материалы дистрибьюторов";
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ss1;
        private System.Windows.Forms.ComboBox cbCustomer;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox tbMat;
        private System.Windows.Forms.Label lbArt;
        private System.Windows.Forms.DataGridView _dgv1;
        private System.Windows.Forms.Timer timer1;

    }
}