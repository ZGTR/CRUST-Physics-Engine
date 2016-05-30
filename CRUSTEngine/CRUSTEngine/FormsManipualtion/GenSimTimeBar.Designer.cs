namespace CRUSTEngine.FormsManipualtion
{
    partial class GenSimTimeBarForm
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
            this.trbrTime = new System.Windows.Forms.TrackBar();
            this.tbMinTS = new System.Windows.Forms.TextBox();
            this.lMinTS = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMaxTS = new System.Windows.Forms.TextBox();
            this.bSetParamsTS = new System.Windows.Forms.Button();
            this.lbTSValue = new System.Windows.Forms.Label();
            this.panetlTS = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bDeleteComp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbCompType = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.bRunAgent = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.bMoveMode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trbrTime)).BeginInit();
            this.panetlTS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // trbrTime
            // 
            this.trbrTime.Location = new System.Drawing.Point(12, 212);
            this.trbrTime.Maximum = 25000;
            this.trbrTime.Minimum = 10000;
            this.trbrTime.Name = "trbrTime";
            this.trbrTime.Size = new System.Drawing.Size(622, 42);
            this.trbrTime.SmallChange = 500;
            this.trbrTime.TabIndex = 0;
            this.trbrTime.TickFrequency = 200;
            this.trbrTime.Value = 10000;
            this.trbrTime.Visible = false;
            this.trbrTime.Scroll += new System.EventHandler(this.tbTime_Scroll);
            // 
            // tbMinTS
            // 
            this.tbMinTS.Location = new System.Drawing.Point(94, 76);
            this.tbMinTS.Name = "tbMinTS";
            this.tbMinTS.Size = new System.Drawing.Size(52, 20);
            this.tbMinTS.TabIndex = 1;
            this.tbMinTS.Text = "300";
            // 
            // lMinTS
            // 
            this.lMinTS.AutoSize = true;
            this.lMinTS.Location = new System.Drawing.Point(5, 79);
            this.lMinTS.Name = "lMinTS";
            this.lMinTS.Size = new System.Drawing.Size(83, 13);
            this.lMinTS.TabIndex = 3;
            this.lMinTS.Text = "Min Time Stamp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max Time Stamp";
            // 
            // tbMaxTS
            // 
            this.tbMaxTS.Location = new System.Drawing.Point(244, 75);
            this.tbMaxTS.Name = "tbMaxTS";
            this.tbMaxTS.Size = new System.Drawing.Size(52, 20);
            this.tbMaxTS.TabIndex = 5;
            this.tbMaxTS.Text = "600";
            // 
            // bSetParamsTS
            // 
            this.bSetParamsTS.Location = new System.Drawing.Point(299, 72);
            this.bSetParamsTS.Name = "bSetParamsTS";
            this.bSetParamsTS.Size = new System.Drawing.Size(81, 26);
            this.bSetParamsTS.TabIndex = 6;
            this.bSetParamsTS.Text = "Set Params";
            this.bSetParamsTS.UseVisualStyleBackColor = true;
            this.bSetParamsTS.Click += new System.EventHandler(this.bSetParamsTS_Click);
            // 
            // lbTSValue
            // 
            this.lbTSValue.AutoSize = true;
            this.lbTSValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTSValue.Location = new System.Drawing.Point(505, 16);
            this.lbTSValue.Name = "lbTSValue";
            this.lbTSValue.Size = new System.Drawing.Size(35, 18);
            this.lbTSValue.TabIndex = 7;
            this.lbTSValue.Text = "300";
            // 
            // panetlTS
            // 
            this.panetlTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panetlTS.Controls.Add(this.pictureBox1);
            this.panetlTS.Location = new System.Drawing.Point(5, 36);
            this.panetlTS.Name = "panetlTS";
            this.panetlTS.Size = new System.Drawing.Size(621, 30);
            this.panetlTS.TabIndex = 8;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(-1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(621, 30);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(454, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "Cursor Pos:";
            // 
            // bDeleteComp
            // 
            this.bDeleteComp.Location = new System.Drawing.Point(386, 72);
            this.bDeleteComp.Name = "bDeleteComp";
            this.bDeleteComp.Size = new System.Drawing.Size(85, 26);
            this.bDeleteComp.TabIndex = 12;
            this.bDeleteComp.Text = "Deletion Off";
            this.bDeleteComp.UseVisualStyleBackColor = true;
            this.bDeleteComp.Click += new System.EventHandler(this.bDeleteComp_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbCompType);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.bRunAgent);
            this.panel1.Location = new System.Drawing.Point(634, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(255, 90);
            this.panel1.TabIndex = 14;
            // 
            // lbCompType
            // 
            this.lbCompType.AutoSize = true;
            this.lbCompType.Location = new System.Drawing.Point(5, 10);
            this.lbCompType.Name = "lbCompType";
            this.lbCompType.Size = new System.Drawing.Size(88, 13);
            this.lbCompType.TabIndex = 12;
            this.lbCompType.Text = "Component Type";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Rope Cut",
            "Rocket Press",
            "Air-Cushion Press",
            "Bubble Pop",
            "Bumper Interaction",
            "Om Nom Feed"});
            this.comboBox1.Location = new System.Drawing.Point(4, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(247, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // bRunAgent
            // 
            this.bRunAgent.Location = new System.Drawing.Point(4, 54);
            this.bRunAgent.Name = "bRunAgent";
            this.bRunAgent.Size = new System.Drawing.Size(247, 31);
            this.bRunAgent.TabIndex = 10;
            this.bRunAgent.Text = "Run Agent";
            this.bRunAgent.UseVisualStyleBackColor = true;
            this.bRunAgent.Click += new System.EventHandler(this.bRunAgent_Click);
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(557, 72);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(70, 26);
            this.bClear.TabIndex = 15;
            this.bClear.Text = "Clear TS";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // bMoveMode
            // 
            this.bMoveMode.Location = new System.Drawing.Point(473, 72);
            this.bMoveMode.Name = "bMoveMode";
            this.bMoveMode.Size = new System.Drawing.Size(82, 26);
            this.bMoveMode.TabIndex = 16;
            this.bMoveMode.Text = "Move Off";
            this.bMoveMode.UseVisualStyleBackColor = true;
            this.bMoveMode.Click += new System.EventHandler(this.bMoveMode_Click);
            // 
            // GenSimTimeBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 103);
            this.Controls.Add(this.bMoveMode);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bDeleteComp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panetlTS);
            this.Controls.Add(this.lbTSValue);
            this.Controls.Add(this.bSetParamsTS);
            this.Controls.Add(this.tbMaxTS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lMinTS);
            this.Controls.Add(this.tbMinTS);
            this.Controls.Add(this.trbrTime);
            this.Location = new System.Drawing.Point(310, 570);
            this.Name = "GenSimTimeBarForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GenSimTimeBarForm";
            ((System.ComponentModel.ISupportInitialize)(this.trbrTime)).EndInit();
            this.panetlTS.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trbrTime;
        private System.Windows.Forms.TextBox tbMinTS;
        private System.Windows.Forms.Label lMinTS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMaxTS;
        private System.Windows.Forms.Button bSetParamsTS;
        private System.Windows.Forms.Label lbTSValue;
        private System.Windows.Forms.Panel panetlTS;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bDeleteComp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbCompType;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button bRunAgent;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.Button bMoveMode;
    }
}