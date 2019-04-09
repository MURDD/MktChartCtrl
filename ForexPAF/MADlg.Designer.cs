namespace ForexPAF
{
    partial class MADlg
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
            this.numPeriod = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.cmbPrice = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // numPeriod
            // 
            this.numPeriod.Location = new System.Drawing.Point(64, 12);
            this.numPeriod.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.numPeriod.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numPeriod.Name = "numPeriod";
            this.numPeriod.Size = new System.Drawing.Size(55, 19);
            this.numPeriod.TabIndex = 0;
            this.numPeriod.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Period";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Method";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(267, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(349, 94);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 24);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Price";
            // 
            // cmbMethod
            // 
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            "Simple",
            "Exponential",
            "SMMA",
            "LWMA"});
            this.cmbMethod.Location = new System.Drawing.Point(64, 39);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(121, 20);
            this.cmbMethod.TabIndex = 1;
            // 
            // cmbPrice
            // 
            this.cmbPrice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrice.FormattingEnabled = true;
            this.cmbPrice.Items.AddRange(new object[] {
            "Mid",
            "End",
            "High",
            "Low"});
            this.cmbPrice.Location = new System.Drawing.Point(64, 69);
            this.cmbPrice.Name = "cmbPrice";
            this.cmbPrice.Size = new System.Drawing.Size(121, 20);
            this.cmbPrice.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Color";
            // 
            // lblColor
            // 
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(64, 98);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(123, 20);
            this.lblColor.TabIndex = 9;
            this.lblColor.Click += new System.EventHandler(this.label5_Click);
            // 
            // MADlg
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(437, 131);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbPrice);
            this.Controls.Add(this.cmbMethod);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numPeriod);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MADlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Moving Average";
            this.Load += new System.EventHandler(this.MADlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown numPeriod;
        public System.Windows.Forms.ComboBox cmbMethod;
        public System.Windows.Forms.ComboBox cmbPrice;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lblColor;
    }
}