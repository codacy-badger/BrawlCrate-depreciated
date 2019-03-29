﻿using System;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using System.BrawlEx;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSTCGroup)]
    class RSTCGroupWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static RSTCGroupWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.AddNewEntry, null, NewEntryAction, Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.ClearList, null, ClearAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<RSTCGroupWrapper>().NewEntry(); }
        protected static void ClearAction(object sender, EventArgs e) { GetInstance<RSTCGroupWrapper>().Clear(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[2].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSTCGroupWrapper w = GetInstance<RSTCGroupWrapper>();
            _menu.Items[0].Enabled = (w._resource.Children.Count < 100);
            _menu.Items[2].Enabled = w._resource.HasChildren;
        }
        #endregion

        //public override string ExportFilter { get { return FileFilters.RSTCGroup; } }

        public void NewEntry()
        {
            if (_resource.Children.Count >= 100)
            {
                return;
            }
            RSTCEntryNode node = new RSTCEntryNode();
            node.FighterID = 0x0;
            node._name = "Mario";
            _resource.AddChild(node);
            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
        }

        public void Clear()
        {
            while (_resource.HasChildren)
                _resource.RemoveChild(_resource.Children[0]);
            BaseWrapper w = this.FindResource(_resource, false);
            w.EnsureVisible();
            w.Expand();
        }

        public RSTCGroupWrapper() { ContextMenuStrip = _menu; }
    }
}