using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.TEX0)]
    [NodeWrapper(ResourceType.SharedTEX0)]
    class TEX0Wrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;
        static TEX0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Re-Encode", null, ReEncodeAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteTEX0Action, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ReEncodeAction(object sender, EventArgs e) { GetInstance<TEX0Wrapper>().ReEncode(); }
        protected static void DeleteTEX0Action(object sender, EventArgs e) { GetInstance<TEX0Wrapper>().DeleteTEX0(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            TEX0Wrapper w = GetInstance<TEX0Wrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        public TEX0Wrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.TEX0; } }

        public override void OnReplace(string inStream, int filterIndex)
        {
            if (filterIndex == 8)
                base.OnReplace(inStream, filterIndex);
            else
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = inStream;
                    dlg.ShowDialog(MainForm.Instance, ResourceNode as TEX0Node);
                }
        }

        public void ReEncode()
        {
            using (TextureConverterDialog dlg = new TextureConverterDialog())
            {
                dlg.LoadImages((ResourceNode as TEX0Node).GetImage(0));
                dlg.ShowDialog(MainForm.Instance, ResourceNode as TEX0Node);
            }
        }

        public void DeleteTEX0()
        {
            if (Parent == null)
                return;

            if (_resource.Parent is BRESGroupNode && _resource.Parent.Parent != null && _resource.Parent.Parent is BRRESNode && ((BRRESNode)_resource.Parent.Parent).GetFolder<PLT0Node>() != null)
            {
                ResourceNode[] plt0 = ((BRRESNode)_resource.Parent.Parent).GetFolder<PLT0Node>().FindChildrenByName(_resource.Name);
                if(plt0.Length > 0)
                    if(MessageBox.Show("Would you like to delete the associated PLT0?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        for (int i = 0; i < plt0.Length; ++i)
                            ((BRRESNode)_resource.Parent.Parent).GetFolder<PLT0Node>().RemoveChild(plt0[i]);
            }

            _resource.Dispose();
            _resource.Remove();
        }

        internal protected override void OnPropertyChanged(ResourceNode node)
        {
            RefreshView(node);
        }
    }
}
