﻿using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.Wii.Animations;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CHR0)]
    class CHR0Wrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static CHR0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.NewBoneTarget, null, NewBoneAction, Keys.Control | Keys.W));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Edit, null,
                new ToolStripMenuItem(BrawlLib.Properties.Resources.GenerateKeyframesAll, null, KeyframeGenAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.MergeAnimation, null, MergeAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.AppendAnimation, null, AppendAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.RepeatAnimation, null, LoopAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.ReverseAnimation, null, ReverseAction),
                new ToolStripMenuItem(BrawlLib.Properties.Resources.ReverseToLoop, null, ReverseLoopAction),
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
        protected static void NewBoneAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().NewBone(); }
        protected static void MergeAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Merge(); }
        protected static void AppendAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Append(); }
        protected static void ReverseAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Reverse(false); }
        protected static void ReverseLoopAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Reverse(true); }
        protected static void ResizeAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().Resize(); }
        protected static void LoopAction(object sender, EventArgs e) {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog(BrawlLib.Properties.Resources.AnimationLooper, BrawlLib.Properties.Resources.NumberRunthroughs) == DialogResult.OK)
                GetInstance<CHR0Wrapper>().Loop((entryCount.NewValue)-1);
        }

        protected static void KeyframeGenAction(object sender, EventArgs e) { GetInstance<CHR0Wrapper>().KeyFrameGen(2, false, false); }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[13].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            CHR0Wrapper w = GetInstance<CHR0Wrapper>();
            _menu.Items[5].Enabled = _menu.Items[7].Enabled = _menu.Items[13].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[9].Enabled = w.PrevNode != null;
            _menu.Items[10].Enabled = w.NextNode != null;
        }

        #endregion

        public CHR0Wrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.CHR0Export; } }
        public override string ImportFilter { get { return FileFilters.CHR0Import; } }

        public void NewBone()
        {
            CHR0EntryNode node = ((CHR0Node)_resource).CreateEntry();
            BaseWrapper res = this.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Merge()
        {
            ((CHR0Node)_resource).MergeWith();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Resize()
        {
            ((CHR0Node)_resource).Resize();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
        
        public void Append()
        {
            ((CHR0Node)_resource).Append();
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Loop(int timesToLoop)
        {
            CHR0Node temp = ((CHR0Node)_resource);
            ((CHR0Node)_resource).Append(temp, timesToLoop);
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void KeyFrameGen(int framesToGenerate, bool allowDuplicates, bool generateOrigin)
        {
            ((CHR0Node)_resource).KeyFrameGen(framesToGenerate, allowDuplicates, generateOrigin);
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void Reverse(bool appendReverse)
        {
            ((CHR0Node)_resource).Reverse(appendReverse);
            BaseWrapper res = this.FindResource(_resource, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.CHR0Entry)]
    class CHR0EntryWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static CHR0EntryWrapper()
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
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.GenerateKeyframe, null, KeyframeGenFromBoneAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ViewInterp(object sender, EventArgs e) { GetInstance<CHR0EntryWrapper>().ViewInterp(); }
        protected static void KeyframeGenFromBoneAction(object sender, EventArgs e) { GetInstance<CHR0EntryWrapper>().KeyFrameGenFromBone(2, true, true); }
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
            CHR0EntryWrapper w = GetInstance<CHR0EntryWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        public void KeyFrameGenFromBone(int framesToGenerate, bool allowOverwrite, bool generateOrigin)
        {
            ((CHR0EntryNode)_resource).KeyFrameGenFromBone(framesToGenerate, allowOverwrite, generateOrigin);
        }

        #endregion

        public CHR0EntryWrapper(IWin32Window owner) { _owner = owner; ContextMenuStrip = _menu; }
        public CHR0EntryWrapper() { _owner = null; ContextMenuStrip = _menu; }
    }
}
