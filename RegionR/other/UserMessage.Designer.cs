namespace RegionR.other
{
    partial class UserMessage
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
            this.cbRead = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbMesTxt = new System.Windows.Forms.TextBox();
            this.lbUserName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbRead
            // 
            this.cbRead.AutoSize = true;
            this.cbRead.Location = new System.Drawing.Point(13, 203);
            this.cbRead.Name = "cbRead";
            this.cbRead.Size = new System.Drawing.Size(144, 17);
            this.cbRead.TabIndex = 0;
            this.cbRead.Text = "Больше не показывать";
            this.cbRead.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(375, 197);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbMesTxt
            // 
            this.tbMesTxt.Location = new System.Drawing.Point(12, 25);
            this.tbMesTxt.Multiline = true;
            this.tbMesTxt.Name = "tbMesTxt";
            this.tbMesTxt.ReadOnly = true;
            this.tbMesTxt.Size = new System.Drawing.Size(438, 166);
            this.tbMesTxt.TabIndex = 2;
            // 
            // lbUserName
            // 
            this.lbUserName.AutoSize = true;
            this.lbUserName.Location = new System.Drawing.Point(9, 9);
            this.lbUserName.Name = "lbUserName";
            this.lbUserName.Size = new System.Drawing.Size(40, 13);
            this.lbUserName.TabIndex = 3;
            this.lbUserName.Text = "Автор:";
            // 
            // UserMessage
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 229);
            this.Controls.Add(this.lbUserName);
            this.Controls.Add(this.tbMesTxt);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbRead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сообщение";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbRead;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbMesTxt;
        private System.Windows.Forms.Label lbUserName;
    }
}