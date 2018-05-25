/*///////////////////////////////////
Copyright BAE SYSTEMS 2017
//////////////////////////////////*/

using BaeSystems.TestStations.Controls;

namespace BaeSystems.TestStations
{
    partial class TestPointManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestPointManager));
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.openConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggerSelect = new System.Windows.Forms.ToolStripComboBox();
            this.openLoggingDir = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuBar = new System.Windows.Forms.MenuStrip();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.engineeringMode = new System.Windows.Forms.ToolStripMenuItem();
            this.testMode = new System.Windows.Forms.ToolStripMenuItem();
            this.noDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.config_openTestStationXML = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.uutSerialNumber = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.operatorId = new System.Windows.Forms.TextBox();
            this.testPointInfo = new System.Windows.Forms.GroupBox();
            this.testPicker = new BaeSystems.TestStations.Controls.TestPointPicker();
            this.runTestPoint = new System.Windows.Forms.Button();
            this.testPointSelect = new System.Windows.Forms.ComboBox();
            this.testProgressBar = new ProgressTextBar.ProgressTextBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.testPointControls = new System.Windows.Forms.TabPage();
            this.statusTab = new System.Windows.Forms.TabPage();
            this.logWindow = new BaeSystems.TestStations.Controls.OutputWindow();
            this.mainMenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.testPointInfo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.statusTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConfig,
            this.saveAsConfig,
            this.toolStripSeparator4,
            this.openConfig,
            this.toolStripSeparator5,
            this.exitApplication});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.configToolStripMenuItem.Text = "File";
            // 
            // saveConfig
            // 
            this.saveConfig.Name = "saveConfig";
            this.saveConfig.Size = new System.Drawing.Size(129, 24);
            this.saveConfig.Text = "Save";
            this.saveConfig.Click += new System.EventHandler(this.saveConfig_Click);
            // 
            // saveAsConfig
            // 
            this.saveAsConfig.Name = "saveAsConfig";
            this.saveAsConfig.Size = new System.Drawing.Size(129, 24);
            this.saveAsConfig.Text = "Save As";
            this.saveAsConfig.Click += new System.EventHandler(this.saveAsConfig_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(126, 6);
            // 
            // openConfig
            // 
            this.openConfig.Name = "openConfig";
            this.openConfig.Size = new System.Drawing.Size(129, 24);
            this.openConfig.Text = "Open";
            this.openConfig.Click += new System.EventHandler(this.openConfig_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(126, 6);
            // 
            // exitApplication
            // 
            this.exitApplication.Name = "exitApplication";
            this.exitApplication.Size = new System.Drawing.Size(129, 24);
            this.exitApplication.Text = "Exit";
            this.exitApplication.Click += new System.EventHandler(this.exitApplication_Click);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loggerSelect,
            this.openLoggingDir});
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            this.loggingToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.loggingToolStripMenuItem.Text = "Logging";
            // 
            // loggerSelect
            // 
            this.loggerSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loggerSelect.Name = "loggerSelect";
            this.loggerSelect.Size = new System.Drawing.Size(121, 23);
            // 
            // openLoggingDir
            // 
            this.openLoggingDir.Name = "openLoggingDir";
            this.openLoggingDir.Size = new System.Drawing.Size(181, 24);
            this.openLoggingDir.Text = "Open Log Dir";
            this.openLoggingDir.Click += new System.EventHandler(this.openLoggingDir_Click);
            // 
            // mainMenuBar
            // 
            this.mainMenuBar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.mainMenuBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.configurationToolStripMenuItem});
            this.mainMenuBar.Location = new System.Drawing.Point(0, 0);
            this.mainMenuBar.Name = "mainMenuBar";
            this.mainMenuBar.Size = new System.Drawing.Size(944, 28);
            this.mainMenuBar.TabIndex = 0;
            this.mainMenuBar.Text = "menuStrip1";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.engineeringMode,
            this.testMode,
            this.noDatabase,
            this.config_openTestStationXML});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // engineeringMode
            // 
            this.engineeringMode.CheckOnClick = true;
            this.engineeringMode.Name = "engineeringMode";
            this.engineeringMode.Size = new System.Drawing.Size(228, 24);
            this.engineeringMode.Text = "Engineering Mode";
            // 
            // testMode
            // 
            this.testMode.CheckOnClick = true;
            this.testMode.Name = "testMode";
            this.testMode.Size = new System.Drawing.Size(228, 24);
            this.testMode.Text = "Test Mode";
            // 
            // noDatabase
            // 
            this.noDatabase.CheckOnClick = true;
            this.noDatabase.Name = "noDatabase";
            this.noDatabase.Size = new System.Drawing.Size(228, 24);
            this.noDatabase.Text = "No Database";
            // 
            // config_openTestStationXML
            // 
            this.config_openTestStationXML.Name = "config_openTestStationXML";
            this.config_openTestStationXML.Size = new System.Drawing.Size(228, 24);
            this.config_openTestStationXML.Text = "Open Test Station XML";
            this.config_openTestStationXML.Click += new System.EventHandler(this.config_openTestStationXML_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.testPointInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.testProgressBar);
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(944, 474);
            this.splitContainer1.SplitterDistance = 218;
            this.splitContainer1.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.uutSerialNumber);
            this.groupBox3.Location = new System.Drawing.Point(3, 73);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 64);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Serial Number";
            // 
            // uutSerialNumber
            // 
            this.uutSerialNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uutSerialNumber.Enabled = false;
            this.uutSerialNumber.Location = new System.Drawing.Point(6, 27);
            this.uutSerialNumber.Name = "uutSerialNumber";
            this.uutSerialNumber.Size = new System.Drawing.Size(200, 24);
            this.uutSerialNumber.TabIndex = 0;
            this.uutSerialNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uutSerialNumber_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.operatorId);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 64);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operator ID";
            // 
            // operatorId
            // 
            this.operatorId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operatorId.Location = new System.Drawing.Point(6, 27);
            this.operatorId.Name = "operatorId";
            this.operatorId.Size = new System.Drawing.Size(200, 24);
            this.operatorId.TabIndex = 0;
            this.operatorId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.operatorId_KeyPress);
            // 
            // testPointInfo
            // 
            this.testPointInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testPointInfo.Controls.Add(this.testPicker);
            this.testPointInfo.Controls.Add(this.runTestPoint);
            this.testPointInfo.Controls.Add(this.testPointSelect);
            this.testPointInfo.Location = new System.Drawing.Point(3, 143);
            this.testPointInfo.Name = "testPointInfo";
            this.testPointInfo.Size = new System.Drawing.Size(212, 319);
            this.testPointInfo.TabIndex = 3;
            this.testPointInfo.TabStop = false;
            this.testPointInfo.Text = "Test Point Select";
            // 
            // testPicker
            // 
            this.testPicker.AllowSelection = true;
            this.testPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.testPicker.AutoHideIfEmpty = true;
            this.testPicker.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testPicker.Location = new System.Drawing.Point(5, 102);
            this.testPicker.Margin = new System.Windows.Forms.Padding(2);
            this.testPicker.Name = "testPicker";
            this.testPicker.Size = new System.Drawing.Size(201, 212);
            this.testPicker.TabIndex = 3;
            // 
            // runTestPoint
            // 
            this.runTestPoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runTestPoint.Enabled = false;
            this.runTestPoint.Location = new System.Drawing.Point(6, 55);
            this.runTestPoint.Name = "runTestPoint";
            this.runTestPoint.Size = new System.Drawing.Size(200, 42);
            this.runTestPoint.TabIndex = 2;
            this.runTestPoint.Text = "Run Test Point";
            this.runTestPoint.UseVisualStyleBackColor = true;
            this.runTestPoint.Click += new System.EventHandler(this.runTestPoint_Click);
            // 
            // testPointSelect
            // 
            this.testPointSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testPointSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.testPointSelect.FormattingEnabled = true;
            this.testPointSelect.Location = new System.Drawing.Point(6, 23);
            this.testPointSelect.Name = "testPointSelect";
            this.testPointSelect.Size = new System.Drawing.Size(200, 26);
            this.testPointSelect.TabIndex = 0;
            this.testPointSelect.SelectedIndexChanged += new System.EventHandler(this.testPointSelect_SelectedIndexChanged);
            // 
            // testProgressBar
            // 
            this.testProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testProgressBar.BarFont = new System.Drawing.Font("Times New Roman", 10F);
            this.testProgressBar.BarText = "";
            this.testProgressBar.BarType = ProgressTextBar.BarDisplayType.Text;
            this.testProgressBar.Location = new System.Drawing.Point(3, 439);
            this.testProgressBar.Name = "testProgressBar";
            this.testProgressBar.Size = new System.Drawing.Size(716, 23);
            this.testProgressBar.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.testPointControls);
            this.tabControl1.Controls.Add(this.statusTab);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(716, 430);
            this.tabControl1.TabIndex = 1;
            // 
            // testPointControls
            // 
            this.testPointControls.Location = new System.Drawing.Point(4, 27);
            this.testPointControls.Name = "testPointControls";
            this.testPointControls.Padding = new System.Windows.Forms.Padding(3);
            this.testPointControls.Size = new System.Drawing.Size(708, 399);
            this.testPointControls.TabIndex = 0;
            this.testPointControls.Text = "Test Point";
            this.testPointControls.UseVisualStyleBackColor = true;
            // 
            // statusTab
            // 
            this.statusTab.Controls.Add(this.logWindow);
            this.statusTab.Location = new System.Drawing.Point(4, 34);
            this.statusTab.Name = "statusTab";
            this.statusTab.Padding = new System.Windows.Forms.Padding(3);
            this.statusTab.Size = new System.Drawing.Size(708, 392);
            this.statusTab.TabIndex = 1;
            this.statusTab.Text = "Log";
            this.statusTab.UseVisualStyleBackColor = true;
            // 
            // logWindow
            // 
            this.logWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logWindow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.logWindow.Location = new System.Drawing.Point(6, 6);
            this.logWindow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logWindow.Name = "logWindow";
            this.logWindow.Size = new System.Drawing.Size(694, 379);
            this.logWindow.TabIndex = 0;
            // 
            // TestPointManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 502);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenuBar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuBar;
            this.MinimumSize = new System.Drawing.Size(960, 540);
            this.Name = "TestPointManager";
            this.Text = "Test Management Software";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestPointManager_FormClosing);
            this.Load += new System.EventHandler(this.TestPointManager_Load);
            this.mainMenuBar.ResumeLayout(false);
            this.mainMenuBar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.testPointInfo.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.statusTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox operatorId;
        private System.Windows.Forms.GroupBox testPointInfo;
        private System.Windows.Forms.ComboBox testPointSelect;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button runTestPoint;
        private ProgressTextBar.ProgressTextBar testProgressBar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage testPointControls;
        private System.Windows.Forms.TabPage statusTab;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.MenuStrip mainMenuBar;
        private System.Windows.Forms.ToolStripComboBox loggerSelect;
        private System.Windows.Forms.ToolStripMenuItem saveConfig;
        private System.Windows.Forms.ToolStripMenuItem saveAsConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem openConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem exitApplication;
        private System.Windows.Forms.ToolStripMenuItem openLoggingDir;
        private OutputWindow logWindow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox uutSerialNumber;
        private TestPointPicker testPicker;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem engineeringMode;
        private System.Windows.Forms.ToolStripMenuItem testMode;
        private System.Windows.Forms.ToolStripMenuItem noDatabase;
        private System.Windows.Forms.ToolStripMenuItem config_openTestStationXML;
    }
}

