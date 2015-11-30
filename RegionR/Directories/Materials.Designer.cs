namespace RegionR.Directories
{
    partial class Materials
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.tbMat = new System.Windows.Forms.TextBox();
            this.lbArt = new System.Windows.Forms.Label();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn7 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btnOM = new System.Windows.Forms.Button();
            this.btnAE = new System.Windows.Forms.Button();
            this.btnHC = new System.Windows.Forms.Button();
            this.btnNull = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).BeginInit();
            this.SuspendLayout();
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
            this._dgv1.Location = new System.Drawing.Point(12, 140);
            this._dgv1.Name = "_dgv1";
            this._dgv1.ReadOnly = true;
            this._dgv1.RowHeadersVisible = false;
            this._dgv1.Size = new System.Drawing.Size(730, 326);
            this._dgv1.TabIndex = 7;
            this._dgv1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this._dgv1_CellMouseDoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(667, 472);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 86;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(202, 108);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 87;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(626, 109);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(116, 23);
            this.btnExcel.TabIndex = 103;
            this.btnExcel.Text = "Выгрузка в Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(121, 109);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 102;
            this.btnFind.Text = "Поиск";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbMat
            // 
            this.tbMat.Location = new System.Drawing.Point(15, 111);
            this.tbMat.Name = "tbMat";
            this.tbMat.Size = new System.Drawing.Size(100, 20);
            this.tbMat.TabIndex = 100;
            this.tbMat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMat_KeyDown);
            // 
            // lbArt
            // 
            this.lbArt.AutoSize = true;
            this.lbArt.Location = new System.Drawing.Point(12, 95);
            this.lbArt.Name = "lbArt";
            this.lbArt.Size = new System.Drawing.Size(51, 13);
            this.lbArt.TabIndex = 99;
            this.lbArt.Text = "Артикул:";
            // 
            // btn8
            // 
            this.btn8.Location = new System.Drawing.Point(495, 55);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(63, 37);
            this.btn8.TabIndex = 98;
            this.btn8.Text = "AM";
            this.btn8.UseVisualStyleBackColor = true;
            this.btn8.Visible = false;
            this.btn8.Click += new System.EventHandler(this.btn8_Click);
            // 
            // btn7
            // 
            this.btn7.Location = new System.Drawing.Point(426, 55);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(63, 37);
            this.btn7.TabIndex = 97;
            this.btn7.Text = "VS";
            this.btn7.UseVisualStyleBackColor = true;
            this.btn7.Visible = false;
            this.btn7.Click += new System.EventHandler(this.btn7_Click);
            // 
            // btn6
            // 
            this.btn6.Location = new System.Drawing.Point(357, 55);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(63, 37);
            this.btn6.TabIndex = 96;
            this.btn6.Text = "CT";
            this.btn6.UseVisualStyleBackColor = true;
            this.btn6.Visible = false;
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // btn5
            // 
            this.btn5.Location = new System.Drawing.Point(288, 55);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(63, 37);
            this.btn5.TabIndex = 95;
            this.btn5.Text = "SP";
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Visible = false;
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // btn4
            // 
            this.btn4.Location = new System.Drawing.Point(219, 55);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(63, 37);
            this.btn4.TabIndex = 94;
            this.btn4.Text = "ICU";
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Visible = false;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn2
            // 
            this.btn2.Location = new System.Drawing.Point(81, 55);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(63, 37);
            this.btn2.TabIndex = 92;
            this.btn2.Text = "CC";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn3
            // 
            this.btn3.Location = new System.Drawing.Point(150, 55);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(63, 37);
            this.btn3.TabIndex = 93;
            this.btn3.Text = "CN";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(12, 55);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(63, 37);
            this.btn1.TabIndex = 91;
            this.btn1.Text = "BC";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btnOM
            // 
            this.btnOM.Location = new System.Drawing.Point(150, 12);
            this.btnOM.Name = "btnOM";
            this.btnOM.Size = new System.Drawing.Size(63, 37);
            this.btnOM.TabIndex = 90;
            this.btnOM.Text = "OM";
            this.btnOM.UseVisualStyleBackColor = true;
            this.btnOM.Click += new System.EventHandler(this.btnOM_Click);
            // 
            // btnAE
            // 
            this.btnAE.Location = new System.Drawing.Point(81, 12);
            this.btnAE.Name = "btnAE";
            this.btnAE.Size = new System.Drawing.Size(63, 37);
            this.btnAE.TabIndex = 89;
            this.btnAE.Text = "AE";
            this.btnAE.UseVisualStyleBackColor = true;
            this.btnAE.Click += new System.EventHandler(this.btnAE_Click);
            // 
            // btnHC
            // 
            this.btnHC.Location = new System.Drawing.Point(12, 12);
            this.btnHC.Name = "btnHC";
            this.btnHC.Size = new System.Drawing.Size(63, 37);
            this.btnHC.TabIndex = 88;
            this.btnHC.Text = "HC";
            this.btnHC.UseVisualStyleBackColor = true;
            this.btnHC.Click += new System.EventHandler(this.btnHC_Click);
            // 
            // btnNull
            // 
            this.btnNull.Location = new System.Drawing.Point(219, 12);
            this.btnNull.Name = "btnNull";
            this.btnNull.Size = new System.Drawing.Size(63, 37);
            this.btnNull.TabIndex = 104;
            this.btnNull.Text = "(нет)";
            this.btnNull.UseVisualStyleBackColor = true;
            this.btnNull.Click += new System.EventHandler(this.btnNull_Click);
            // 
            // Materials
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 507);
            this.Controls.Add(this.btnNull);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.tbMat);
            this.Controls.Add(this.lbArt);
            this.Controls.Add(this.btn8);
            this.Controls.Add(this.btn7);
            this.Controls.Add(this.btn6);
            this.Controls.Add(this.btn5);
            this.Controls.Add(this.btn4);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.btnOM);
            this.Controls.Add(this.btnAE);
            this.Controls.Add(this.btnHC);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this._dgv1);
            this.Name = "Materials";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список продукции";
            this.Activated += new System.EventHandler(this.Materials_Activated);
            ((System.ComponentModel.ISupportInitialize)(this._dgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView _dgv1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox tbMat;
        private System.Windows.Forms.Label lbArt;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btnOM;
        private System.Windows.Forms.Button btnAE;
        private System.Windows.Forms.Button btnHC;
        private System.Windows.Forms.Button btnNull;
    }
}