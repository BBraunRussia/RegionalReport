namespace RegionR.other
{
    partial class MoveSales
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
            this.lbLPU = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbUserLPU = new System.Windows.Forms.ComboBox();
            this.btnMoveSales = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Переместить из";
            // 
            // lbLPU
            // 
            this.lbLPU.AutoSize = true;
            this.lbLPU.Location = new System.Drawing.Point(109, 14);
            this.lbLPU.Name = "lbLPU";
            this.lbLPU.Size = new System.Drawing.Size(35, 13);
            this.lbLPU.TabIndex = 1;
            this.lbLPU.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "в ЛПУ";
            // 
            // cbUserLPU
            // 
            this.cbUserLPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserLPU.FormattingEnabled = true;
            this.cbUserLPU.Location = new System.Drawing.Point(63, 43);
            this.cbUserLPU.Name = "cbUserLPU";
            this.cbUserLPU.Size = new System.Drawing.Size(239, 21);
            this.cbUserLPU.TabIndex = 3;
            // 
            // btnMoveSales
            // 
            this.btnMoveSales.Location = new System.Drawing.Point(129, 80);
            this.btnMoveSales.Name = "btnMoveSales";
            this.btnMoveSales.Size = new System.Drawing.Size(92, 23);
            this.btnMoveSales.TabIndex = 4;
            this.btnMoveSales.Text = "Переместить";
            this.btnMoveSales.UseVisualStyleBackColor = true;
            this.btnMoveSales.Click += new System.EventHandler(this.btnMoveSales_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(227, 80);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Отмена";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // MoveSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 115);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnMoveSales);
            this.Controls.Add(this.cbUserLPU);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbLPU);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MoveSales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Перемещение продаж";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbLPU;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbUserLPU;
        private System.Windows.Forms.Button btnMoveSales;
        private System.Windows.Forms.Button btnClose;
    }
}