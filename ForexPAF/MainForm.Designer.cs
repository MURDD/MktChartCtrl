namespace ForexPAF
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.colorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.numBox = new ForexPAF.ToolStripNumericUpDown();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.numReversal = new ForexPAF.ToolStripNumericUpDown();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cmbScale = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.cmbMargin = new System.Windows.Forms.ToolStripComboBox();
            this.pfChartCtrl1 = new PFChartCtrl.PFChartCtrl();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 486);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1241, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(133, 52);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(132, 24);
            this.toolStripMenuItem1.Text = "Edit";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(132, 24);
            this.toolStripMenuItem2.Text = "Remove";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1241, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem4,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(164, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mAToolStripMenuItem,
            this.toolStripMenuItem3,
            this.copyToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // mAToolStripMenuItem
            // 
            this.mAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem1,
            this.removeToolStripMenuItem});
            this.mAToolStripMenuItem.Name = "mAToolStripMenuItem";
            this.mAToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.mAToolStripMenuItem.Text = "&MA";
            this.mAToolStripMenuItem.DropDownOpening += new System.EventHandler(this.mAToolStripMenuItem_DropDownOpening);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+A";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.addToolStripMenuItem.Text = "&Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(158, 24);
            this.editToolStripMenuItem1.Text = "&Edit";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.removeToolStripMenuItem.Text = "&Remove";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(160, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLeftToolStripMenuItem,
            this.barStyleToolStripMenuItem,
            this.gridLineToolStripMenuItem,
            this.simpleLineToolStripMenuItem,
            this.toolStripMenuItem5,
            this.colorsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // showLeftToolStripMenuItem
            // 
            this.showLeftToolStripMenuItem.CheckOnClick = true;
            this.showLeftToolStripMenuItem.Name = "showLeftToolStripMenuItem";
            this.showLeftToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.showLeftToolStripMenuItem.Text = "&Show Left";
            this.showLeftToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showLeftToolStripMenuItem_CheckedChanged);
            // 
            // barStyleToolStripMenuItem
            // 
            this.barStyleToolStripMenuItem.CheckOnClick = true;
            this.barStyleToolStripMenuItem.Name = "barStyleToolStripMenuItem";
            this.barStyleToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.barStyleToolStripMenuItem.Text = "&Bar Style";
            this.barStyleToolStripMenuItem.CheckedChanged += new System.EventHandler(this.barStyleToolStripMenuItem_CheckedChanged);
            // 
            // gridLineToolStripMenuItem
            // 
            this.gridLineToolStripMenuItem.CheckOnClick = true;
            this.gridLineToolStripMenuItem.Name = "gridLineToolStripMenuItem";
            this.gridLineToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.gridLineToolStripMenuItem.Text = "&Grid Line";
            this.gridLineToolStripMenuItem.CheckedChanged += new System.EventHandler(this.gridLineToolStripMenuItem_CheckedChanged);
            // 
            // simpleLineToolStripMenuItem
            // 
            this.simpleLineToolStripMenuItem.CheckOnClick = true;
            this.simpleLineToolStripMenuItem.Name = "simpleLineToolStripMenuItem";
            this.simpleLineToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.simpleLineToolStripMenuItem.Text = "S&imple Line";
            this.simpleLineToolStripMenuItem.CheckedChanged += new System.EventHandler(this.simpleLineToolStripMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(166, 6);
            // 
            // colorsToolStripMenuItem
            // 
            this.colorsToolStripMenuItem.Name = "colorsToolStripMenuItem";
            this.colorsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+L";
            this.colorsToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.colorsToolStripMenuItem.Text = "&Colors";
            this.colorsToolStripMenuItem.Click += new System.EventHandler(this.colorsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.versionToolStripMenuItem.Text = "&About";
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.versionToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.numBox,
            this.toolStripLabel2,
            this.numReversal,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.cmbScale,
            this.toolStripLabel4,
            this.cmbMargin});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1241, 30);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 27);
            this.toolStripLabel1.Text = "Box";
            // 
            // numBox
            // 
            this.numBox.Name = "numBox";
            this.numBox.NumericUpDownText = "5";
            this.numBox.Size = new System.Drawing.Size(49, 27);
            this.numBox.Text = "5";
            this.numBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(33, 27);
            this.toolStripLabel2.Text = "Rev";
            // 
            // numReversal
            // 
            this.numReversal.Name = "numReversal";
            this.numReversal.NumericUpDownText = "3";
            this.numReversal.Size = new System.Drawing.Size(49, 27);
            this.numReversal.Text = "3";
            this.numReversal.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(78, 27);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.refreshToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(44, 27);
            this.toolStripLabel3.Text = "Scale";
            // 
            // cmbScale
            // 
            this.cmbScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScale.DropDownWidth = 42;
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(99, 30);
            this.cmbScale.SelectedIndexChanged += new System.EventHandler(this.cmbScale_SelectedIndexChanged);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(56, 27);
            this.toolStripLabel4.Text = "Margin";
            this.toolStripLabel4.Visible = false;
            // 
            // cmbMargin
            // 
            this.cmbMargin.DropDownWidth = 42;
            this.cmbMargin.Name = "cmbMargin";
            this.cmbMargin.Size = new System.Drawing.Size(99, 30);
            this.cmbMargin.Visible = false;
            this.cmbMargin.SelectedIndexChanged += new System.EventHandler(this.cmbMargin_SelectedIndexChanged);
            // 
            // pfChartCtrl1
            // 
            this.pfChartCtrl1.BarStyle = true;
            this.pfChartCtrl1.BoxPipSize = 0;
            this.pfChartCtrl1.DataSource = null;
            this.pfChartCtrl1.Location = new System.Drawing.Point(201, 121);
            this.pfChartCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.pfChartCtrl1.MouseSelectMode = false;
            this.pfChartCtrl1.Name = "pfChartCtrl1";
            this.pfChartCtrl1.PendingOrders = null;
            this.pfChartCtrl1.PFDataSource = null;
            this.pfChartCtrl1.PipSize = 0D;
            this.pfChartCtrl1.Positions = null;
            this.pfChartCtrl1.PriceFormat = "-,000.0000";
            this.pfChartCtrl1.PriceTickSize = 0.001D;
            this.pfChartCtrl1.ReversalBox = 0;
            this.pfChartCtrl1.ShowGridLine = false;
            this.pfChartCtrl1.ShowLeftPrice = false;
            this.pfChartCtrl1.ShowLine = false;
            this.pfChartCtrl1.Size = new System.Drawing.Size(649, 249);
            this.pfChartCtrl1.TabIndex = 80;
            this.pfChartCtrl1.Text = "pfChartCtrl1";
            this.pfChartCtrl1.ViewPriceTickSize = 0.01D;
            this.pfChartCtrl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pfChartCtrl1_MouseMove);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 511);
            this.Controls.Add(this.pfChartCtrl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "ForexPAF";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        public PFChartCtrl.PFChartCtrl pfChartCtrl1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem showLeftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem barStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simpleLineToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private ToolStripNumericUpDown numBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private ToolStripNumericUpDown numReversal;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox cmbScale;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox cmbMargin;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    }
}

