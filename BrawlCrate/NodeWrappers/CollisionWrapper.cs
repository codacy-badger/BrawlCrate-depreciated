﻿using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib;

namespace BrawlCrate
{
    [NodeWrapper(ResourceType.CollisionDef)]
    class CollisionWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static CollisionWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Preview / Edit", null, EditAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripMenuItem("&Advanced Editor", null, AdvancedEditAction, Keys.Control | Keys.Shift | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Merge", null, MergeAction, Keys.Control | Keys.M));
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
            _menu.Items.Add(new ToolStripMenuItem("&Mirror Unbound Collisions", null,
                new ToolStripMenuItem("X-Axis", null, FlipXAction),
                new ToolStripMenuItem("Y-Axis", null, FlipYAction)
                ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem(BrawlLib.Properties.Resources.Delete, null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        // StageBox collision flipping
        private static void FlipXAction(object sender, EventArgs e) { GetInstance<CollisionWrapper>().FlipX(); }
        private static void FlipYAction(object sender, EventArgs e) { GetInstance<CollisionWrapper>().FlipY(); }

        private static void MergeAction(object sender, EventArgs e) { GetInstance<CollisionWrapper>().Merge(); }

        protected static void EditAction(object sender, EventArgs e) { GetInstance<CollisionWrapper>().Preview(); }
        protected static void AdvancedEditAction(object sender, EventArgs e) { GetInstance<CollisionWrapper>().AdvancedEdit(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[8].Enabled = _menu.Items[10].Enabled = _menu.Items[11].Enabled = _menu.Items[16].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            CollisionWrapper w = GetInstance<CollisionWrapper>();
            _menu.Items[6].Enabled = _menu.Items[8].Enabled = _menu.Items[16].Enabled = w.Parent != null;
            _menu.Items[7].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[10].Enabled = w.PrevNode != null;
            _menu.Items[11].Enabled = w.NextNode != null;
        }
        #endregion

        public override string ExportFilter { get { return FileFilters.CollisionDef; } }

        public CollisionWrapper() { ContextMenuStrip = _menu; }

        public override ResourceNode Duplicate()
        {
            if(_resource._parent == null)
            {
                return null;
            }
            _resource.Rebuild();
            CollisionNode newNode = NodeFactory.FromAddress(null, _resource.WorkingUncompressed.Address, _resource.WorkingUncompressed.Length) as CollisionNode;
            int newIndex = _resource.Index + 1;
            _resource._parent.InsertChild(newNode, true, newIndex);
            newNode.Populate();
            newNode.FileType = ((CollisionNode)_resource).FileType;
            newNode.FileIndex = ((CollisionNode)_resource).FileIndex;
            newNode.RedirectIndex = ((CollisionNode)_resource).RedirectIndex;
            newNode.GroupID = ((CollisionNode)_resource).GroupID;
            return newNode;
        }

        public void Merge()
        {
            ((CollisionNode)_resource).MergeWith();
        }

        public void FlipX()
        {
            CollisionNode coll = ((CollisionNode)_resource);
            //int i = 0;
            //int j = 0;
            foreach(CollisionObject cObj in coll._objects)
            {
                //++i;
                //Console.WriteLine("COLLISION OBJECT: " + i);
                if(cObj.LinkedBone == null)
                {
                    //j = 0;
                    //Console.WriteLine("   Not linked to a model");
                    cObj.resetFlip();
                    foreach (CollisionPlane p in cObj._planes)
                    {
                        //++j;
                        //Console.WriteLine("      Initiating Plane: " + j);
                        p.flipAcrossPlane('X');
                    }
                }
            }
            coll.SignalPropertyChange();
        }

        public void FlipY()
        {
            CollisionNode coll = ((CollisionNode)_resource);
            //int i = 0;
            //int j = 0;
            foreach (CollisionObject cObj in coll._objects)
            {
                //++i;
                //Console.WriteLine("COLLISION OBJECT: " + i);
                if (cObj.LinkedBone == null)
                {
                    //j = 0;
                    //Console.WriteLine("   Not linked to a model");
                    cObj.resetFlip();
                    foreach (CollisionPlane p in cObj._planes)
                    {
                        //++j;
                        //Console.WriteLine("      Initiating Plane: " + j);
                        p.flipAcrossPlane('Y');
                    }
                }
            }
            coll.SignalPropertyChange();
        }

        private void Preview()
        {
            using (CollisionForm frm = new CollisionForm())
                frm.ShowDialog(null, _resource as CollisionNode);
        }

        private void AdvancedEdit()
        {
            DialogResult CollisionResult = MessageBox.Show("Please note: The advanced collision editor is for experimental purposes only. Unless you really know what you're doing, the regular collision editor is overall better for the same purposes. Are you sure you'd like to open in the Advanced Editor?", "Open Advanced Editor", MessageBoxButtons.YesNo);
            if (CollisionResult == DialogResult.Yes)
            {
                using (AdvancedCollisionForm frm = new AdvancedCollisionForm())
                    frm.ShowDialog(null, _resource as CollisionNode);
            }
        }
    }
}
