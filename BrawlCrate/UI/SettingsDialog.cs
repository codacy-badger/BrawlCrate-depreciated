using System;
using System.Windows.Forms;
using System.IO;
using BrawlLib.SSBB;
using System.Collections.Generic;
using Microsoft.Win32;

namespace BrawlCrate
{
    class SettingsDialog : Form
    {
        private bool _updating;

        static SettingsDialog()
        {
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
                if (info._forEditing)
                    foreach (string s in info._extensions)
                    {
                        if (s != "dat")
                        {
                            _assocList.Add(FileAssociation.Get("." + s));
                            _typeList.Add(FileType.Get("SSBB." + s.ToUpper()));
                        }
                    }
        }

        private static List<FileAssociation> _assocList = new List<FileAssociation>();
        private static List<FileType> _typeList = new List<FileType>();
        private CheckBox chkShowHex;
        private CheckBox chkDocUpdates;
        private CheckBox chkCanary;
        private TabControl tabControl1;
        private TabPage tabGeneral;
        private TabPage tabUpdater;
        private GroupBox updaterBehaviorGroupbox;
        private TabPage tabFileAssociations;
        private Button btnApply;
        private GroupBox groupBox1;
        private CheckBox checkBox1;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private GroupBox groupBox3;
        private Button btnCanaryBranch;
        private RadioButton rdoAutoUpdate;
        private RadioButton rdoCheckManual;
        private RadioButton rdoCheckStartup;
        private GroupBox groupBox4;
        private Label lblAdminApproval;
        private CheckBox chkShowPropDesc;

        public SettingsDialog()
        {
            InitializeComponent();

            tabUpdater.Enabled = tabUpdater.Visible =
                MainForm.Instance.checkForUpdatesToolStripMenuItem.Enabled;

            if (!MainForm.Instance.checkForUpdatesToolStripMenuItem.Enabled)
                tabControl1.TabPages.Remove(tabUpdater);

            listView1.Items.Clear();
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
                if (info._forEditing)
                    foreach (string s in info._extensions)
                        if(s != "dat")
                            listView1.Items.Add(new ListViewItem() { Text = String.Format("{0} (*.{1})", info._name, s) });
        }

        private void Apply()
        {
            try
            {
                bool check;
                int index = 0;
                foreach (ListViewItem i in listView1.Items)
                {
                    if ((check = i.Checked) != (bool)i.Tag)
                    {
                        if (check)
                        {
                            _assocList[index].FileType = _typeList[index];
                            _typeList[index].SetCommand("open", String.Format("\"{0}\" \"%1\"", Program.FullPath));
                        }
                        else
                        {
                            _typeList[index].Delete();
                            _assocList[index].Delete();
                        }
                        i.Tag = check;
                    }
                    index++;
                }
                listView1.Sort();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(null, "Unable to access the registry to set file associations.\nRun the program as administrator and try again.", "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnApply.Enabled = false;
            }
        }
        private void SettingsDialog_Shown(object sender, EventArgs e)
        {
            int index = 0;
            string cmd;
            foreach (ListViewItem i in listView1.Items)
            {
                try
                {
                    if ((_typeList[index] == _assocList[index].FileType) &&
                        (!String.IsNullOrEmpty(cmd = _typeList[index].GetCommand("open"))) &&
                        (cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0))
                        i.Tag = i.Checked = true;
                    else
                        i.Tag = i.Checked = false;
                }
                catch
                {

                }
                index++;
            }

            _updating = true;
            chkDocUpdates.Checked = MainForm.Instance.GetDocumentationUpdates;
            updaterBehaviorGroupbox.Enabled = !MainForm.Instance.Canary;
            if (MainForm.Instance.UpdateAutomatically)
                rdoAutoUpdate.Checked = true;
            else if (MainForm.Instance.CheckUpdatesOnStartup)
                rdoCheckStartup.Checked = true;
            else
                rdoCheckManual.Checked = true;
            chkCanary.Checked = MainForm.Instance.Canary;
            chkShowPropDesc.Checked = MainForm.Instance.DisplayPropertyDescriptionsWhenAvailable;
            chkShowHex.Checked = MainForm.Instance.ShowHex;

            _updating = false;
            checkAdminAccess();
            btnApply.Enabled = false;      
        }

        // Unimplemented
        private bool checkAdminAccess()
        {
            try
            {
                //throw (new Exception());
                lblAdminApproval.Visible = false;
                btnApply.Visible = true;
                groupBox1.Enabled = true;
                return true;
            }
            catch
            {
                lblAdminApproval.Visible = true;
                btnApply.Visible = false;
                groupBox1.Enabled = false;
                return false;
            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            btnApply.Enabled = true;
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            Apply();
        }
        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (btnApply.Enabled)
                Apply();

            this.DialogResult = DialogResult.OK;
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
        
        #region Designer
    
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("File Types", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Resource Types", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("ARChive Pack (*.pac)");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Compressed ARChive Pack (*.pcs)");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("ARChive (*.arc)");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Compressed ARChive (*.szs)");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Resource Pack (*.brres)");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Model Pack (*.brmdl)");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Texture Pack (*.brtex)");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("MSBin Message List (*.msbin)");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Sound Archive (*.brsar)");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Sound Stream (*.brstm)");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("Texture (*.tex0)");
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem("Palette (*.plt0)");
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem("Model (*.mdl0)");
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem("Model Animation (*.chr0)");
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem("Texture Animation (*.srt0)");
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem("Vertex Morph (*.shp0)");
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem("Texture Pattern (*.pat0)");
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem("Bone Visibility (*.vis0)");
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem("Scene Settings (*.scn0)");
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem("Color Sequence (*.clr0)");
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem("Effect List (*.efls)");
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem("Effect Parameters (*.breff)");
            System.Windows.Forms.ListViewItem listViewItem23 = new System.Windows.Forms.ListViewItem("Effect Textures (*.breft)");
            System.Windows.Forms.ListViewItem listViewItem24 = new System.Windows.Forms.ListViewItem("Sound Stream (*.brwsd)");
            System.Windows.Forms.ListViewItem listViewItem25 = new System.Windows.Forms.ListViewItem("Sound Bank (*.brbnk)");
            System.Windows.Forms.ListViewItem listViewItem26 = new System.Windows.Forms.ListViewItem("Sound Sequence (*.brseq)");
            System.Windows.Forms.ListViewItem listViewItem27 = new System.Windows.Forms.ListViewItem("Static Module (*.dol)");
            System.Windows.Forms.ListViewItem listViewItem28 = new System.Windows.Forms.ListViewItem("Relocatable Module (*.rel)");
            System.Windows.Forms.ListViewItem listViewItem29 = new System.Windows.Forms.ListViewItem("Texture Archive (*.tpl)");
            this.chkShowPropDesc = new System.Windows.Forms.CheckBox();
            this.chkShowHex = new System.Windows.Forms.CheckBox();
            this.chkDocUpdates = new System.Windows.Forms.CheckBox();
            this.chkCanary = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabFileAssociations = new System.Windows.Forms.TabPage();
            this.lblAdminApproval = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabUpdater = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCanaryBranch = new System.Windows.Forms.Button();
            this.updaterBehaviorGroupbox = new System.Windows.Forms.GroupBox();
            this.rdoAutoUpdate = new System.Windows.Forms.RadioButton();
            this.rdoCheckManual = new System.Windows.Forms.RadioButton();
            this.rdoCheckStartup = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabFileAssociations.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabUpdater.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.updaterBehaviorGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowPropDesc
            // 
            this.chkShowPropDesc.AutoSize = true;
            this.chkShowPropDesc.Location = new System.Drawing.Point(6, 22);
            this.chkShowPropDesc.Name = "chkShowPropDesc";
            this.chkShowPropDesc.Size = new System.Drawing.Size(242, 17);
            this.chkShowPropDesc.TabIndex = 7;
            this.chkShowPropDesc.Text = "Show property description box when available";
            this.chkShowPropDesc.UseVisualStyleBackColor = true;
            this.chkShowPropDesc.CheckedChanged += new System.EventHandler(this.chkShowPropDesc_CheckedChanged);
            // 
            // chkShowHex
            // 
            this.chkShowHex.AutoSize = true;
            this.chkShowHex.Location = new System.Drawing.Point(6, 45);
            this.chkShowHex.Name = "chkShowHex";
            this.chkShowHex.Size = new System.Drawing.Size(233, 17);
            this.chkShowHex.TabIndex = 9;
            this.chkShowHex.Text = "Show hexadecimal for files without previews";
            this.chkShowHex.UseVisualStyleBackColor = true;
            this.chkShowHex.CheckedChanged += new System.EventHandler(this.chkShowHex_CheckedChanged);
            // 
            // chkDocUpdates
            // 
            this.chkDocUpdates.AutoSize = true;
            this.chkDocUpdates.Location = new System.Drawing.Point(10, 91);
            this.chkDocUpdates.Name = "chkDocUpdates";
            this.chkDocUpdates.Size = new System.Drawing.Size(180, 17);
            this.chkDocUpdates.TabIndex = 11;
            this.chkDocUpdates.Text = "Receive documentation updates";
            this.chkDocUpdates.UseVisualStyleBackColor = true;
            this.chkDocUpdates.CheckedChanged += new System.EventHandler(this.chkDocUpdates_CheckedChanged);
            // 
            // chkCanary
            // 
            this.chkCanary.AutoSize = true;
            this.chkCanary.Location = new System.Drawing.Point(10, 22);
            this.chkCanary.Name = "chkCanary";
            this.chkCanary.Size = new System.Drawing.Size(231, 17);
            this.chkCanary.TabIndex = 13;
            this.chkCanary.Text = "Opt into BrawlCrate Canary (BETA) updates";
            this.chkCanary.UseVisualStyleBackColor = true;
            this.chkCanary.CheckedChanged += new System.EventHandler(this.chkCanary_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabFileAssociations);
            this.tabControl1.Controls.Add(this.tabUpdater);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(289, 345);
            this.tabControl1.TabIndex = 48;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabGeneral.Controls.Add(this.groupBox4);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(281, 319);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chkShowPropDesc);
            this.groupBox4.Controls.Add(this.chkShowHex);
            this.groupBox4.Location = new System.Drawing.Point(8, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(265, 75);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Main Form";
            // 
            // tabFileAssociations
            // 
            this.tabFileAssociations.Controls.Add(this.lblAdminApproval);
            this.tabFileAssociations.Controls.Add(this.btnApply);
            this.tabFileAssociations.Controls.Add(this.groupBox1);
            this.tabFileAssociations.Location = new System.Drawing.Point(4, 22);
            this.tabFileAssociations.Name = "tabFileAssociations";
            this.tabFileAssociations.Size = new System.Drawing.Size(281, 319);
            this.tabFileAssociations.TabIndex = 2;
            this.tabFileAssociations.Text = "File Associations";
            // 
            // lblAdminApproval
            // 
            this.lblAdminApproval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdminApproval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminApproval.ForeColor = System.Drawing.Color.Red;
            this.lblAdminApproval.Location = new System.Drawing.Point(3, 293);
            this.lblAdminApproval.Name = "lblAdminApproval";
            this.lblAdminApproval.Size = new System.Drawing.Size(275, 18);
            this.lblAdminApproval.TabIndex = 5;
            this.lblAdminApproval.Text = "Administrator access required to make changes";
            this.lblAdminApproval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(203, 291);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Location = new System.Drawing.Point(8, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 282);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.Location = new System.Drawing.Point(158, 256);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox1.Size = new System.Drawing.Size(104, 20);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Check All";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.AutoArrange = false;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            listViewGroup1.Header = "File Types";
            listViewGroup1.Name = "grpFileTypes";
            listViewGroup2.Header = "Resource Types";
            listViewGroup2.Name = "grpResTypes";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.Tag = "";
            listViewItem2.StateImageIndex = 0;
            listViewItem2.Tag = "";
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem5.Tag = "";
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem8.Tag = "";
            listViewItem9.StateImageIndex = 0;
            listViewItem10.StateImageIndex = 0;
            listViewItem11.StateImageIndex = 0;
            listViewItem12.StateImageIndex = 0;
            listViewItem13.StateImageIndex = 0;
            listViewItem14.StateImageIndex = 0;
            listViewItem15.StateImageIndex = 0;
            listViewItem16.StateImageIndex = 0;
            listViewItem17.StateImageIndex = 0;
            listViewItem18.StateImageIndex = 0;
            listViewItem19.StateImageIndex = 0;
            listViewItem20.StateImageIndex = 0;
            listViewItem21.StateImageIndex = 0;
            listViewItem22.StateImageIndex = 0;
            listViewItem23.StateImageIndex = 0;
            listViewItem24.StateImageIndex = 0;
            listViewItem25.StateImageIndex = 0;
            listViewItem26.StateImageIndex = 0;
            listViewItem27.StateImageIndex = 0;
            listViewItem28.StateImageIndex = 0;
            listViewItem29.StateImageIndex = 0;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19,
            listViewItem20,
            listViewItem21,
            listViewItem22,
            listViewItem23,
            listViewItem24,
            listViewItem25,
            listViewItem26,
            listViewItem27,
            listViewItem28,
            listViewItem29});
            this.listView1.Location = new System.Drawing.Point(3, 11);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(256, 239);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 300;
            // 
            // tabUpdater
            // 
            this.tabUpdater.Controls.Add(this.groupBox3);
            this.tabUpdater.Controls.Add(this.updaterBehaviorGroupbox);
            this.tabUpdater.Location = new System.Drawing.Point(4, 22);
            this.tabUpdater.Name = "tabUpdater";
            this.tabUpdater.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdater.Size = new System.Drawing.Size(281, 319);
            this.tabUpdater.TabIndex = 1;
            this.tabUpdater.Text = "Updater";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnCanaryBranch);
            this.groupBox3.Controls.Add(this.chkCanary);
            this.groupBox3.Location = new System.Drawing.Point(8, 132);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(265, 71);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BrawlCrate Canary";
            // 
            // btnCanaryBranch
            // 
            this.btnCanaryBranch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCanaryBranch.Location = new System.Drawing.Point(60, 42);
            this.btnCanaryBranch.Name = "btnCanaryBranch";
            this.btnCanaryBranch.Size = new System.Drawing.Size(138, 23);
            this.btnCanaryBranch.TabIndex = 14;
            this.btnCanaryBranch.Text = "Change Canary Branch";
            this.btnCanaryBranch.UseVisualStyleBackColor = true;
            this.btnCanaryBranch.Click += new System.EventHandler(this.btnCanaryBranch_Click);
            // 
            // updaterBehaviorGroupbox
            // 
            this.updaterBehaviorGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updaterBehaviorGroupbox.Controls.Add(this.rdoAutoUpdate);
            this.updaterBehaviorGroupbox.Controls.Add(this.rdoCheckManual);
            this.updaterBehaviorGroupbox.Controls.Add(this.rdoCheckStartup);
            this.updaterBehaviorGroupbox.Controls.Add(this.chkDocUpdates);
            this.updaterBehaviorGroupbox.Location = new System.Drawing.Point(8, 6);
            this.updaterBehaviorGroupbox.Name = "updaterBehaviorGroupbox";
            this.updaterBehaviorGroupbox.Size = new System.Drawing.Size(265, 120);
            this.updaterBehaviorGroupbox.TabIndex = 14;
            this.updaterBehaviorGroupbox.TabStop = false;
            this.updaterBehaviorGroupbox.Text = "Updater Behavior";
            this.updaterBehaviorGroupbox.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // rdoAutoUpdate
            // 
            this.rdoAutoUpdate.AutoSize = true;
            this.rdoAutoUpdate.Location = new System.Drawing.Point(10, 22);
            this.rdoAutoUpdate.Name = "rdoAutoUpdate";
            this.rdoAutoUpdate.Size = new System.Drawing.Size(72, 17);
            this.rdoAutoUpdate.TabIndex = 2;
            this.rdoAutoUpdate.TabStop = true;
            this.rdoAutoUpdate.Text = "Automatic";
            this.rdoAutoUpdate.UseVisualStyleBackColor = true;
            this.rdoAutoUpdate.CheckedChanged += new System.EventHandler(this.updaterBehavior_CheckedChanged);
            // 
            // rdoCheckManual
            // 
            this.rdoCheckManual.AutoSize = true;
            this.rdoCheckManual.Location = new System.Drawing.Point(10, 68);
            this.rdoCheckManual.Name = "rdoCheckManual";
            this.rdoCheckManual.Size = new System.Drawing.Size(60, 17);
            this.rdoCheckManual.TabIndex = 1;
            this.rdoCheckManual.TabStop = true;
            this.rdoCheckManual.Text = "Manual";
            this.rdoCheckManual.UseVisualStyleBackColor = true;
            // 
            // rdoCheckStartup
            // 
            this.rdoCheckStartup.AutoSize = true;
            this.rdoCheckStartup.Location = new System.Drawing.Point(10, 45);
            this.rdoCheckStartup.Name = "rdoCheckStartup";
            this.rdoCheckStartup.Size = new System.Drawing.Size(220, 17);
            this.rdoCheckStartup.TabIndex = 0;
            this.rdoCheckStartup.TabStop = true;
            this.rdoCheckStartup.Text = "Manual, but check for updates on startup";
            this.rdoCheckStartup.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.ClientSize = new System.Drawing.Size(289, 345);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.Shown += new System.EventHandler(this.SettingsDialog_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabFileAssociations.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabUpdater.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.updaterBehaviorGroupbox.ResumeLayout(false);
            this.updaterBehaviorGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                bool check = checkBox1.Checked;
                foreach (ListViewItem i in listView1.Items)
                    i.Checked = check;
            }
        }

        private void chkShowPropDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.DisplayPropertyDescriptionsWhenAvailable = chkShowPropDesc.Checked;
        }
        
        private void SettingsDialog_Load(object sender, EventArgs e)
        {

        }

        private void chkShowHex_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.ShowHex = chkShowHex.Checked;
        }

        private void chkDocUpdates_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.GetDocumentationUpdates = chkDocUpdates.Checked;
        }
        
        private void chkCanary_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            _updating = true;
            if (!MainForm.Instance.Canary)
            {
                DialogResult dc = MessageBox.Show(this, "Are you sure you'd like to receive BrawlCrate canary updates? " +
                    "These updates will happen more often and include features as they are developed, but will come at the cost of stability. " +
                    "If you do take this track, it is highly recommended to join our discord server: https://discord.gg/s7c8763 \n\n" +
                    "If you select yes, the update will begin immediately, so make sure your work is saved.", "BrawlCrate Canary Updater", MessageBoxButtons.YesNo);
                if(dc == DialogResult.Yes)
                {
                    Program.ForceDownloadCanary();
                }
            }
            else
            {
                DialogResult dc = MessageBox.Show(this, "Are you sure you'd like to return to the stable build? " +
                    "Please note that there may be issues saving settings between the old version and the next update. " +
                    "If you a bug caused you to move off this build, please report it on our discord server: https://discord.gg/s7c8763 \n\n" +
                    "If you select yes, the downgrade will begin immediately, so make sure your work is saved.", "BrawlCrate Canary Updater", MessageBoxButtons.YesNo);
                if (dc == DialogResult.Yes)
                {
                    Program.ForceDownloadStable();
                }
            }
            chkCanary.Checked = MainForm.Instance.Canary;
            _updating = false;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void updaterBehavior_CheckedChanged(object sender, EventArgs e)
        {
            if(!_updating)
            {
                MainForm.Instance.UpdateAutomatically = rdoAutoUpdate.Checked;
                MainForm.Instance.CheckUpdatesOnStartup = (rdoAutoUpdate.Checked || rdoCheckStartup.Checked);
            }
        }

        private void btnCanaryBranch_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(this, "Warning: Changing Branches can be unstable unless you know what you're doing. You should generally stay on the brawlcrate-master branch unless directed otherwise for testing purposes", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string cBranch = MainForm.currentBranch;
                RenameDialog d = new RenameDialog();
                if(d.ShowDialog(this, "Enter new branch to track", cBranch) == DialogResult.OK)
                {
                    if(d.NewName != cBranch)
                        MainForm.currentBranch = d.NewName;
                }
            }
        }
    }
}