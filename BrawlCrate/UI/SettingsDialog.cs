﻿using BrawlLib.SSBB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate
{
    internal class SettingsDialog : Form
    {
        private bool _updating;

        static SettingsDialog()
        {
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
            {
                if (info._forEditing)
                {
                    foreach (string s in info._extensions)
                    {
                        if (!s.Equals("dat", StringComparison.OrdinalIgnoreCase) &&
                            !s.Equals("bin", StringComparison.OrdinalIgnoreCase))
                        {
                            _assocList.Add(FileAssociation.Get("." + s));
                            _typeList.Add(FileType.Get("SSBB." + s.ToUpper()));
                        }
                    }
                }
            }
        }

        private static readonly List<FileAssociation> _assocList = new List<FileAssociation>();
        private static readonly List<FileType> _typeList = new List<FileType>();
        private CheckBox chkShowHex;
        private CheckBox chkDocUpdates;
        private CheckBox chkCanary;
        private TabControl tabControl1;
        private TabPage tabGeneral;
        private TabPage tabUpdater;
        private GroupBox updaterBehaviorGroupbox;
        private TabPage tabFileAssociations;
        private Button btnApply;
        private GroupBox associatiedFilesBox;
        private CheckBox checkBox1;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private GroupBox grpBoxCanary;
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
        private GroupBox genericFileAssociationBox;
        private CheckBox binFileAssociation;
        private CheckBox datFileAssociation;
        private GroupBox grpBoxFileNameDisplayGeneral;
        private RadioButton rdoShowShortName;
        private RadioButton rdoShowFullPath;
        private Label lblRecentFiles;
        private NumericInputBox recentFileCountBox;
        private CheckBox chkShowPropDesc;

        public SettingsDialog()
        {
            InitializeComponent();

            tabUpdater.Enabled = tabUpdater.Visible =
                MainForm.Instance.checkForUpdatesToolStripMenuItem.Enabled;

            if (!MainForm.Instance.checkForUpdatesToolStripMenuItem.Enabled)
            {
                tabControl1.TabPages.Remove(tabUpdater);
            }

            listView1.Items.Clear();
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
            {
                if (info._forEditing)
                {
                    foreach (string s in info._extensions)
                    {
                        if (s != "dat" && s != "bin")
                        {
                            listView1.Items.Add(new ListViewItem()
                                {Text = string.Format("{0} (*.{1})", info._name, s)});
                        }
                    }
                }
            }
        }

        private void Apply()
        {
            try
            {
                bool check;
                int index = 0;
                foreach (ListViewItem i in listView1.Items)
                {
                    if ((check = i.Checked) != (bool) i.Tag)
                    {
                        if (check)
                        {
                            _assocList[index].FileType = _typeList[index];
                            _typeList[index].SetCommand("open", string.Format("\"{0}\" \"%1\"", Program.FullPath));
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
                if (datFileAssociation.Checked)
                {
                    FileAssociation.Get(".dat").FileType = FileType.Get("SSBB.DAT");
                    FileType.Get("SSBB.DAT").SetCommand("open", string.Format("\"{0}\" \"%1\"", Program.FullPath));
                }
                else
                {
                    FileType.Get("SSBB.DAT").Delete();
                    FileAssociation.Get(".dat").Delete();
                }

                if (binFileAssociation.Checked)
                {
                    FileAssociation.Get(".bin").FileType = FileType.Get("SSBB.BIN");
                    FileType.Get("SSBB.BIN").SetCommand("open", string.Format("\"{0}\" \"%1\"", Program.FullPath));
                }
                else
                {
                    FileType.Get("SSBB.BIN").Delete();
                    FileAssociation.Get(".bin").Delete();
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(null,
                    "Unable to access the registry to set file associations.\nRun the program as administrator and try again.",
                    "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(null,
                    "Unable to access the registry to set file associations.\nRun the program as administrator and try again.",
                    "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (_typeList[index] == _assocList[index].FileType &&
                        !string.IsNullOrEmpty(cmd = _typeList[index].GetCommand("open")) &&
                        cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        i.Tag = i.Checked = true;
                    }
                    else
                    {
                        i.Tag = i.Checked = false;
                    }
                }
                catch
                {
                }

                index++;
            }

            _updating = true;
            try
            {
                datFileAssociation.Checked = !string.IsNullOrEmpty(cmd = FileType.Get("SSBB.DAT").GetCommand("open")) &&
                                             cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch
            {
            }

            try
            {
                binFileAssociation.Checked = !string.IsNullOrEmpty(cmd = FileType.Get("SSBB.BIN").GetCommand("open")) &&
                                             cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch
            {
            }

            chkDocUpdates.Checked = MainForm.Instance.GetDocumentationUpdates;
            updaterBehaviorGroupbox.Enabled = !MainForm.Instance.Canary;
            if (MainForm.Instance.UpdateAutomatically)
            {
                rdoAutoUpdate.Checked = true;
            }
            else if (MainForm.Instance.CheckUpdatesOnStartup)
            {
                rdoCheckStartup.Checked = true;
            }
            else
            {
                rdoCheckManual.Checked = true;
            }

            rdoShowFullPath.Checked = MainForm.Instance.ShowFullPath;
            rdoShowShortName.Checked = !MainForm.Instance.ShowFullPath;
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
            recentFileCountBox.Value = Properties.Settings.Default.RecentFilesMax;

            Discord.DiscordSettings.LoadSettings();
            grpBoxDiscordRPCType.Enabled = chkBoxEnableDiscordRPC.Checked = Discord.DiscordSettings.enabled;
            if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.Disabled)
            {
                rdoDiscordRPCNameDisabled.Checked = true;
            }
            else if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.UserDefined)
            {
                rdoDiscordRPCNameCustom.Checked = true;
            }
            else if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.AutoInternal)
            {
                rdoDiscordRPCNameInternal.Checked = true;
            }
            else if (Discord.DiscordSettings.modNameType == Discord.DiscordSettings.ModNameType.AutoExternal)
            {
                rdoDiscordRPCNameExternal.Checked = true;
            }

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
                associatiedFilesBox.Enabled = true;
                return true;
            }
            catch
            {
                lblAdminApproval.Visible = true;
                btnApply.Visible = false;
                associatiedFilesBox.Enabled = false;
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
            {
                Apply();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region Designer

        private void InitializeComponent()
        {
            ListViewItem listViewItem1 = new ListViewItem("ARChive Pack (*.pac)");
            ListViewItem listViewItem2 = new ListViewItem("Compressed ARChive Pack (*.pcs)");
            ListViewItem listViewItem3 = new ListViewItem("ARChive (*.arc)");
            ListViewItem listViewItem4 = new ListViewItem("Compressed ARChive (*.szs)");
            ListViewItem listViewItem5 = new ListViewItem("Resource Pack (*.brres)");
            ListViewItem listViewItem6 = new ListViewItem("Model Pack (*.brmdl)");
            ListViewItem listViewItem7 = new ListViewItem("Texture Pack (*.brtex)");
            ListViewItem listViewItem8 = new ListViewItem("MSBin Message List (*.msbin)");
            ListViewItem listViewItem9 = new ListViewItem("Sound Archive (*.brsar)");
            ListViewItem listViewItem10 = new ListViewItem("Sound Stream (*.brstm)");
            ListViewItem listViewItem11 = new ListViewItem("Texture (*.tex0)");
            ListViewItem listViewItem12 = new ListViewItem("Palette (*.plt0)");
            ListViewItem listViewItem13 = new ListViewItem("Model (*.mdl0)");
            ListViewItem listViewItem14 = new ListViewItem("Model Animation (*.chr0)");
            ListViewItem listViewItem15 = new ListViewItem("Texture Animation (*.srt0)");
            ListViewItem listViewItem16 = new ListViewItem("Vertex Morph (*.shp0)");
            ListViewItem listViewItem17 = new ListViewItem("Texture Pattern (*.pat0)");
            ListViewItem listViewItem18 = new ListViewItem("Bone Visibility (*.vis0)");
            ListViewItem listViewItem19 = new ListViewItem("Scene Settings (*.scn0)");
            ListViewItem listViewItem20 = new ListViewItem("Color Sequence (*.clr0)");
            ListViewItem listViewItem21 = new ListViewItem("Effect List (*.efls)");
            ListViewItem listViewItem22 = new ListViewItem("Effect Parameters (*.breff)");
            ListViewItem listViewItem23 = new ListViewItem("Effect Textures (*.breft)");
            ListViewItem listViewItem24 = new ListViewItem("Sound Stream (*.brwsd)");
            ListViewItem listViewItem25 = new ListViewItem("Sound Bank (*.brbnk)");
            ListViewItem listViewItem26 = new ListViewItem("Sound Sequence (*.brseq)");
            ListViewItem listViewItem27 = new ListViewItem("Static Module (*.dol)");
            ListViewItem listViewItem28 = new ListViewItem("Relocatable Module (*.rel)");
            ListViewItem listViewItem29 = new ListViewItem("Texture Archive (*.tpl)");
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            chkShowPropDesc = new CheckBox();
            chkShowHex = new CheckBox();
            chkDocUpdates = new CheckBox();
            chkCanary = new CheckBox();
            tabControl1 = new TabControl();
            tabGeneral = new TabPage();
            grpBoxMDL0General = new GroupBox();
            chkBoxMDL0Compatibility = new CheckBox();
            grpBoxAudioGeneral = new GroupBox();
            chkBoxAutoPlayAudio = new CheckBox();
            grpBoxMainFormGeneral = new GroupBox();
            lblRecentFiles = new Label();
            recentFileCountBox = new NumericInputBox();
            grpBoxFileNameDisplayGeneral = new GroupBox();
            rdoShowShortName = new RadioButton();
            rdoShowFullPath = new RadioButton();
            tabCompression = new TabPage();
            groupBoxModuleCompression = new GroupBox();
            chkBoxModuleCompress = new CheckBox();
            groupBoxStageCompression = new GroupBox();
            chkBoxStageCompress = new CheckBox();
            groupBoxFighterCompression = new GroupBox();
            chkBoxFighterPacDecompress = new CheckBox();
            chkBoxFighterPcsCompress = new CheckBox();
            tabFileAssociations = new TabPage();
            genericFileAssociationBox = new GroupBox();
            binFileAssociation = new CheckBox();
            datFileAssociation = new CheckBox();
            lblAdminApproval = new Label();
            btnApply = new Button();
            associatiedFilesBox = new GroupBox();
            checkBox1 = new CheckBox();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            tabDiscord = new TabPage();
            grpBoxDiscordRPC = new GroupBox();
            chkBoxEnableDiscordRPC = new CheckBox();
            grpBoxDiscordRPCType = new GroupBox();
            DiscordRPCCustomName = new TextBox();
            rdoDiscordRPCNameCustom = new RadioButton();
            rdoDiscordRPCNameExternal = new RadioButton();
            rdoDiscordRPCNameInternal = new RadioButton();
            rdoDiscordRPCNameDisabled = new RadioButton();
            tabUpdater = new TabPage();
            grpBoxCanary = new GroupBox();
            updaterBehaviorGroupbox = new GroupBox();
            rdoAutoUpdate = new RadioButton();
            rdoCheckManual = new RadioButton();
            rdoCheckStartup = new RadioButton();
            tabControl1.SuspendLayout();
            tabGeneral.SuspendLayout();
            grpBoxMDL0General.SuspendLayout();
            grpBoxAudioGeneral.SuspendLayout();
            grpBoxMainFormGeneral.SuspendLayout();
            grpBoxFileNameDisplayGeneral.SuspendLayout();
            tabCompression.SuspendLayout();
            groupBoxModuleCompression.SuspendLayout();
            groupBoxStageCompression.SuspendLayout();
            groupBoxFighterCompression.SuspendLayout();
            tabFileAssociations.SuspendLayout();
            genericFileAssociationBox.SuspendLayout();
            associatiedFilesBox.SuspendLayout();
            tabDiscord.SuspendLayout();
            grpBoxDiscordRPC.SuspendLayout();
            grpBoxDiscordRPCType.SuspendLayout();
            tabUpdater.SuspendLayout();
            grpBoxCanary.SuspendLayout();
            updaterBehaviorGroupbox.SuspendLayout();
            SuspendLayout();
            // 
            // chkShowPropDesc
            // 
            chkShowPropDesc.AutoSize = true;
            chkShowPropDesc.Location = new System.Drawing.Point(10, 22);
            chkShowPropDesc.Name = "chkShowPropDesc";
            chkShowPropDesc.Size = new System.Drawing.Size(242, 17);
            chkShowPropDesc.TabIndex = 7;
            chkShowPropDesc.Text = "Show property description box when available";
            chkShowPropDesc.UseVisualStyleBackColor = true;
            chkShowPropDesc.CheckedChanged += chkShowPropDesc_CheckedChanged;
            // 
            // chkShowHex
            // 
            chkShowHex.AutoSize = true;
            chkShowHex.Location = new System.Drawing.Point(10, 45);
            chkShowHex.Name = "chkShowHex";
            chkShowHex.Size = new System.Drawing.Size(233, 17);
            chkShowHex.TabIndex = 9;
            chkShowHex.Text = "Show hexadecimal for files without previews";
            chkShowHex.UseVisualStyleBackColor = true;
            chkShowHex.CheckedChanged += chkShowHex_CheckedChanged;
            // 
            // chkDocUpdates
            // 
            chkDocUpdates.AutoSize = true;
            chkDocUpdates.Location = new System.Drawing.Point(10, 91);
            chkDocUpdates.Name = "chkDocUpdates";
            chkDocUpdates.Size = new System.Drawing.Size(180, 17);
            chkDocUpdates.TabIndex = 11;
            chkDocUpdates.Text = "Receive documentation updates";
            chkDocUpdates.UseVisualStyleBackColor = true;
            chkDocUpdates.CheckedChanged += chkDocUpdates_CheckedChanged;
            // 
            // chkCanary
            // 
            chkCanary.AutoSize = true;
            chkCanary.Location = new System.Drawing.Point(10, 22);
            chkCanary.Name = "chkCanary";
            chkCanary.Size = new System.Drawing.Size(263, 17);
            chkCanary.TabIndex = 13;
            chkCanary.Text = "Opt into BrawlCrate Canary (Experimental) updates";
            chkCanary.UseVisualStyleBackColor = true;
            chkCanary.CheckedChanged += chkCanary_CheckedChanged;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabGeneral);
            tabControl1.Controls.Add(tabCompression);
            tabControl1.Controls.Add(tabFileAssociations);
            tabControl1.Controls.Add(tabDiscord);
            tabControl1.Controls.Add(tabUpdater);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(326, 427);
            tabControl1.TabIndex = 48;
            // 
            // tabGeneral
            // 
            tabGeneral.BackColor = System.Drawing.SystemColors.Control;
            tabGeneral.Controls.Add(grpBoxMDL0General);
            tabGeneral.Controls.Add(grpBoxAudioGeneral);
            tabGeneral.Controls.Add(grpBoxMainFormGeneral);
            tabGeneral.Location = new System.Drawing.Point(4, 22);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Padding = new Padding(3);
            tabGeneral.Size = new System.Drawing.Size(318, 401);
            tabGeneral.TabIndex = 0;
            tabGeneral.Text = "General";
            // 
            // grpBoxMDL0General
            // 
            grpBoxMDL0General.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                        | AnchorStyles.Right;
            grpBoxMDL0General.Controls.Add(chkBoxMDL0Compatibility);
            grpBoxMDL0General.Location = new System.Drawing.Point(8, 241);
            grpBoxMDL0General.Name = "grpBoxMDL0General";
            grpBoxMDL0General.Size = new System.Drawing.Size(302, 53);
            grpBoxMDL0General.TabIndex = 19;
            grpBoxMDL0General.TabStop = false;
            grpBoxMDL0General.Text = "Models";
            // 
            // chkBoxMDL0Compatibility
            // 
            chkBoxMDL0Compatibility.AutoSize = true;
            chkBoxMDL0Compatibility.Location = new System.Drawing.Point(10, 22);
            chkBoxMDL0Compatibility.Name = "chkBoxMDL0Compatibility";
            chkBoxMDL0Compatibility.Size = new System.Drawing.Size(134, 17);
            chkBoxMDL0Compatibility.TabIndex = 7;
            chkBoxMDL0Compatibility.Text = "Use compatibility mode";
            chkBoxMDL0Compatibility.UseVisualStyleBackColor = true;
            chkBoxMDL0Compatibility.CheckedChanged += chkBoxMDL0Compatibility_CheckedChanged;
            // 
            // grpBoxAudioGeneral
            // 
            grpBoxAudioGeneral.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                         | AnchorStyles.Right;
            grpBoxAudioGeneral.Controls.Add(chkBoxAutoPlayAudio);
            grpBoxAudioGeneral.Location = new System.Drawing.Point(8, 182);
            grpBoxAudioGeneral.Name = "grpBoxAudioGeneral";
            grpBoxAudioGeneral.Size = new System.Drawing.Size(302, 53);
            grpBoxAudioGeneral.TabIndex = 18;
            grpBoxAudioGeneral.TabStop = false;
            grpBoxAudioGeneral.Text = "Audio";
            // 
            // chkBoxAutoPlayAudio
            // 
            chkBoxAutoPlayAudio.AutoSize = true;
            chkBoxAutoPlayAudio.Location = new System.Drawing.Point(10, 22);
            chkBoxAutoPlayAudio.Name = "chkBoxAutoPlayAudio";
            chkBoxAutoPlayAudio.Size = new System.Drawing.Size(171, 17);
            chkBoxAutoPlayAudio.TabIndex = 7;
            chkBoxAutoPlayAudio.Text = "Automatically play audio nodes";
            chkBoxAutoPlayAudio.UseVisualStyleBackColor = true;
            chkBoxAutoPlayAudio.CheckedChanged += chkBoxAutoPlayAudio_CheckedChanged;
            // 
            // grpBoxMainFormGeneral
            // 
            grpBoxMainFormGeneral.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                            | AnchorStyles.Right;
            grpBoxMainFormGeneral.Controls.Add(lblRecentFiles);
            grpBoxMainFormGeneral.Controls.Add(recentFileCountBox);
            grpBoxMainFormGeneral.Controls.Add(grpBoxFileNameDisplayGeneral);
            grpBoxMainFormGeneral.Controls.Add(chkShowPropDesc);
            grpBoxMainFormGeneral.Controls.Add(chkShowHex);
            grpBoxMainFormGeneral.Location = new System.Drawing.Point(8, 6);
            grpBoxMainFormGeneral.Name = "grpBoxMainFormGeneral";
            grpBoxMainFormGeneral.Size = new System.Drawing.Size(302, 170);
            grpBoxMainFormGeneral.TabIndex = 15;
            grpBoxMainFormGeneral.TabStop = false;
            grpBoxMainFormGeneral.Text = "Main Form";
            // 
            // lblRecentFiles
            // 
            lblRecentFiles.AutoSize = true;
            lblRecentFiles.Location = new System.Drawing.Point(8, 68);
            lblRecentFiles.Name = "lblRecentFiles";
            lblRecentFiles.Size = new System.Drawing.Size(120, 13);
            lblRecentFiles.TabIndex = 12;
            lblRecentFiles.Text = "Max Recent Files Count";
            // 
            // recentFileCountBox
            // 
            recentFileCountBox.Integer = true;
            recentFileCountBox.Integral = true;
            recentFileCountBox.Location = new System.Drawing.Point(134, 65);
            recentFileCountBox.MaximumValue = 3.402823E+38F;
            recentFileCountBox.MinimumValue = -3.402823E+38F;
            recentFileCountBox.Name = "recentFileCountBox";
            recentFileCountBox.Size = new System.Drawing.Size(100, 20);
            recentFileCountBox.TabIndex = 11;
            recentFileCountBox.Text = "0";
            recentFileCountBox.TextChanged += RecentFileCountBox_TextChanged;
            // 
            // grpBoxFileNameDisplayGeneral
            // 
            grpBoxFileNameDisplayGeneral.Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                                                                      | AnchorStyles.Right;
            grpBoxFileNameDisplayGeneral.Controls.Add(rdoShowShortName);
            grpBoxFileNameDisplayGeneral.Controls.Add(rdoShowFullPath);
            grpBoxFileNameDisplayGeneral.Location = new System.Drawing.Point(6, 89);
            grpBoxFileNameDisplayGeneral.Name = "grpBoxFileNameDisplayGeneral";
            grpBoxFileNameDisplayGeneral.Size = new System.Drawing.Size(290, 75);
            grpBoxFileNameDisplayGeneral.TabIndex = 10;
            grpBoxFileNameDisplayGeneral.TabStop = false;
            grpBoxFileNameDisplayGeneral.Text = "Filename Display";
            // 
            // rdoShowShortName
            // 
            rdoShowShortName.AutoSize = true;
            rdoShowShortName.Location = new System.Drawing.Point(10, 45);
            rdoShowShortName.Name = "rdoShowShortName";
            rdoShowShortName.Size = new System.Drawing.Size(94, 17);
            rdoShowShortName.TabIndex = 1;
            rdoShowShortName.TabStop = true;
            rdoShowShortName.Text = "Show filename";
            rdoShowShortName.UseVisualStyleBackColor = true;
            rdoShowShortName.CheckedChanged += RdoPathDisplay_CheckedChanged;
            // 
            // rdoShowFullPath
            // 
            rdoShowFullPath.AutoSize = true;
            rdoShowFullPath.Location = new System.Drawing.Point(10, 22);
            rdoShowFullPath.Name = "rdoShowFullPath";
            rdoShowFullPath.Size = new System.Drawing.Size(92, 17);
            rdoShowFullPath.TabIndex = 0;
            rdoShowFullPath.TabStop = true;
            rdoShowFullPath.Text = "Show full path";
            rdoShowFullPath.UseVisualStyleBackColor = true;
            rdoShowFullPath.CheckedChanged += RdoPathDisplay_CheckedChanged;
            // 
            // tabCompression
            // 
            tabCompression.BackColor = System.Drawing.SystemColors.Control;
            tabCompression.Controls.Add(groupBoxModuleCompression);
            tabCompression.Controls.Add(groupBoxStageCompression);
            tabCompression.Controls.Add(groupBoxFighterCompression);
            tabCompression.Location = new System.Drawing.Point(4, 22);
            tabCompression.Name = "tabCompression";
            tabCompression.Padding = new Padding(3);
            tabCompression.Size = new System.Drawing.Size(318, 401);
            tabCompression.TabIndex = 3;
            tabCompression.Text = "Compression";
            // 
            // groupBoxModuleCompression
            // 
            groupBoxModuleCompression.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right;
            groupBoxModuleCompression.Controls.Add(chkBoxModuleCompress);
            groupBoxModuleCompression.Location = new System.Drawing.Point(8, 146);
            groupBoxModuleCompression.Name = "groupBoxModuleCompression";
            groupBoxModuleCompression.Size = new System.Drawing.Size(302, 53);
            groupBoxModuleCompression.TabIndex = 18;
            groupBoxModuleCompression.TabStop = false;
            groupBoxModuleCompression.Text = "Modules";
            // 
            // chkBoxModuleCompress
            // 
            chkBoxModuleCompress.AutoSize = true;
            chkBoxModuleCompress.Location = new System.Drawing.Point(10, 22);
            chkBoxModuleCompress.Name = "chkBoxModuleCompress";
            chkBoxModuleCompress.Size = new System.Drawing.Size(251, 17);
            chkBoxModuleCompress.TabIndex = 7;
            chkBoxModuleCompress.Text = "Automatically compress files (not recommended)";
            chkBoxModuleCompress.UseVisualStyleBackColor = true;
            chkBoxModuleCompress.CheckedChanged += chkBoxModuleCompress_CheckedChanged;
            // 
            // groupBoxStageCompression
            // 
            groupBoxStageCompression.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                               | AnchorStyles.Right;
            groupBoxStageCompression.Controls.Add(chkBoxStageCompress);
            groupBoxStageCompression.Location = new System.Drawing.Point(8, 87);
            groupBoxStageCompression.Name = "groupBoxStageCompression";
            groupBoxStageCompression.Size = new System.Drawing.Size(302, 53);
            groupBoxStageCompression.TabIndex = 17;
            groupBoxStageCompression.TabStop = false;
            groupBoxStageCompression.Text = "Stages";
            // 
            // chkBoxStageCompress
            // 
            chkBoxStageCompress.AutoSize = true;
            chkBoxStageCompress.Location = new System.Drawing.Point(10, 22);
            chkBoxStageCompress.Name = "chkBoxStageCompress";
            chkBoxStageCompress.Size = new System.Drawing.Size(157, 17);
            chkBoxStageCompress.TabIndex = 7;
            chkBoxStageCompress.Text = "Automatically compress files";
            chkBoxStageCompress.UseVisualStyleBackColor = true;
            chkBoxStageCompress.CheckedChanged += chkBoxStageCompress_CheckedChanged;
            // 
            // groupBoxFighterCompression
            // 
            groupBoxFighterCompression.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                                 | AnchorStyles.Right;
            groupBoxFighterCompression.Controls.Add(chkBoxFighterPacDecompress);
            groupBoxFighterCompression.Controls.Add(chkBoxFighterPcsCompress);
            groupBoxFighterCompression.Location = new System.Drawing.Point(8, 6);
            groupBoxFighterCompression.Name = "groupBoxFighterCompression";
            groupBoxFighterCompression.Size = new System.Drawing.Size(302, 75);
            groupBoxFighterCompression.TabIndex = 16;
            groupBoxFighterCompression.TabStop = false;
            groupBoxFighterCompression.Text = "Fighters";
            // 
            // chkBoxFighterPacDecompress
            // 
            chkBoxFighterPacDecompress.AutoSize = true;
            chkBoxFighterPacDecompress.Location = new System.Drawing.Point(10, 22);
            chkBoxFighterPacDecompress.Name = "chkBoxFighterPacDecompress";
            chkBoxFighterPacDecompress.Size = new System.Drawing.Size(193, 17);
            chkBoxFighterPacDecompress.TabIndex = 7;
            chkBoxFighterPacDecompress.Text = "Automatically decompress PAC files";
            chkBoxFighterPacDecompress.UseVisualStyleBackColor = true;
            chkBoxFighterPacDecompress.CheckedChanged += chkBoxFighterPacDecompress_CheckedChanged;
            // 
            // chkBoxFighterPcsCompress
            // 
            chkBoxFighterPcsCompress.AutoSize = true;
            chkBoxFighterPcsCompress.Location = new System.Drawing.Point(10, 45);
            chkBoxFighterPcsCompress.Name = "chkBoxFighterPcsCompress";
            chkBoxFighterPcsCompress.Size = new System.Drawing.Size(181, 17);
            chkBoxFighterPcsCompress.TabIndex = 9;
            chkBoxFighterPcsCompress.Text = "Automatically compress PCS files";
            chkBoxFighterPcsCompress.UseVisualStyleBackColor = true;
            chkBoxFighterPcsCompress.CheckedChanged += chkBoxFighterPcsCompress_CheckedChanged;
            // 
            // tabFileAssociations
            // 
            tabFileAssociations.Controls.Add(genericFileAssociationBox);
            tabFileAssociations.Controls.Add(lblAdminApproval);
            tabFileAssociations.Controls.Add(btnApply);
            tabFileAssociations.Controls.Add(associatiedFilesBox);
            tabFileAssociations.Location = new System.Drawing.Point(4, 22);
            tabFileAssociations.Name = "tabFileAssociations";
            tabFileAssociations.Size = new System.Drawing.Size(318, 401);
            tabFileAssociations.TabIndex = 2;
            tabFileAssociations.Text = "File Associations";
            // 
            // genericFileAssociationBox
            // 
            genericFileAssociationBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                                                                   | AnchorStyles.Right;
            genericFileAssociationBox.Controls.Add(binFileAssociation);
            genericFileAssociationBox.Controls.Add(datFileAssociation);
            genericFileAssociationBox.Location = new System.Drawing.Point(8, 290);
            genericFileAssociationBox.Name = "genericFileAssociationBox";
            genericFileAssociationBox.Size = new System.Drawing.Size(302, 75);
            genericFileAssociationBox.TabIndex = 6;
            genericFileAssociationBox.TabStop = false;
            genericFileAssociationBox.Text = "Generic File Types";
            // 
            // binFileAssociation
            // 
            binFileAssociation.AutoSize = true;
            binFileAssociation.Location = new System.Drawing.Point(10, 45);
            binFileAssociation.Name = "binFileAssociation";
            binFileAssociation.Size = new System.Drawing.Size(135, 17);
            binFileAssociation.TabIndex = 9;
            binFileAssociation.Text = "Associate with .bin files";
            binFileAssociation.UseVisualStyleBackColor = true;
            binFileAssociation.CheckedChanged += BinFileAssociation_CheckedChanged;
            // 
            // datFileAssociation
            // 
            datFileAssociation.AutoSize = true;
            datFileAssociation.Location = new System.Drawing.Point(10, 22);
            datFileAssociation.Name = "datFileAssociation";
            datFileAssociation.Size = new System.Drawing.Size(136, 17);
            datFileAssociation.TabIndex = 8;
            datFileAssociation.Text = "Associate with .dat files";
            datFileAssociation.UseVisualStyleBackColor = true;
            datFileAssociation.CheckedChanged += DatFileAssociation_CheckedChanged;
            // 
            // lblAdminApproval
            // 
            lblAdminApproval.Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                                                          | AnchorStyles.Right;
            lblAdminApproval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblAdminApproval.ForeColor = System.Drawing.Color.Red;
            lblAdminApproval.Location = new System.Drawing.Point(3, 375);
            lblAdminApproval.Name = "lblAdminApproval";
            lblAdminApproval.Size = new System.Drawing.Size(312, 18);
            lblAdminApproval.TabIndex = 5;
            lblAdminApproval.Text = "Administrator access required to make changes";
            lblAdminApproval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApply
            // 
            btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnApply.Location = new System.Drawing.Point(240, 373);
            btnApply.Name = "btnApply";
            btnApply.Size = new System.Drawing.Size(75, 23);
            btnApply.TabIndex = 4;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // associatiedFilesBox
            // 
            associatiedFilesBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                          | AnchorStyles.Left
                                                          | AnchorStyles.Right;
            associatiedFilesBox.Controls.Add(checkBox1);
            associatiedFilesBox.Controls.Add(listView1);
            associatiedFilesBox.Location = new System.Drawing.Point(8, 6);
            associatiedFilesBox.Name = "associatiedFilesBox";
            associatiedFilesBox.Size = new System.Drawing.Size(302, 278);
            associatiedFilesBox.TabIndex = 1;
            associatiedFilesBox.TabStop = false;
            associatiedFilesBox.Text = "Wii File Types";
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            checkBox1.Location = new System.Drawing.Point(195, 252);
            checkBox1.Name = "checkBox1";
            checkBox1.RightToLeft = RightToLeft.Yes;
            checkBox1.Size = new System.Drawing.Size(104, 20);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Check All";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                | AnchorStyles.Left
                                                | AnchorStyles.Right;
            listView1.AutoArrange = false;
            listView1.BorderStyle = BorderStyle.FixedSingle;
            listView1.CheckBoxes = true;
            listView1.Columns.AddRange(new ColumnHeader[]
            {
                columnHeader1
            });
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.HideSelection = false;
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
            listView1.Items.AddRange(new ListViewItem[]
            {
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
                listViewItem29
            });
            listView1.Location = new System.Drawing.Point(6, 19);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(290, 227);
            listView1.TabIndex = 6;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.ItemChecked += new ItemCheckedEventHandler(listView1_ItemChecked);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 300;
            // 
            // tabDiscord
            // 
            tabDiscord.BackColor = System.Drawing.SystemColors.Control;
            tabDiscord.Controls.Add(grpBoxDiscordRPC);
            tabDiscord.Location = new System.Drawing.Point(4, 22);
            tabDiscord.Name = "tabDiscord";
            tabDiscord.Padding = new Padding(3);
            tabDiscord.Size = new System.Drawing.Size(318, 401);
            tabDiscord.TabIndex = 4;
            tabDiscord.Text = "Discord";
            // 
            // grpBoxDiscordRPC
            // 
            grpBoxDiscordRPC.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                       | AnchorStyles.Right;
            grpBoxDiscordRPC.Controls.Add(chkBoxEnableDiscordRPC);
            grpBoxDiscordRPC.Controls.Add(grpBoxDiscordRPCType);
            grpBoxDiscordRPC.Location = new System.Drawing.Point(8, 6);
            grpBoxDiscordRPC.Name = "grpBoxDiscordRPC";
            grpBoxDiscordRPC.Size = new System.Drawing.Size(302, 172);
            grpBoxDiscordRPC.TabIndex = 0;
            grpBoxDiscordRPC.TabStop = false;
            grpBoxDiscordRPC.Text = "Rich Presence";
            // 
            // chkBoxEnableDiscordRPC
            // 
            chkBoxEnableDiscordRPC.AutoSize = true;
            chkBoxEnableDiscordRPC.Location = new System.Drawing.Point(10, 22);
            chkBoxEnableDiscordRPC.Name = "chkBoxEnableDiscordRPC";
            chkBoxEnableDiscordRPC.Size = new System.Drawing.Size(171, 17);
            chkBoxEnableDiscordRPC.TabIndex = 1;
            chkBoxEnableDiscordRPC.Text = "Enable Discord Rich Presence";
            chkBoxEnableDiscordRPC.UseVisualStyleBackColor = true;
            chkBoxEnableDiscordRPC.CheckedChanged += ChkBoxEnableDiscordRPC_CheckedChanged;
            // 
            // grpBoxDiscordRPCType
            // 
            grpBoxDiscordRPCType.Controls.Add(DiscordRPCCustomName);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameCustom);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameExternal);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameInternal);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameDisabled);
            grpBoxDiscordRPCType.Location = new System.Drawing.Point(6, 45);
            grpBoxDiscordRPCType.Name = "grpBoxDiscordRPCType";
            grpBoxDiscordRPCType.Size = new System.Drawing.Size(290, 119);
            grpBoxDiscordRPCType.TabIndex = 0;
            grpBoxDiscordRPCType.TabStop = false;
            grpBoxDiscordRPCType.Text = "Mod Name Detection";
            // 
            // DiscordRPCCustomName
            // 
            DiscordRPCCustomName.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                           | AnchorStyles.Right;
            DiscordRPCCustomName.Location = new System.Drawing.Point(30, 88);
            DiscordRPCCustomName.Name = "DiscordRPCCustomName";
            DiscordRPCCustomName.Size = new System.Drawing.Size(254, 20);
            DiscordRPCCustomName.TabIndex = 2;
            DiscordRPCCustomName.Text = "My Mod";
            DiscordRPCCustomName.TextChanged += DiscordRPCCustomName_TextChanged;
            // 
            // rdoDiscordRPCNameCustom
            // 
            rdoDiscordRPCNameCustom.AutoSize = true;
            rdoDiscordRPCNameCustom.Location = new System.Drawing.Point(10, 91);
            rdoDiscordRPCNameCustom.Name = "rdoDiscordRPCNameCustom";
            rdoDiscordRPCNameCustom.Size = new System.Drawing.Size(14, 13);
            rdoDiscordRPCNameCustom.TabIndex = 3;
            rdoDiscordRPCNameCustom.TabStop = true;
            rdoDiscordRPCNameCustom.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameCustom.CheckedChanged += DiscordRPCNameSettings_CheckedChanged;
            // 
            // rdoDiscordRPCNameExternal
            // 
            rdoDiscordRPCNameExternal.AutoSize = true;
            rdoDiscordRPCNameExternal.Location = new System.Drawing.Point(10, 68);
            rdoDiscordRPCNameExternal.Name = "rdoDiscordRPCNameExternal";
            rdoDiscordRPCNameExternal.Size = new System.Drawing.Size(126, 17);
            rdoDiscordRPCNameExternal.TabIndex = 2;
            rdoDiscordRPCNameExternal.TabStop = true;
            rdoDiscordRPCNameExternal.Text = "Use external filename";
            rdoDiscordRPCNameExternal.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameExternal.CheckedChanged += DiscordRPCNameSettings_CheckedChanged;
            // 
            // rdoDiscordRPCNameInternal
            // 
            rdoDiscordRPCNameInternal.AutoSize = true;
            rdoDiscordRPCNameInternal.Location = new System.Drawing.Point(10, 45);
            rdoDiscordRPCNameInternal.Name = "rdoDiscordRPCNameInternal";
            rdoDiscordRPCNameInternal.Size = new System.Drawing.Size(123, 17);
            rdoDiscordRPCNameInternal.TabIndex = 1;
            rdoDiscordRPCNameInternal.TabStop = true;
            rdoDiscordRPCNameInternal.Text = "Use internal filename";
            rdoDiscordRPCNameInternal.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameInternal.CheckedChanged += DiscordRPCNameSettings_CheckedChanged;
            // 
            // rdoDiscordRPCNameDisabled
            // 
            rdoDiscordRPCNameDisabled.AutoSize = true;
            rdoDiscordRPCNameDisabled.Location = new System.Drawing.Point(10, 22);
            rdoDiscordRPCNameDisabled.Name = "rdoDiscordRPCNameDisabled";
            rdoDiscordRPCNameDisabled.Size = new System.Drawing.Size(66, 17);
            rdoDiscordRPCNameDisabled.TabIndex = 0;
            rdoDiscordRPCNameDisabled.TabStop = true;
            rdoDiscordRPCNameDisabled.Text = "Disabled";
            rdoDiscordRPCNameDisabled.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameDisabled.CheckedChanged += DiscordRPCNameSettings_CheckedChanged;
            // 
            // tabUpdater
            // 
            tabUpdater.Controls.Add(grpBoxCanary);
            tabUpdater.Controls.Add(updaterBehaviorGroupbox);
            tabUpdater.Location = new System.Drawing.Point(4, 22);
            tabUpdater.Name = "tabUpdater";
            tabUpdater.Padding = new Padding(3);
            tabUpdater.Size = new System.Drawing.Size(318, 401);
            tabUpdater.TabIndex = 1;
            tabUpdater.Text = "Updater";
            // 
            // grpBoxCanary
            // 
            grpBoxCanary.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            grpBoxCanary.Controls.Add(chkCanary);
            grpBoxCanary.Location = new System.Drawing.Point(8, 132);
            grpBoxCanary.Name = "grpBoxCanary";
            grpBoxCanary.Size = new System.Drawing.Size(302, 53);
            grpBoxCanary.TabIndex = 15;
            grpBoxCanary.TabStop = false;
            grpBoxCanary.Text = "BrawlCrate Canary";
            // 
            // updaterBehaviorGroupbox
            // 
            updaterBehaviorGroupbox.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                              | AnchorStyles.Right;
            updaterBehaviorGroupbox.Controls.Add(rdoAutoUpdate);
            updaterBehaviorGroupbox.Controls.Add(rdoCheckManual);
            updaterBehaviorGroupbox.Controls.Add(rdoCheckStartup);
            updaterBehaviorGroupbox.Controls.Add(chkDocUpdates);
            updaterBehaviorGroupbox.Location = new System.Drawing.Point(8, 6);
            updaterBehaviorGroupbox.Name = "updaterBehaviorGroupbox";
            updaterBehaviorGroupbox.Size = new System.Drawing.Size(302, 120);
            updaterBehaviorGroupbox.TabIndex = 14;
            updaterBehaviorGroupbox.TabStop = false;
            updaterBehaviorGroupbox.Text = "Updater Behavior";
            updaterBehaviorGroupbox.Enter += groupBox2_Enter;
            // 
            // rdoAutoUpdate
            // 
            rdoAutoUpdate.AutoSize = true;
            rdoAutoUpdate.Location = new System.Drawing.Point(10, 22);
            rdoAutoUpdate.Name = "rdoAutoUpdate";
            rdoAutoUpdate.Size = new System.Drawing.Size(72, 17);
            rdoAutoUpdate.TabIndex = 2;
            rdoAutoUpdate.TabStop = true;
            rdoAutoUpdate.Text = "Automatic";
            rdoAutoUpdate.UseVisualStyleBackColor = true;
            rdoAutoUpdate.CheckedChanged += updaterBehavior_CheckedChanged;
            // 
            // rdoCheckManual
            // 
            rdoCheckManual.AutoSize = true;
            rdoCheckManual.Location = new System.Drawing.Point(10, 68);
            rdoCheckManual.Name = "rdoCheckManual";
            rdoCheckManual.Size = new System.Drawing.Size(60, 17);
            rdoCheckManual.TabIndex = 1;
            rdoCheckManual.TabStop = true;
            rdoCheckManual.Text = "Manual";
            rdoCheckManual.UseVisualStyleBackColor = true;
            // 
            // rdoCheckStartup
            // 
            rdoCheckStartup.AutoSize = true;
            rdoCheckStartup.Location = new System.Drawing.Point(10, 45);
            rdoCheckStartup.Name = "rdoCheckStartup";
            rdoCheckStartup.Size = new System.Drawing.Size(220, 17);
            rdoCheckStartup.TabIndex = 0;
            rdoCheckStartup.TabStop = true;
            rdoCheckStartup.Text = "Manual, but check for updates on startup";
            rdoCheckStartup.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            ClientSize = new System.Drawing.Size(326, 427);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Icon = (System.Drawing.Icon) resources.GetObject("$this.Icon");
            Name = "SettingsDialog";
            Text = "Settings";
            Load += SettingsDialog_Load;
            Shown += SettingsDialog_Shown;
            tabControl1.ResumeLayout(false);
            tabGeneral.ResumeLayout(false);
            grpBoxMDL0General.ResumeLayout(false);
            grpBoxMDL0General.PerformLayout();
            grpBoxAudioGeneral.ResumeLayout(false);
            grpBoxAudioGeneral.PerformLayout();
            grpBoxMainFormGeneral.ResumeLayout(false);
            grpBoxMainFormGeneral.PerformLayout();
            grpBoxFileNameDisplayGeneral.ResumeLayout(false);
            grpBoxFileNameDisplayGeneral.PerformLayout();
            tabCompression.ResumeLayout(false);
            groupBoxModuleCompression.ResumeLayout(false);
            groupBoxModuleCompression.PerformLayout();
            groupBoxStageCompression.ResumeLayout(false);
            groupBoxStageCompression.PerformLayout();
            groupBoxFighterCompression.ResumeLayout(false);
            groupBoxFighterCompression.PerformLayout();
            tabFileAssociations.ResumeLayout(false);
            genericFileAssociationBox.ResumeLayout(false);
            genericFileAssociationBox.PerformLayout();
            associatiedFilesBox.ResumeLayout(false);
            tabDiscord.ResumeLayout(false);
            grpBoxDiscordRPC.ResumeLayout(false);
            grpBoxDiscordRPC.PerformLayout();
            grpBoxDiscordRPCType.ResumeLayout(false);
            grpBoxDiscordRPCType.PerformLayout();
            tabUpdater.ResumeLayout(false);
            grpBoxCanary.ResumeLayout(false);
            grpBoxCanary.PerformLayout();
            updaterBehaviorGroupbox.ResumeLayout(false);
            updaterBehaviorGroupbox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                bool check = checkBox1.Checked;
                foreach (ListViewItem i in listView1.Items)
                {
                    i.Checked = check;
                }
            }
        }

        private void chkShowPropDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.DisplayPropertyDescriptionsWhenAvailable = chkShowPropDesc.Checked;
            }
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
        }

        private void chkShowHex_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.ShowHex = chkShowHex.Checked;
            }
        }

        private void chkDocUpdates_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.GetDocumentationUpdates = chkDocUpdates.Checked;
            }
        }

        private void chkCanary_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;
            if (!MainForm.Instance.Canary)
            {
                DialogResult dc = MessageBox.Show(this,
                    "Are you sure you'd like to receive BrawlCrate canary updates? " +
                    "These updates will happen more often and include features as they are developed, but will come at the cost of stability. " +
                    "If you do take this track, it is highly recommended to join our discord server: https://discord.gg/s7c8763 \n\n" +
                    "If you select yes, the update will begin immediately, so make sure your work is saved.",
                    "BrawlCrate Canary Updater", MessageBoxButtons.YesNo);
                if (dc == DialogResult.Yes)
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
                                                        "If you select yes, the downgrade will begin immediately, so make sure your work is saved.",
                    "BrawlCrate Canary Updater", MessageBoxButtons.YesNo);
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
            if (!_updating)
            {
                MainForm.Instance.UpdateAutomatically = rdoAutoUpdate.Checked;
                MainForm.Instance.CheckUpdatesOnStartup = rdoAutoUpdate.Checked || rdoCheckStartup.Checked;
            }
        }

        private void btnCanaryBranch_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                    "Warning: Switching branches or repositories can be unstable unless you know what you're doing. You should generally stay on the brawlcrate-master branch unless directed otherwise for testing purposes. You can reset to the default for either field by leaving it blank.",
                    "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string cRepo = MainForm.currentRepo;
                string cBranch = MainForm.currentBranch;
                TwoInputStringDialog d = new TwoInputStringDialog();
                if (d.ShowDialog(this, "Enter new repo/branch to track", "Repo:", cRepo, "Branch:", cBranch) ==
                    DialogResult.OK)
                {
                    if (!d.InputText1.Equals(cRepo, StringComparison.OrdinalIgnoreCase) ||
                        !d.InputText2.Equals(cBranch, StringComparison.OrdinalIgnoreCase))
                    {
                        MainForm.SetCanaryTracking(d.InputText1, d.InputText2);
                    }
                }
            }
        }

        private void chkBoxAutoPlayAudio_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoPlayAudio = chkBoxAutoPlayAudio.Checked;
            }
        }

        private void chkBoxFighterPacDecompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoDecompressFighterPAC = chkBoxFighterPacDecompress.Checked;
            }
        }

        private void chkBoxFighterPcsCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoCompressPCS = chkBoxFighterPcsCompress.Checked;
            }
        }

        private void chkBoxStageCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoCompressStages = chkBoxStageCompress.Checked;
            }
        }

        private void chkBoxModuleCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                if (chkBoxModuleCompress.Checked)
                {
                    if (MessageBox.Show(
                            "Warning: Module compression does not save much space and can reduce editablity of modules. Are you sure you want to turn this on?",
                            "Module Compressor", MessageBoxButtons.YesNo) != DialogResult.Yes)
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
            {
                MainForm.Instance.CompatibilityMode = chkBoxMDL0Compatibility.Checked;
            }
        }

        private void ChkBoxEnableDiscordRPC_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                Properties.Settings.Default.DiscordRPCEnabled = chkBoxEnableDiscordRPC.Checked;
                Properties.Settings.Default.Save();
                grpBoxDiscordRPCType.Enabled = chkBoxEnableDiscordRPC.Checked;
                Discord.DiscordSettings.LoadSettings(true);
            }
        }

        private void DiscordRPCNameSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                if (rdoDiscordRPCNameDisabled.Checked)
                {
                    Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.Disabled;
                }
                else if (rdoDiscordRPCNameInternal.Checked)
                {
                    Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.AutoInternal;
                }
                else if (rdoDiscordRPCNameExternal.Checked)
                {
                    Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.AutoExternal;
                }
                else if (rdoDiscordRPCNameCustom.Checked)
                {
                    Properties.Settings.Default.DiscordRPCNameType = Discord.DiscordSettings.ModNameType.UserDefined;
                }

                DiscordRPCCustomName.Enabled = rdoDiscordRPCNameCustom.Checked;
                DiscordRPCCustomName.ReadOnly = !rdoDiscordRPCNameCustom.Checked;
                Properties.Settings.Default.Save();
                Discord.DiscordSettings.LoadSettings(true);
            }
        }

        private void DiscordRPCCustomName_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DiscordRPCNameCustom = DiscordRPCCustomName.Text;
            Properties.Settings.Default.Save();
            if (rdoDiscordRPCNameCustom.Checked)
            {
                Discord.DiscordSettings.LoadSettings(true);
            }
        }

        private void DatFileAssociation_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnApply.Enabled = true;
        }

        private void BinFileAssociation_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnApply.Enabled = true;
        }

        private void RdoPathDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            MainForm.Instance.ShowFullPath = rdoShowFullPath.Checked;
        }

        private void RecentFileCountBox_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (int.TryParse(recentFileCountBox.Text, out int i))
            {
                Properties.Settings.Default.RecentFilesMax = i;
                Properties.Settings.Default.Save();
            }
        }
    }
}