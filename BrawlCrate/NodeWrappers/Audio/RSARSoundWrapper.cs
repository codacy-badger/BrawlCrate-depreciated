﻿using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSARSound)]
    class RSARSoundWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.WAV; } }

        #region Menu

        private static ContextMenuStrip _menu;
        static RSARSoundWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.ChangeSound, null, ChangeSoundAction, Keys.Control | Keys.W));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.ViewFile, null, ViewFileAction, Keys.Control | Keys.I));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Restore, null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveUp, null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveDown, null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Rename, null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ChangeSoundAction(object sender, EventArgs e) { GetInstance<RSARSoundWrapper>().ChangeSound(); }
        protected static void ViewFileAction(object sender, EventArgs e) { GetInstance<RSARSoundWrapper>().ViewFile(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[1].Enabled = _menu.Items[3].Enabled = _menu.Items[5].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSARSoundWrapper w = GetInstance<RSARSoundWrapper>();
            RSARSoundNode n = w._resource as RSARSoundNode;
            _menu.Items[0].Enabled = n._waveDataNode != null;
            _menu.Items[1].Enabled = n.SoundFileNode != null;
            _menu.Items[3].Enabled = w.Parent != null;
            _menu.Items[4].Visible = false;
            _menu.Items[5].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[7].Enabled = w.PrevNode != null;
            _menu.Items[8].Enabled = w.NextNode != null;
        }

        #endregion

        public RSARSoundWrapper() { ContextMenuStrip = _menu; }

        public void ChangeSound()
        {
            RSARSoundNode n = _resource as RSARSoundNode;
            if (n._waveDataNode != null)
            {
                if (n._waveDataNode._refs.Count > 1)
                {
                    string s = BrawlLib.Properties.Resources.EntriesUseSound + '\n';
                    foreach (RSARSoundNode x in n._waveDataNode._refs)
                        s += x.TreePath + "\n";
                    s += '\n' + BrawlLib.Properties.Resources.ConfirmReplaceSound;
                    if (MessageBox.Show(s, BrawlLib.Properties.Resources.Continue, MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                string inPath;
                int index = Program.OpenFile(ReplaceFilter, out inPath);
                if (index != 0)
                {
                    n._waveDataNode.Sound.Replace(inPath);
                    MainForm.Instance.resourceTree_SelectionChanged(null, null);
                }
            }
        }
        public void ViewFile()
        {
            RSARFileNode n;
            if ((n = (_resource as RSARSoundNode).SoundFileNode) == null)
                return;
            if (n is RSARExtFileNode)
            {
                RSARExtFileNode ext = n as RSARExtFileNode;
                if (File.Exists(ext.FullExtPath)) 
                    Process.Start(ext.FullExtPath);
            }
            else
                new EditRSARFileDialog().ShowDialog(MainForm.Instance, n);
        }
    }
}
