using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.Wii.Animations;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SHP0)]
    class SHP0Wrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static SHP0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.NewEntry, null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Edit, null,
                //new ToolStripMenuItem(BrawlLib.Properties.Resources.MergeAnimation, null, MergeAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.AppendAnimation, null, AppendAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.SortEntries, null, SortAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.Resize, null, ResizeAction)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Restore, null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Duplicate, null, DuplicateAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveUp, null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.MoveDown, null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Rename, null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing; 
        }
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<SHP0Wrapper>().NewEntry(); }
        protected static void MergeAction(object sender, EventArgs e) { GetInstance<SHP0Wrapper>().Merge(); }
        protected static void AppendAction(object sender, EventArgs e) { GetInstance<SHP0Wrapper>().Append(); }
        protected static void ResizeAction(object sender, EventArgs e) { GetInstance<SHP0Wrapper>().Resize(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[13].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            SHP0Wrapper w = GetInstance<SHP0Wrapper>();
            _menu.Items[5].Enabled = _menu.Items[7].Enabled = _menu.Items[13].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[9].Enabled = w.PrevNode != null;
            _menu.Items[10].Enabled = w.NextNode != null;
        }

        #endregion

        public SHP0Wrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.SHP0; } }

        public void NewEntry()
        {
            SHP0EntryNode node = ((SHP0Node)_resource).FindOrCreateEntry(_resource.FindName(null));
            BaseWrapper res = this.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
        private void Merge()
        {

        }
        private void Append()
        {
            ((SHP0Node)_resource).Append();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
        private void Resize()
        {
            ((SHP0Node)_resource).Resize();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.SHP0Entry)]
    class SHP0EntryWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static SHP0EntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.NewEntry, null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
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
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<SHP0EntryWrapper>().NewEntry(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            SHP0EntryWrapper w = GetInstance<SHP0EntryWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }
        #endregion

        public SHP0EntryWrapper() { ContextMenuStrip = _menu; }

        public void NewEntry()
        {
            SHP0EntryNode n = (SHP0EntryNode)_resource;
            n.CreateEntry();
        }
    }

    [NodeWrapper(ResourceType.SHP0VertexSet)]
    class SHP0VertexSetNodeWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static SHP0VertexSetNodeWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.ViewInterpolation, null, ViewInterp, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
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
        protected static void ViewInterp(object sender, EventArgs e) { GetInstance<SHP0VertexSetNodeWrapper>().ViewInterp(); }
        private void ViewInterp()
        {
            InterpolationForm f = MainForm.Instance.InterpolationForm;
            if (f != null)
            {
                InterpolationEditor e = f._interpolationEditor;
                if (e != null)
                    e.SetTarget(_resource as IKeyframeSource);
            }
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            SHP0VertexSetNodeWrapper w = GetInstance<SHP0VertexSetNodeWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion

        public SHP0VertexSetNodeWrapper(IWin32Window owner) { _owner = owner; ContextMenuStrip = _menu; }
        public SHP0VertexSetNodeWrapper() { _owner = null; ContextMenuStrip = _menu; }
    }
}
