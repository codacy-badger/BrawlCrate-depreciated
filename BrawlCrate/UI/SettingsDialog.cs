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
                        if (!s.Equals("dat", StringComparison.OrdinalIgnoreCase) && !s.Equals("bin", StringComparison.OrdinalIgnoreCase))
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
        private GroupBox grpBoxCanary;
        private Button btnCanaryBranch;
        private RadioButton rdoAutoUpdate;
        private RadioButton rdoCheckManual;
        private RadioButton rdoCheckStartup;
        private GroupBox grpBoxMainFormGeneral;
        private Label lblAdminApproval;
        private TabPage tabCompression;
        private GroupBox groupBoxFighterCompression;
        private CheckBox chkBoxFighterPacDecompress;
        private CheckBox chkBoxFighterPcsCompress;
        private GroupBox groupBoxStageCompression;
        private CheckBox chkBoxStageCompress;
        private GroupBox groupBoxModuleCompression;
        private CheckBox chkBoxModuleCompress;
        private GroupBox grpBoxAudioGeneral;
        private CheckBox chkBoxAutoPlayAudio;
        private GroupBox grpBoxMDL0General;
        private CheckBox chkBoxMDL0Compatibility;
        private TabPage tabDiscord;
        private GroupBox grpBoxDiscordRPC;
        private CheckBox chkBoxEnableDiscordRPC;
        private GroupBox grpBoxDiscordRPCType;
        private RadioButton rdoDiscordRPCNameInternal;
        private RadioButton rdoDiscordRPCNameDisabled;
        private TextBox DiscordRPCCustomName;
        private RadioButton rdoDiscordRPCNameCustom;
        private RadioButton rdoDiscordRPCNameExternal;
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
            catch (Exception)
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
            chkBoxAutoPlayAudio.Checked = MainForm.Instance.AutoPlayAudio;
            chkBoxFighterPacDecompress.Checked = MainForm.Instance.AutoDecompressFighterPAC;
            chkBoxFighterPcsCompress.Checked = MainForm.Instance.AutoCompressPCS;
            chkBoxStageCompress.Checked = MainForm.Instance.AutoCompressStages;
            chkBoxModuleCompress.Checked = MainForm.Instance.AutoCompressModules;
            chkBoxAutoPlayAudio.Checked = MainForm.Instance.AutoPlayAudio;
            chkBoxMDL0Compatibility.Checked = MainForm.Instance.CompatibilityMode;

            Discord.DiscordSettings.LoadSettings();
            grpBoxDiscordRPCType.Enabled = chkBoxEnableDiscordRPC.Checked = Discord.DiscordSettings.enabled;
            if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.Disabled)
                rdoDiscordRPCNameDisabled.Checked = true;
            else if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.UserDefined)
                rdoDiscordRPCNameCustom.Checked = true;
            else if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.AutoInternal)
                rdoDiscordRPCNameInternal.Checked = true;
            else if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.AutoExternal)
                rdoDiscordRPCNameExternal.Checked = true;
            DiscordRPCCustomName.Text = Discord.DiscordSettings.userNamedMod;
            DiscordRPCCustomName.Enabled = rdoDiscordRPCNameCustom.Checked;
            DiscordRPCCustomName.ReadOnly = !rdoDiscordRPCNameCustom.Checked;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            this.chkShowPropDesc = new System.Windows.Forms.CheckBox();
            this.chkShowHex = new System.Windows.Forms.CheckBox();
            this.chkDocUpdates = new System.Windows.Forms.CheckBox();
            this.chkCanary = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.grpBoxMDL0General = new System.Windows.Forms.GroupBox();
            this.chkBoxMDL0Compatibility = new System.Windows.Forms.CheckBox();
            this.grpBoxAudioGeneral = new System.Windows.Forms.GroupBox();
            this.chkBoxAutoPlayAudio = new System.Windows.Forms.CheckBox();
            this.grpBoxMainFormGeneral = new System.Windows.Forms.GroupBox();
            this.tabCompression = new System.Windows.Forms.TabPage();
            this.groupBoxModuleCompression = new System.Windows.Forms.GroupBox();
            this.chkBoxModuleCompress = new System.Windows.Forms.CheckBox();
            this.groupBoxStageCompression = new System.Windows.Forms.GroupBox();
            this.chkBoxStageCompress = new System.Windows.Forms.CheckBox();
            this.groupBoxFighterCompression = new System.Windows.Forms.GroupBox();
            this.chkBoxFighterPacDecompress = new System.Windows.Forms.CheckBox();
            this.chkBoxFighterPcsCompress = new System.Windows.Forms.CheckBox();
            this.tabFileAssociations = new System.Windows.Forms.TabPage();
            this.lblAdminApproval = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabDiscord = new System.Windows.Forms.TabPage();
            this.grpBoxDiscordRPC = new System.Windows.Forms.GroupBox();
            this.chkBoxEnableDiscordRPC = new System.Windows.Forms.CheckBox();
            this.grpBoxDiscordRPCType = new System.Windows.Forms.GroupBox();
            this.DiscordRPCCustomName = new System.Windows.Forms.TextBox();
            this.rdoDiscordRPCNameCustom = new System.Windows.Forms.RadioButton();
            this.rdoDiscordRPCNameExternal = new System.Windows.Forms.RadioButton();
            this.rdoDiscordRPCNameInternal = new System.Windows.Forms.RadioButton();
            this.rdoDiscordRPCNameDisabled = new System.Windows.Forms.RadioButton();
            this.tabUpdater = new System.Windows.Forms.TabPage();
            this.grpBoxCanary = new System.Windows.Forms.GroupBox();
            this.btnCanaryBranch = new System.Windows.Forms.Button();
            this.updaterBehaviorGroupbox = new System.Windows.Forms.GroupBox();
            this.rdoAutoUpdate = new System.Windows.Forms.RadioButton();
            this.rdoCheckManual = new System.Windows.Forms.RadioButton();
            this.rdoCheckStartup = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.grpBoxMDL0General.SuspendLayout();
            this.grpBoxAudioGeneral.SuspendLayout();
            this.grpBoxMainFormGeneral.SuspendLayout();
            this.tabCompression.SuspendLayout();
            this.groupBoxModuleCompression.SuspendLayout();
            this.groupBoxStageCompression.SuspendLayout();
            this.groupBoxFighterCompression.SuspendLayout();
            this.tabFileAssociations.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabDiscord.SuspendLayout();
            this.grpBoxDiscordRPC.SuspendLayout();
            this.grpBoxDiscordRPCType.SuspendLayout();
            this.tabUpdater.SuspendLayout();
            this.grpBoxCanary.SuspendLayout();
            this.updaterBehaviorGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowPropDesc
            // 
            this.chkShowPropDesc.AutoSize = true;
            this.chkShowPropDesc.Location = new System.Drawing.Point(10, 22);
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
            this.chkShowHex.Location = new System.Drawing.Point(10, 45);
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
            this.tabControl1.Controls.Add(this.tabCompression);
            this.tabControl1.Controls.Add(this.tabFileAssociations);
            this.tabControl1.Controls.Add(this.tabDiscord);
            this.tabControl1.Controls.Add(this.tabUpdater);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(326, 345);
            this.tabControl1.TabIndex = 48;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabGeneral.Controls.Add(this.grpBoxMDL0General);
            this.tabGeneral.Controls.Add(this.grpBoxAudioGeneral);
            this.tabGeneral.Controls.Add(this.grpBoxMainFormGeneral);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(318, 319);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // grpBoxMDL0General
            // 
            this.grpBoxMDL0General.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxMDL0General.Controls.Add(this.chkBoxMDL0Compatibility);
            this.grpBoxMDL0General.Location = new System.Drawing.Point(8, 146);
            this.grpBoxMDL0General.Name = "grpBoxMDL0General";
            this.grpBoxMDL0General.Size = new System.Drawing.Size(302, 53);
            this.grpBoxMDL0General.TabIndex = 19;
            this.grpBoxMDL0General.TabStop = false;
            this.grpBoxMDL0General.Text = "Models";
            // 
            // chkBoxMDL0Compatibility
            // 
            this.chkBoxMDL0Compatibility.AutoSize = true;
            this.chkBoxMDL0Compatibility.Location = new System.Drawing.Point(10, 22);
            this.chkBoxMDL0Compatibility.Name = "chkBoxMDL0Compatibility";
            this.chkBoxMDL0Compatibility.Size = new System.Drawing.Size(134, 17);
            this.chkBoxMDL0Compatibility.TabIndex = 7;
            this.chkBoxMDL0Compatibility.Text = "Use compatibility mode";
            this.chkBoxMDL0Compatibility.UseVisualStyleBackColor = true;
            this.chkBoxMDL0Compatibility.CheckedChanged += new System.EventHandler(this.chkBoxMDL0Compatibility_CheckedChanged);
            // 
            // grpBoxAudioGeneral
            // 
            this.grpBoxAudioGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAudioGeneral.Controls.Add(this.chkBoxAutoPlayAudio);
            this.grpBoxAudioGeneral.Location = new System.Drawing.Point(8, 87);
            this.grpBoxAudioGeneral.Name = "grpBoxAudioGeneral";
            this.grpBoxAudioGeneral.Size = new System.Drawing.Size(302, 53);
            this.grpBoxAudioGeneral.TabIndex = 18;
            this.grpBoxAudioGeneral.TabStop = false;
            this.grpBoxAudioGeneral.Text = "Audio";
            // 
            // chkBoxAutoPlayAudio
            // 
            this.chkBoxAutoPlayAudio.AutoSize = true;
            this.chkBoxAutoPlayAudio.Location = new System.Drawing.Point(10, 22);
            this.chkBoxAutoPlayAudio.Name = "chkBoxAutoPlayAudio";
            this.chkBoxAutoPlayAudio.Size = new System.Drawing.Size(171, 17);
            this.chkBoxAutoPlayAudio.TabIndex = 7;
            this.chkBoxAutoPlayAudio.Text = "Automatically play audio nodes";
            this.chkBoxAutoPlayAudio.UseVisualStyleBackColor = true;
            this.chkBoxAutoPlayAudio.CheckedChanged += new System.EventHandler(this.chkBoxAutoPlayAudio_CheckedChanged);
            // 
            // grpBoxMainFormGeneral
            // 
            this.grpBoxMainFormGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxMainFormGeneral.Controls.Add(this.chkShowPropDesc);
            this.grpBoxMainFormGeneral.Controls.Add(this.chkShowHex);
            this.grpBoxMainFormGeneral.Location = new System.Drawing.Point(8, 6);
            this.grpBoxMainFormGeneral.Name = "grpBoxMainFormGeneral";
            this.grpBoxMainFormGeneral.Size = new System.Drawing.Size(302, 75);
            this.grpBoxMainFormGeneral.TabIndex = 15;
            this.grpBoxMainFormGeneral.TabStop = false;
            this.grpBoxMainFormGeneral.Text = "Main Form";
            // 
            // tabCompression
            // 
            this.tabCompression.BackColor = System.Drawing.SystemColors.Control;
            this.tabCompression.Controls.Add(this.groupBoxModuleCompression);
            this.tabCompression.Controls.Add(this.groupBoxStageCompression);
            this.tabCompression.Controls.Add(this.groupBoxFighterCompression);
            this.tabCompression.Location = new System.Drawing.Point(4, 22);
            this.tabCompression.Name = "tabCompression";
            this.tabCompression.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompression.Size = new System.Drawing.Size(318, 319);
            this.tabCompression.TabIndex = 3;
            this.tabCompression.Text = "Compression";
            // 
            // groupBoxModuleCompression
            // 
            this.groupBoxModuleCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxModuleCompression.Controls.Add(this.chkBoxModuleCompress);
            this.groupBoxModuleCompression.Location = new System.Drawing.Point(8, 146);
            this.groupBoxModuleCompression.Name = "groupBoxModuleCompression";
            this.groupBoxModuleCompression.Size = new System.Drawing.Size(302, 53);
            this.groupBoxModuleCompression.TabIndex = 18;
            this.groupBoxModuleCompression.TabStop = false;
            this.groupBoxModuleCompression.Text = "Modules";
            // 
            // chkBoxModuleCompress
            // 
            this.chkBoxModuleCompress.AutoSize = true;
            this.chkBoxModuleCompress.Location = new System.Drawing.Point(10, 22);
            this.chkBoxModuleCompress.Name = "chkBoxModuleCompress";
            this.chkBoxModuleCompress.Size = new System.Drawing.Size(157, 17);
            this.chkBoxModuleCompress.TabIndex = 7;
            this.chkBoxModuleCompress.Text = "Automatically compress files";
            this.chkBoxModuleCompress.UseVisualStyleBackColor = true;
            this.chkBoxModuleCompress.CheckedChanged += new System.EventHandler(this.chkBoxModuleCompress_CheckedChanged);
            // 
            // groupBoxStageCompression
            // 
            this.groupBoxStageCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStageCompression.Controls.Add(this.chkBoxStageCompress);
            this.groupBoxStageCompression.Location = new System.Drawing.Point(8, 87);
            this.groupBoxStageCompression.Name = "groupBoxStageCompression";
            this.groupBoxStageCompression.Size = new System.Drawing.Size(302, 53);
            this.groupBoxStageCompression.TabIndex = 17;
            this.groupBoxStageCompression.TabStop = false;
            this.groupBoxStageCompression.Text = "Stages";
            // 
            // chkBoxStageCompress
            // 
            this.chkBoxStageCompress.AutoSize = true;
            this.chkBoxStageCompress.Location = new System.Drawing.Point(10, 22);
            this.chkBoxStageCompress.Name = "chkBoxStageCompress";
            this.chkBoxStageCompress.Size = new System.Drawing.Size(157, 17);
            this.chkBoxStageCompress.TabIndex = 7;
            this.chkBoxStageCompress.Text = "Automatically compress files";
            this.chkBoxStageCompress.UseVisualStyleBackColor = true;
            this.chkBoxStageCompress.CheckedChanged += new System.EventHandler(this.chkBoxStageCompress_CheckedChanged);
            // 
            // groupBoxFighterCompression
            // 
            this.groupBoxFighterCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFighterCompression.Controls.Add(this.chkBoxFighterPacDecompress);
            this.groupBoxFighterCompression.Controls.Add(this.chkBoxFighterPcsCompress);
            this.groupBoxFighterCompression.Location = new System.Drawing.Point(8, 6);
            this.groupBoxFighterCompression.Name = "groupBoxFighterCompression";
            this.groupBoxFighterCompression.Size = new System.Drawing.Size(302, 75);
            this.groupBoxFighterCompression.TabIndex = 16;
            this.groupBoxFighterCompression.TabStop = false;
            this.groupBoxFighterCompression.Text = "Fighters";
            // 
            // chkBoxFighterPacDecompress
            // 
            this.chkBoxFighterPacDecompress.AutoSize = true;
            this.chkBoxFighterPacDecompress.Location = new System.Drawing.Point(10, 22);
            this.chkBoxFighterPacDecompress.Name = "chkBoxFighterPacDecompress";
            this.chkBoxFighterPacDecompress.Size = new System.Drawing.Size(193, 17);
            this.chkBoxFighterPacDecompress.TabIndex = 7;
            this.chkBoxFighterPacDecompress.Text = "Automatically decompress PAC files";
            this.chkBoxFighterPacDecompress.UseVisualStyleBackColor = true;
            this.chkBoxFighterPacDecompress.CheckedChanged += new System.EventHandler(this.chkBoxFighterPacDecompress_CheckedChanged);
            // 
            // chkBoxFighterPcsCompress
            // 
            this.chkBoxFighterPcsCompress.AutoSize = true;
            this.chkBoxFighterPcsCompress.Location = new System.Drawing.Point(10, 45);
            this.chkBoxFighterPcsCompress.Name = "chkBoxFighterPcsCompress";
            this.chkBoxFighterPcsCompress.Size = new System.Drawing.Size(181, 17);
            this.chkBoxFighterPcsCompress.TabIndex = 9;
            this.chkBoxFighterPcsCompress.Text = "Automatically compress PCS files";
            this.chkBoxFighterPcsCompress.UseVisualStyleBackColor = true;
            this.chkBoxFighterPcsCompress.CheckedChanged += new System.EventHandler(this.chkBoxFighterPcsCompress_CheckedChanged);
            // 
            // tabFileAssociations
            // 
            this.tabFileAssociations.Controls.Add(this.lblAdminApproval);
            this.tabFileAssociations.Controls.Add(this.btnApply);
            this.tabFileAssociations.Controls.Add(this.groupBox1);
            this.tabFileAssociations.Location = new System.Drawing.Point(4, 22);
            this.tabFileAssociations.Name = "tabFileAssociations";
            this.tabFileAssociations.Size = new System.Drawing.Size(318, 319);
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
            this.lblAdminApproval.Size = new System.Drawing.Size(312, 18);
            this.lblAdminApproval.TabIndex = 5;
            this.lblAdminApproval.Text = "Administrator access required to make changes";
            this.lblAdminApproval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(240, 291);
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
            this.groupBox1.Size = new System.Drawing.Size(302, 282);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.Location = new System.Drawing.Point(195, 256);
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
            this.listView1.Size = new System.Drawing.Size(293, 239);
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
            // tabDiscord
            // 
            this.tabDiscord.BackColor = System.Drawing.SystemColors.Control;
            this.tabDiscord.Controls.Add(this.grpBoxDiscordRPC);
            this.tabDiscord.Location = new System.Drawing.Point(4, 22);
            this.tabDiscord.Name = "tabDiscord";
            this.tabDiscord.Padding = new System.Windows.Forms.Padding(3);
            this.tabDiscord.Size = new System.Drawing.Size(318, 319);
            this.tabDiscord.TabIndex = 4;
            this.tabDiscord.Text = "Discord";
            // 
            // grpBoxDiscordRPC
            // 
            this.grpBoxDiscordRPC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxDiscordRPC.Controls.Add(this.chkBoxEnableDiscordRPC);
            this.grpBoxDiscordRPC.Controls.Add(this.grpBoxDiscordRPCType);
            this.grpBoxDiscordRPC.Location = new System.Drawing.Point(8, 6);
            this.grpBoxDiscordRPC.Name = "grpBoxDiscordRPC";
            this.grpBoxDiscordRPC.Size = new System.Drawing.Size(302, 171);
            this.grpBoxDiscordRPC.TabIndex = 0;
            this.grpBoxDiscordRPC.TabStop = false;
            this.grpBoxDiscordRPC.Text = "Rich Presence";
            // 
            // chkBoxEnableDiscordRPC
            // 
            this.chkBoxEnableDiscordRPC.AutoSize = true;
            this.chkBoxEnableDiscordRPC.Location = new System.Drawing.Point(10, 22);
            this.chkBoxEnableDiscordRPC.Name = "chkBoxEnableDiscordRPC";
            this.chkBoxEnableDiscordRPC.Size = new System.Drawing.Size(171, 17);
            this.chkBoxEnableDiscordRPC.TabIndex = 1;
            this.chkBoxEnableDiscordRPC.Text = "Enable Discord Rich Presence";
            this.chkBoxEnableDiscordRPC.UseVisualStyleBackColor = true;
            this.chkBoxEnableDiscordRPC.CheckedChanged += new System.EventHandler(this.ChkBoxEnableDiscordRPC_CheckedChanged);
            // 
            // grpBoxDiscordRPCType
            // 
            this.grpBoxDiscordRPCType.Controls.Add(this.DiscordRPCCustomName);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameCustom);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameExternal);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameInternal);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameDisabled);
            this.grpBoxDiscordRPCType.Location = new System.Drawing.Point(6, 45);
            this.grpBoxDiscordRPCType.Name = "grpBoxDiscordRPCType";
            this.grpBoxDiscordRPCType.Size = new System.Drawing.Size(290, 119);
            this.grpBoxDiscordRPCType.TabIndex = 0;
            this.grpBoxDiscordRPCType.TabStop = false;
            this.grpBoxDiscordRPCType.Text = "Mod Name Detection";
            // 
            // DiscordRPCCustomName
            // 
            this.DiscordRPCCustomName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DiscordRPCCustomName.Location = new System.Drawing.Point(30, 88);
            this.DiscordRPCCustomName.Name = "DiscordRPCCustomName";
            this.DiscordRPCCustomName.Size = new System.Drawing.Size(254, 20);
            this.DiscordRPCCustomName.TabIndex = 2;
            this.DiscordRPCCustomName.Text = "My Mod";
            this.DiscordRPCCustomName.TextChanged += new System.EventHandler(this.DiscordRPCCustomName_TextChanged);
            // 
            // rdoDiscordRPCNameCustom
            // 
            this.rdoDiscordRPCNameCustom.AutoSize = true;
            this.rdoDiscordRPCNameCustom.Location = new System.Drawing.Point(10, 91);
            this.rdoDiscordRPCNameCustom.Name = "rdoDiscordRPCNameCustom";
            this.rdoDiscordRPCNameCustom.Size = new System.Drawing.Size(14, 13);
            this.rdoDiscordRPCNameCustom.TabIndex = 3;
            this.rdoDiscordRPCNameCustom.TabStop = true;
            this.rdoDiscordRPCNameCustom.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameCustom.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameExternal
            // 
            this.rdoDiscordRPCNameExternal.AutoSize = true;
            this.rdoDiscordRPCNameExternal.Location = new System.Drawing.Point(10, 68);
            this.rdoDiscordRPCNameExternal.Name = "rdoDiscordRPCNameExternal";
            this.rdoDiscordRPCNameExternal.Size = new System.Drawing.Size(126, 17);
            this.rdoDiscordRPCNameExternal.TabIndex = 2;
            this.rdoDiscordRPCNameExternal.TabStop = true;
            this.rdoDiscordRPCNameExternal.Text = "Use external filename";
            this.rdoDiscordRPCNameExternal.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameExternal.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameInternal
            // 
            this.rdoDiscordRPCNameInternal.AutoSize = true;
            this.rdoDiscordRPCNameInternal.Location = new System.Drawing.Point(10, 45);
            this.rdoDiscordRPCNameInternal.Name = "rdoDiscordRPCNameInternal";
            this.rdoDiscordRPCNameInternal.Size = new System.Drawing.Size(123, 17);
            this.rdoDiscordRPCNameInternal.TabIndex = 1;
            this.rdoDiscordRPCNameInternal.TabStop = true;
            this.rdoDiscordRPCNameInternal.Text = "Use internal filename";
            this.rdoDiscordRPCNameInternal.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameInternal.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameDisabled
            // 
            this.rdoDiscordRPCNameDisabled.AutoSize = true;
            this.rdoDiscordRPCNameDisabled.Location = new System.Drawing.Point(10, 22);
            this.rdoDiscordRPCNameDisabled.Name = "rdoDiscordRPCNameDisabled";
            this.rdoDiscordRPCNameDisabled.Size = new System.Drawing.Size(66, 17);
            this.rdoDiscordRPCNameDisabled.TabIndex = 0;
            this.rdoDiscordRPCNameDisabled.TabStop = true;
            this.rdoDiscordRPCNameDisabled.Text = "Disabled";
            this.rdoDiscordRPCNameDisabled.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameDisabled.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // tabUpdater
            // 
            this.tabUpdater.Controls.Add(this.grpBoxCanary);
            this.tabUpdater.Controls.Add(this.updaterBehaviorGroupbox);
            this.tabUpdater.Location = new System.Drawing.Point(4, 22);
            this.tabUpdater.Name = "tabUpdater";
            this.tabUpdater.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdater.Size = new System.Drawing.Size(318, 319);
            this.tabUpdater.TabIndex = 1;
            this.tabUpdater.Text = "Updater";
            // 
            // grpBoxCanary
            // 
            this.grpBoxCanary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxCanary.Controls.Add(this.btnCanaryBranch);
            this.grpBoxCanary.Controls.Add(this.chkCanary);
            this.grpBoxCanary.Location = new System.Drawing.Point(8, 132);
            this.grpBoxCanary.Name = "grpBoxCanary";
            this.grpBoxCanary.Size = new System.Drawing.Size(302, 71);
            this.grpBoxCanary.TabIndex = 15;
            this.grpBoxCanary.TabStop = false;
            this.grpBoxCanary.Text = "BrawlCrate Canary";
            // 
            // btnCanaryBranch
            // 
            this.btnCanaryBranch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCanaryBranch.Location = new System.Drawing.Point(79, 42);
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
            this.updaterBehaviorGroupbox.Size = new System.Drawing.Size(302, 120);
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
            this.ClientSize = new System.Drawing.Size(326, 345);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.Shown += new System.EventHandler(this.SettingsDialog_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.grpBoxMDL0General.ResumeLayout(false);
            this.grpBoxMDL0General.PerformLayout();
            this.grpBoxAudioGeneral.ResumeLayout(false);
            this.grpBoxAudioGeneral.PerformLayout();
            this.grpBoxMainFormGeneral.ResumeLayout(false);
            this.grpBoxMainFormGeneral.PerformLayout();
            this.tabCompression.ResumeLayout(false);
            this.groupBoxModuleCompression.ResumeLayout(false);
            this.groupBoxModuleCompression.PerformLayout();
            this.groupBoxStageCompression.ResumeLayout(false);
            this.groupBoxStageCompression.PerformLayout();
            this.groupBoxFighterCompression.ResumeLayout(false);
            this.groupBoxFighterCompression.PerformLayout();
            this.tabFileAssociations.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabDiscord.ResumeLayout(false);
            this.grpBoxDiscordRPC.ResumeLayout(false);
            this.grpBoxDiscordRPC.PerformLayout();
            this.grpBoxDiscordRPCType.ResumeLayout(false);
            this.grpBoxDiscordRPCType.PerformLayout();
            this.tabUpdater.ResumeLayout(false);
            this.grpBoxCanary.ResumeLayout(false);
            this.grpBoxCanary.PerformLayout();
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
                    MainForm.Instance.Canary = true;
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
                    MainForm.Instance.Canary = false;
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
            if(MessageBox.Show(this, "Warning: Switching branches or repositories can be unstable unless you know what you're doing. You should generally stay on the brawlcrate-master branch unless directed otherwise for testing purposes. You can reset to the default for either field by leaving it blank.", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string cRepo = MainForm.currentRepo;
                string cBranch = MainForm.currentBranch;
                TwoInputStringDialog d = new TwoInputStringDialog();
                if(d.ShowDialog(this, "Enter new repo/branch to track", "Repo:", cRepo, "Branch:", cBranch) == DialogResult.OK)
                {
                    if (!d.InputText1.Equals(cRepo, StringComparison.OrdinalIgnoreCase) || !d.InputText2.Equals(cBranch, StringComparison.OrdinalIgnoreCase))
                    {
                        MainForm.SetCanaryTracking(d.InputText1, d.InputText2);
                    }
                }
            }
        }

        private void chkBoxAutoPlayAudio_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.AutoPlayAudio = chkBoxAutoPlayAudio.Checked;
        }

        private void chkBoxFighterPacDecompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.AutoDecompressFighterPAC = chkBoxFighterPacDecompress.Checked;
        }

        private void chkBoxFighterPcsCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.AutoCompressPCS = chkBoxFighterPcsCompress.Checked;
        }

        private void chkBoxStageCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.AutoCompressStages = chkBoxStageCompress.Checked;
        }

        private void chkBoxModuleCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                if (chkBoxModuleCompress.Checked)
                {
                    if(MessageBox.Show("Warning: Module compression does not save much space and can reduce editablity of modules. Are you sure you want to turn this on?", "Module Compressor", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        _updating = true;
                        chkBoxModuleCompress.Checked = false;
                        _updating = false;
                        return;
                    }
                }
                MainForm.Instance.AutoCompressModules = chkBoxModuleCompress.Checked;
            }
        }

        private void chkBoxMDL0Compatibility_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
                MainForm.Instance.CompatibilityMode = chkBoxMDL0Compatibility.Checked;
        }

        private void ChkBoxEnableDiscordRPC_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                BrawlCrate.Properties.Settings.Default.DiscordRPCEnabled = chkBoxEnableDiscordRPC.Checked;
                BrawlCrate.Properties.Settings.Default.Save();
                grpBoxDiscordRPCType.Enabled = chkBoxEnableDiscordRPC.Checked;
                BrawlCrate.Discord.DiscordSettings.LoadSettings(true);
            }
        }

        private void DiscordRPCNameSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                if (rdoDiscordRPCNameDisabled.Checked)
                    BrawlCrate.Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.Disabled;
                else if (rdoDiscordRPCNameInternal.Checked)
                    BrawlCrate.Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.AutoInternal;
                else if (rdoDiscordRPCNameExternal.Checked)
                    BrawlCrate.Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.AutoExternal;
                else if (rdoDiscordRPCNameCustom.Checked)
                    BrawlCrate.Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.UserDefined;
                DiscordRPCCustomName.Enabled = rdoDiscordRPCNameCustom.Checked;
                DiscordRPCCustomName.ReadOnly = !rdoDiscordRPCNameCustom.Checked;
                BrawlCrate.Properties.Settings.Default.Save();
                Discord.DiscordSettings.LoadSettings(true);
            }
        }

        private void DiscordRPCCustomName_TextChanged(object sender, EventArgs e)
        {
            BrawlCrate.Properties.Settings.Default.DiscordRPCNameCustom = DiscordRPCCustomName.Text;
            BrawlCrate.Properties.Settings.Default.Save();
            if (rdoDiscordRPCNameCustom.Checked)
                Discord.DiscordSettings.LoadSettings(true);
        }
    }
}