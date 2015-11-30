namespace RegionR
{
    partial class FirstPage
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
            this.btnRR = new System.Windows.Forms.Button();
            this.btnFR = new System.Windows.Forms.Button();
            this.lbUserName = new System.Windows.Forms.Label();
            this.lbChoose = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRR
            // 
            this.btnRR.Location = new System.Drawing.Point(128, 71);
            this.btnRR.Name = "btnRR";
            this.btnRR.Size = new System.Drawing.Size(115, 33);
            this.btnRR.TabIndex = 0;
            this.btnRR.Text = "Regional Report";
            this.btnRR.UseVisualStyleBackColor = true;
            this.btnRR.Click += new System.EventHandler(this.btnRR_Click);
            // 
            // btnFR
            // 
            this.btnFR.Location = new System.Drawing.Point(284, 71);
            this.btnFR.Name = "btnFR";
            this.btnFR.Size = new System.Drawing.Size(115, 33);
            this.btnFR.TabIndex = 1;
            this.btnFR.Text = "Finance Report";
            this.btnFR.UseVisualStyleBackColor = true;
            this.btnFR.Click += new System.EventHandler(this.btnFR_Click);
            this.btnFR.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnFR_MouseMove);
            // 
            // lbUserName
            // 
            this.lbUserName.AutoSize = true;
            this.lbUserName.Location = new System.Drawing.Point(12, 14);
            this.lbUserName.Name = "lbUserName";
            this.lbUserName.Size = new System.Drawing.Size(114, 13);
            this.lbUserName.TabIndex = 2;
            this.lbUserName.Text = "Здравствуйте, гость.";
            // 
            // lbChoose
            // 
            this.lbChoose.AutoSize = true;
            this.lbChoose.Location = new System.Drawing.Point(12, 37);
            this.lbChoose.Name = "lbChoose";
            this.lbChoose.Size = new System.Drawing.Size(263, 13);
            this.lbChoose.TabIndex = 3;
            this.lbChoose.Text = "Выберите систему, в которой вы хотите работать.";
            this.lbChoose.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Teal;
            this.label1.Location = new System.Drawing.Point(103, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(332, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Вы хотите удалить Regional Report?";
            this.label1.Visible = false;
            // 
            // FirstPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 122);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbChoose);
            this.Controls.Add(this.lbUserName);
            this.Controls.Add(this.btnFR);
            this.Controls.Add(this.btnRR);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FirstPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор системы";
            this.Load += new System.EventHandler(this.FirstPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRR;
        private System.Windows.Forms.Button btnFR;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.Label lbChoose;
        private System.Windows.Forms.Label label1;
    }
}