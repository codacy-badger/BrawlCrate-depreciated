﻿using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSARFileSoundGroup)]
    class RWSDSoundGroupWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static RWSDSoundGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.ImportNewSound, null, CreateAction, Keys.Control | Keys.I));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Rename, null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveUp, null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveDown, null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void CreateAction(object sender, EventArgs e) { GetInstance<RWSDSoundGroupWrapper>().Import(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[6].Enabled = _menu.Items[7].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RWSDSoundGroupWrapper w = GetInstance<RWSDSoundGroupWrapper>();
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }
        private unsafe void Import()
        {
            string path;
            if (Program.OpenFile("PCM Audio (*.wav)|*.wav", out path) > 0)
            {
                RSARFileAudioNode n;

                if ((_resource.Parent as NW4RNode).VersionMinor >= 3)
                    n = new RWAVNode();
                else
                    n = new WAVESoundNode();

                _resource.AddChild(n);
                n.Replace(path);
                
                BaseWrapper res = this.FindResource(n, true);
                res.EnsureVisible();
                res.TreeView.SelectedNode = res;
            }
        }
        #endregion

        public RWSDSoundGroupWrapper() { ContextMenuStrip = _menu; }
    }
}
