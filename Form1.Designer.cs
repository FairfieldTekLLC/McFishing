namespace Fisher
{
    partial class Form1
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
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAdjustX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAdjustY = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(12, 12);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(175, 83);
            this.txtConsole.TabIndex = 2;
            // 
            // pbPreview
            // 
            this.pbPreview.Location = new System.Drawing.Point(193, 12);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(204, 177);
            this.pbPreview.TabIndex = 3;
            this.pbPreview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Press Ctrl F2 to Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Press Ctrl F3 To Stop";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Adjust X";
            // 
            // txtAdjustX
            // 
            this.txtAdjustX.Location = new System.Drawing.Point(68, 101);
            this.txtAdjustX.Name = "txtAdjustX";
            this.txtAdjustX.Size = new System.Drawing.Size(100, 20);
            this.txtAdjustX.TabIndex = 7;
            this.txtAdjustX.Text = "0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Adjust Y";
            // 
            // txtAdjustY
            // 
            this.txtAdjustY.Location = new System.Drawing.Point(68, 127);
            this.txtAdjustY.Name = "txtAdjustY";
            this.txtAdjustY.Size = new System.Drawing.Size(100, 20);
            this.txtAdjustY.TabIndex = 9;
            this.txtAdjustY.Text = "0";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(16, 176);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(123, 13);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status: Not Started";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 197);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtAdjustY);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAdjustX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.txtConsole);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAdjustX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAdjustY;
        private System.Windows.Forms.Label lblStatus;
    }
}

