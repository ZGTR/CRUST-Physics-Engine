namespace CRUSTEngine.FormsManipualtion
{
    partial class SetLevelDialog
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
            this.bUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxLevelString = new System.Windows.Forms.TextBox();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bUpload
            // 
            this.bUpload.Location = new System.Drawing.Point(668, 12);
            this.bUpload.Name = "bUpload";
            this.bUpload.Size = new System.Drawing.Size(106, 35);
            this.bUpload.TabIndex = 2;
            this.bUpload.Text = "Upload from File";
            this.bUpload.UseVisualStyleBackColor = true;
            this.bUpload.Click += new System.EventHandler(this.bUpload_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Level String";
            // 
            // tbxLevelString
            // 
            this.tbxLevelString.Location = new System.Drawing.Point(81, 12);
            this.tbxLevelString.Multiline = true;
            this.tbxLevelString.Name = "tbxLevelString";
            this.tbxLevelString.Size = new System.Drawing.Size(581, 35);
            this.tbxLevelString.TabIndex = 2;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(556, 62);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(106, 25);
            this.bOk.TabIndex = 0;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(668, 62);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(106, 25);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // SetLevelDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 94);
            this.Controls.Add(this.tbxLevelString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.bUpload);
            this.Name = "SetLevelDialog";
            this.Text = "SetLevelDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bUpload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxLevelString;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
    }
}