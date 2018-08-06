using System;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using System.BrawlEx;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.RSTC)]
    class RSTCWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static RSTCWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Entry (Both Lists)", null, NewEntryAction, Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Clear Lists", null, ClearAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewEntryAction(object sender, EventArgs e)
        {
            RSTCWrapper w = GetInstance<RSTCWrapper>();
            ResourceNode r = w._resource;
            if (((RSTCNode)r).cssList.Children.Count >= 100 || (((RSTCNode)r).randList.Children.Count >= 100))
                return;
            StageBoxHexEntry entryID = new StageBoxHexEntry();
            if (entryID.ShowDialog("New Fighter", "CSS Slot ID:") == DialogResult.OK)
                if(entryID.NewValue != -1)
                    GetInstance<RSTCWrapper>().NewEntry((byte)entryID.NewValue);
        }
        protected static void ClearAction(object sender, EventArgs e) { GetInstance<RSTCWrapper>().Clear(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[8].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSTCWrapper w = GetInstance<RSTCWrapper>();
            ResourceNode r = w._resource;
            _menu.Items[0].Enabled = (((RSTCNode)r).cssList.Children.Count <= 100 || (((RSTCNode)r).randList.Children.Count <= 100));
            _menu.Items[3].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[8].Enabled = (((RSTCNode)r).cssList.Children.Count > 0) || (((RSTCNode)r).randList.Children.Count > 0);
        }
        #endregion

        public override string ExportFilter { get { return FileFilters.RSTC; } }

        public void NewEntry(byte cssID)
        {
            if (((RSTCNode)_resource).cssList.Children.Count >= 100 || (((RSTCNode)_resource).randList.Children.Count >= 100))
            {
                return;
            }
            RSTCEntryNode node1 = new RSTCEntryNode();
            node1.FighterID = cssID;
            node1._name = CSSSlotIDs.getNameFromID(cssID);
            if (node1._name == null)
                node1._name = "ID 0x" + cssID.ToString("X2");
            ((RSTCNode)_resource).cssList.AddChild(node1);
            RSTCEntryNode node2 = new RSTCEntryNode();
            node2.FighterID = cssID;
            node2._name = CSSSlotIDs.getNameFromID(cssID);
            if (node2._name == null)
                node2._name = "ID 0x" + cssID.ToString("X2");
            ((RSTCNode)_resource).randList.AddChild(node2);
        }

        public void Clear()
        {
            while (((RSTCNode)_resource).cssList.HasChildren)
                ((RSTCNode)_resource).cssList.RemoveChild(((RSTCNode)_resource).cssList.Children[0]);
            while (((RSTCNode)_resource).randList.HasChildren)
                ((RSTCNode)_resource).randList.RemoveChild(((RSTCNode)_resource).randList.Children[0]);
            BaseWrapper w = this.FindResource(((RSTCNode)_resource).cssList, false);
            w.EnsureVisible();
            w.Expand();
            w = this.FindResource(((RSTCNode)_resource).randList, false);
            w.EnsureVisible();
            w.Expand();
        }

        public RSTCWrapper() { ContextMenuStrip = _menu; }
    }
}