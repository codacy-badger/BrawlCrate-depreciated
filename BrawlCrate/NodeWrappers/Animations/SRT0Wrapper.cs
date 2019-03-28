using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.Wii.Animations;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SRT0)]
    class SRT0Wrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static SRT0Wrapper()
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
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<SRT0Wrapper>().NewEntry(); }
        protected static void MergeAction(object sender, EventArgs e) { GetInstance<SRT0Wrapper>().Merge(); }
        protected static void AppendAction(object sender, EventArgs e) { GetInstance<SRT0Wrapper>().Append(); }
        protected static void ResizeAction(object sender, EventArgs e) { GetInstance<SRT0Wrapper>().Resize(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[13].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            SRT0Wrapper w = GetInstance<SRT0Wrapper>();
            _menu.Items[5].Enabled = _menu.Items[7].Enabled = _menu.Items[13].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[9].Enabled = w.PrevNode != null;
            _menu.Items[10].Enabled = w.NextNode != null;
        }
        #endregion

        public SRT0Wrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.SRT0; } }

        private void NewEntry()
        {
            ((SRT0Node)_resource).CreateEntry();
        }
        private void Merge()
        {

        }
        private void Append()
        {
            ((SRT0Node)_resource).Append();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
        private void Resize()
        {
            ((SRT0Node)_resource).Resize();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.SRT0Entry)]
    class SRT0EntryWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static SRT0EntryWrapper()
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
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<SRT0EntryWrapper>().NewEntry(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            SRT0EntryWrapper w = GetInstance<SRT0EntryWrapper>();
            _menu.Items[0].Enabled = w._resource.Children.Count < 11;
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }
        #endregion

        public SRT0EntryWrapper() { ContextMenuStrip = _menu; }

        public void NewEntry()
        {
            SRT0EntryNode n = (SRT0EntryNode)_resource;
            n.CreateEntry();
        }
    }

    [NodeWrapper(ResourceType.SRT0Texture)]
    class SRT0TextureWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static SRT0TextureWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.ViewInterpolation, null, ViewInterp, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Restore, null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ViewInterp(object sender, EventArgs e) { GetInstance<SRT0TextureWrapper>().ViewInterp(); }
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
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            SRT0TextureWrapper w = GetInstance<SRT0TextureWrapper>();
            _menu.Items[3].Enabled = _menu.Items[6].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
        }
        #endregion

        public SRT0TextureWrapper() { ContextMenuStrip = _menu; }
    }
}
