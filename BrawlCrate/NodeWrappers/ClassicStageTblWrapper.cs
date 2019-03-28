using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.ClassicStageTbl)]
    class ClassicStageTblWrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;
        static ClassicStageTblWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.NewEntry, null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Restore, null, RestoreAction, Keys.Control | Keys.T));
            //_menu.Items.Add(new ToolStripSeparator());
            //_menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveUp, null, MoveUpAction, Keys.Control | Keys.Up));
            //_menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveDown, null, MoveDownAction, Keys.Control | Keys.Down));
            //_menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Rename, null, RenameAction, Keys.Control | Keys.N));
            //_menu.Items.Add(new ToolStripSeparator());
            //_menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<ClassicStageTblWrapper>().NewEntry();
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {

        }
        public ClassicStageTblWrapper() { ContextMenuStrip = _menu; }
        public void NewEntry()
        {
            ResourceNode n = ((ClassicStageTblNode)_resource).CreateEntry();
            BaseWrapper w = this.FindResource(n, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
        }
    }
}