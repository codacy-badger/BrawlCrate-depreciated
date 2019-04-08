using System;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using System.BrawlEx;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MASQ)]
    class MasqueradeWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static MasqueradeWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<MasqueradeWrapper>().NewEntry(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MasqueradeWrapper w = GetInstance<MasqueradeWrapper>();
            _menu.Items[0].Enabled = (w._resource.Children.Count < 50);
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }
        #endregion

        public override string ExportFilter { get { return FileFilters.MASQ; } }

        public void NewEntry()
        {
            if (_resource.Children.Count >= 50)
                return;
            MasqueradeEntryNode node = new MasqueradeEntryNode();
            node._colorID = 0x0B;
            if (_resource.HasChildren)
            {
                byte nextID = (byte)(((MasqueradeEntryNode)(_resource.Children[_resource.Children.Count - 1]))._costumeID + 1);
                if (((MasqueradeNode)_resource)._cosmeticSlot == 21 && (
                    nextID == 15 ||
                    nextID == 31 ||
                    nextID == 47 ||
                    nextID == 63))
                    ++nextID; // Prevent wario edge cases
                node._costumeID = nextID;
            }
            _resource.AddChild(node);
            node.regenName();
        }

        public MasqueradeWrapper() { ContextMenuStrip = _menu; }
    }
}