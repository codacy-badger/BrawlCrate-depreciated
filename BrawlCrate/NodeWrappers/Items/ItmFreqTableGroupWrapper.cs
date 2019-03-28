using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.ItemFreqTableGroupNode)]
    class ItmFreqTableGroupWrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;
        static ItmFreqTableGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            //_menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.NewEntry, null, NewEntryAction, Keys.Control | Keys.H));
            //_menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<ItmFreqTableGroupWrapper>().NewEntry();
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {

        }
        public ItmFreqTableGroupWrapper() { ContextMenuStrip = _menu; }
        public ItmFreqEntryNode NewEntry()
        {
            ItmFreqEntryNode node = new ItmFreqEntryNode();
            node.UpdateName();
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
    }
}