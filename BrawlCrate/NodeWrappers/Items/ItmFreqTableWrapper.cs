using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.ItemFreqTableNode)]
    class ItmFreqTableWrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;
        static ItmFreqTableWrapper()
        {
            _menu = new ContextMenuStrip();
            //_menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.NewGroup, null, NewEntryAction, Keys.Control | Keys.H));
            //_menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<ItmFreqTableWrapper>().NewEntry();
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {

        }
        public ItmFreqTableWrapper() { ContextMenuStrip = _menu; }
        public TableGroupNode NewEntry()
        {
            int childCount = _resource.Children == null ? 0 : _resource.Children.Count;
            TableGroupNode node = new TableGroupNode() { Name = ("Group [" + childCount + "]") };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
    }
}