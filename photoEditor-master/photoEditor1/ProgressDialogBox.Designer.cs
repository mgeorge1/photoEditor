namespace photoEditor1
{
    partial class ProgressDialogBox
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressDialogBoxCancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(57, 44);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 0;
            // 
            // progressDialogBoxCancelButton
            // 
            this.progressDialogBoxCancelButton.Location = new System.Drawing.Point(70, 82);
            this.progressDialogBoxCancelButton.Name = "progressDialogBoxCancelButton";
            this.progressDialogBoxCancelButton.Size = new System.Drawing.Size(75, 23);
            this.progressDialogBoxCancelButton.TabIndex = 1;
            this.progressDialogBoxCancelButton.Text = "Cancel";
            this.progressDialogBoxCancelButton.UseVisualStyleBackColor = true;
            this.progressDialogBoxCancelButton.Click += new System.EventHandler(this.ProgressDialogBoxCancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please wait...";
            // 
            // ProgressDialogBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 129);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressDialogBoxCancelButton);
            this.Controls.Add(this.progressBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressDialogBox";
            this.Text = "Transforming";
            this.Load += new System.EventHandler(this.ProgressDialogBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button progressDialogBoxCancelButton;
        private System.Windows.Forms.Label label1;
    }
}