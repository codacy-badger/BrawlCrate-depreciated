using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.PAT0)]
    class PAT0Wrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static PAT0Wrapper()
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
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<PAT0Wrapper>().NewEntry(); }
        protected static void MergeAction(object sender, EventArgs e) { GetInstance<PAT0Wrapper>().Merge(); }
        protected static void AppendAction(object sender, EventArgs e) { GetInstance<PAT0Wrapper>().Append(); }
        protected static void ResizeAction(object sender, EventArgs e) { GetInstance<PAT0Wrapper>().Resize(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[13].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            PAT0Wrapper w = GetInstance<PAT0Wrapper>();
            _menu.Items[5].Enabled = _menu.Items[7].Enabled = _menu.Items[13].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[9].Enabled = w.PrevNode != null;
            _menu.Items[10].Enabled = w.NextNode != null;
        }
        #endregion

        public PAT0Wrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.PAT0; } }

        public void NewEntry() { ((PAT0Node)_resource).CreateEntry(); }
        private void Merge()
        {

        }
        private void Append()
        {
            ((PAT0Node)_resource).Append();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
        private void Resize()
        {
            ((PAT0Node)_resource).Resize();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.PAT0Entry)]
    class PAT0EntryWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static PAT0EntryWrapper()
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
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<PAT0EntryWrapper>().NewEntry(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            PAT0EntryWrapper w = GetInstance<PAT0EntryWrapper>();
            _menu.Items[0].Enabled = w._resource.Children.Count < 8;
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }
        #endregion

        public PAT0EntryWrapper() { ContextMenuStrip = _menu; }

        public void NewEntry()
        {
            PAT0EntryNode n = (PAT0EntryNode)_resource;
            n.CreateEntry();
        }
    }

    [NodeWrapper(ResourceType.PAT0Texture)]
    class PAT0TextureWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static PAT0TextureWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.NewEntry, null, NewEntryAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Restore, null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<PAT0TextureWrapper>().NewEntry(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            PAT0TextureWrapper w = GetInstance<PAT0TextureWrapper>();
            _menu.Items[3].Enabled = _menu.Items[6].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
        }
        #endregion

        public void NewEntry()
        {
            PAT0TextureNode n = (PAT0TextureNode)_resource;
            n.CreateEntry();
        }

        public PAT0TextureWrapper() { ContextMenuStrip = _menu; }
    }

    [NodeWrapper(ResourceType.PAT0TextureEntry)]
    class PAT0TextureEntryWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static PAT0TextureEntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.OffsetFrame, null, OffsetAction, Keys.Control | Keys.F));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Export, null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Replace, null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Restore, null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Rename, null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void OffsetAction(object sender, EventArgs e)
        {
            PAT0OffsetControl offsetControl = new PAT0OffsetControl();
            if (offsetControl.ShowDialog() == DialogResult.OK)
            {
                GetInstance<PAT0TextureEntryWrapper>().Offset(offsetControl.NewValue, offsetControl.IncreaseFrames, offsetControl.OffsetOtherTextures);
            }
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[8].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            PAT0TextureEntryWrapper w = GetInstance<PAT0TextureEntryWrapper>();
            _menu.Items[3].Enabled = _menu.Items[8].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
        }
        #endregion

        public PAT0TextureEntryWrapper() { ContextMenuStrip = _menu; }

        public void Offset(int offsetValue, bool offsetFrames, bool offsetOtherTextures)
        {
            float currentFrame = ((PAT0TextureEntryNode)_resource).FrameIndex;
            if (offsetFrames)
                ((PAT0Node)_resource.Parent.Parent.Parent).FrameCount += offsetValue;
            if (offsetOtherTextures)
            {
                OffsetAll(currentFrame, offsetValue);
                return;
            }
            foreach(PAT0TextureEntryNode pte in _resource.Parent.Children)
                if (pte._frame >= currentFrame)
                    pte._frame += offsetValue;
        }

        public void OffsetAll(float currentFrame, int offsetValue)
        {
            // Go through every entry in the PAT0 and edit it as necessary
            foreach(PAT0EntryNode pe in _resource.Parent.Parent.Parent.Children)
                foreach(PAT0TextureNode pt in pe.Children)
                    foreach(PAT0TextureEntryNode pte in pt.Children)
                        if (pte._frame >= currentFrame)
                            pte._frame += offsetValue;
        }
    }
}
