﻿using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib;
using BrawlLib.Modeling;

namespace BrawlCrate
{
    [NodeWrapper(ResourceType.ARC)]
    class ARCWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static ARCWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("ARChive", null, NewARCAction),
                new ToolStripMenuItem("BRResource Pack", null, NewBRESAction),
                new ToolStripMenuItem("BLOC", null, NewBLOCAction),
                new ToolStripMenuItem("Collision", null, NewCollisionAction),
                //new ToolStripMenuItem("Item Generation", null, NewItemGenerationAction),
                new ToolStripMenuItem("MSBin", null, NewMSBinAction),
                new ToolStripMenuItem("Redirect", null, NewRedirectAction),
                new ToolStripMenuItem("SCLA", null,
                    new ToolStripMenuItem("Empty", null, NewSCLAAction),
                    new ToolStripMenuItem("Full", null, NewSCLAFullAction),
                    new ToolStripMenuItem("Expanded", null, NewSCLAExpandedAction)
                    ),
                new ToolStripMenuItem("STDT", null, NewSTDTAction),
                new ToolStripMenuItem("STPM", null, NewSTPMAction),
                new ToolStripMenuItem("Stage Table", null,
                    new ToolStripMenuItem("TBCL", null, NewTBCLAction),
                    new ToolStripMenuItem("TBGC", null, NewTBGCAction),
                    new ToolStripMenuItem("TBGD", null, NewTBGDAction),
                    new ToolStripMenuItem("TBGM", null, NewTBGMAction),
                    new ToolStripMenuItem("TBLV", null, NewTBLVAction),
                    new ToolStripMenuItem("TBRM", null, NewTBRMAction),
                    new ToolStripMenuItem("TBST", null, NewTBSTAction)
                    )
                ));
            _menu.Items.Add(new ToolStripMenuItem("&Import", null,
                new ToolStripMenuItem("ARChive", null, ImportARCAction),
                new ToolStripMenuItem("BRResource Pack", null, ImportBRESAction),
                new ToolStripMenuItem("BLOC", null, ImportBLOCAction),
                new ToolStripMenuItem("Collision", null, ImportCollisionAction),
                new ToolStripMenuItem("MSBin", null, ImportMSBinAction),
                new ToolStripMenuItem("SCLA", null, ImportSCLAAction),
                new ToolStripMenuItem("STDT", null, ImportSTDTAction),
                new ToolStripMenuItem("STPM", null, ImportSTPMAction),
                new ToolStripMenuItem("Stage Table", null,
                    new ToolStripMenuItem("TBCL", null, ImportTBCLAction),
                    new ToolStripMenuItem("TBGC", null, ImportTBGCAction),
                    new ToolStripMenuItem("TBGD", null, ImportTBGDAction),
                    new ToolStripMenuItem("TBGM", null, ImportTBGMAction),
                    new ToolStripMenuItem("TBLV", null, ImportTBLVAction),
                    new ToolStripMenuItem("TBRM", null, ImportTBRMAction),
                    new ToolStripMenuItem("TBST", null, ImportTBSTAction)
                    ),
                new ToolStripMenuItem("Havok Data", null, ImportHavokAction)
                ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Preview All Models", null, PreviewAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewBRESAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewBRES(); }
        protected static void NewARCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewARC(); }
        protected static void NewMSBinAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewMSBin(); }
        protected static void NewCollisionAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewCollision(); }
        protected static void NewItemGenerationAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewItemGeneration(); }
        protected static void NewBLOCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewBLOC(); }
        protected static void NewSCLAAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewSCLA(0); }
        protected static void NewSCLAFullAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewSCLA(32); }
        protected static void NewSCLAExpandedAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewSCLA(256); }
        protected static void NewSTDTAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("STDT Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewSTDT(entryCount.NewValue);
        }
        protected static void NewSTPMAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewSTPM(); }
        protected static void NewTBCLAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("TBCL Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewTBCL(entryCount.NewValue);
        }
        protected static void NewTBGCAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("TBGC Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewTBGC(entryCount.NewValue);
        }
        protected static void NewTBGDAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("TBGD Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewTBGD(entryCount.NewValue);
        }
        protected static void NewTBGMAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("TBGM Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewTBGM(entryCount.NewValue);
        }
        protected static void NewTBLVAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("TBLV Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewTBLV(entryCount.NewValue);
        }
        protected static void NewTBRMAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("TBRM Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewTBRM(entryCount.NewValue);
        }
        protected static void NewTBSTAction(object sender, EventArgs e)
        {
            NumericEntryBox entryCount = new NumericEntryBox();
            if (entryCount.ShowDialog("TBST Generation", "Number of Entries:") == DialogResult.OK)
                GetInstance<ARCWrapper>().NewTBST(entryCount.NewValue);
        }
        protected static void NewRedirectAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewRedirect(); }
        protected static void ImportBRESAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportBRES(); }
        protected static void ImportARCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportARC(); }
        protected static void ImportBLOCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportBLOC(); }
        protected static void ImportCollisionAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportCollision(); }
        protected static void ImportMSBinAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportMSBin(); }
        protected static void ImportSCLAAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportSCLA(); }
        protected static void ImportSTDTAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportSTDT(); }
        protected static void ImportSTPMAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportSTPM(); }

        protected static void ImportTBCLAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportTBCL(); }
        protected static void ImportTBGCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportTBGC(); }
        protected static void ImportTBGDAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportTBGD(); }
        protected static void ImportTBGMAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportTBGM(); }
        protected static void ImportTBLVAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportTBLV(); }
        protected static void ImportTBRMAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportTBRM(); }
        protected static void ImportTBSTAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportTBST(); }
        protected static void ImportHavokAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportHavok(); }

        protected static void PreviewAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().PreviewAll(); }
        protected static void ExportAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ExportAll(); }
        protected static void ReplaceAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ReplaceAll(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[8].Enabled = _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[12].Enabled = _menu.Items[13].Enabled = _menu.Items[16].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            ARCWrapper w = GetInstance<ARCWrapper>();
                _menu.Items[8].Enabled = _menu.Items[10].Enabled = _menu.Items[16].Enabled = w.Parent != null;
                _menu.Items[9].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
                _menu.Items[12].Enabled = w.PrevNode != null;
                _menu.Items[13].Enabled = w.NextNode != null;
        }
        #endregion

        public override string ExportFilter
        {
            get
            {
                return "PAC Archive (*.pac)|*.pac|" +
                    "Compressed PAC Archive (*.pcs)|*.pcs|" +
                    "Archive Pair (*.pac & *.pcs)|*.pair|" +
                    "Mushroomy Kingdom (STGMARIOPAST_00.pac & STGMARIOPAST_01.pac)|*.mariopast|" +
                    "Shadow Moses Island (STGMETALGEAR_00, STGMETALGEAR_01.pac, & STGMETALGEAR_02.pac)|*.metalgear|" +
                    "Smashville (STGVILLAGE_00.pac, STGVILLAGE_01.pac, STGVILLAGE_02.pac, STGVILLAGE_03.pac, & STGVILLAGE_04.pac)|*.village|" +
                    "Spear Pillar (STGTENGAN_1.pac, STGTENGAN_2.pac, & STGTENGAN_3.pac)|*.tengan|" +
                    "Multiple Resource Group (*.mrg)|*.mrg";
            }
        }

        public ARCWrapper() { ContextMenuStrip = _menu; }

        public override ResourceNode Duplicate()
        {
            if (_resource._parent == null)
            {
                return null;
            }
            _resource.Rebuild();
            ARCNode newNode = NodeFactory.FromAddress(null, _resource.WorkingUncompressed.Address, _resource.WorkingUncompressed.Length) as ARCNode;
            int newIndex = _resource.Index + 1;
            _resource._parent.InsertChild(newNode, true, newIndex);
            newNode.Populate();
            newNode.FileType = ((ARCNode)_resource).FileType;
            newNode.FileIndex = ((ARCNode)_resource).FileIndex;
            newNode.RedirectIndex = ((ARCNode)_resource).RedirectIndex;
            newNode.GroupID = ((ARCNode)_resource).GroupID;
            newNode.Compression = ((ARCNode)_resource).Compression;
            return newNode;
        }

        public ARCNode NewARC()
        {
            ARCNode node = new ARCNode() { Name = _resource.FindName("NewARChive"), FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public ARCEntryNode NewRedirect()
        {
            ARCEntryNode node = new ARCEntryNode() { };
            node.FileType = ARCFileType.MiscData;
            node._resourceType = ResourceType.Redirect;
            _resource.AddChild(node);
            node.RedirectIndex = 0;

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public BRRESNode NewBRES()
        {
            BRRESNode node = new BRRESNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public CollisionNode NewCollision()
        {
            CollisionNode node = new CollisionNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public ItmFreqNode NewItemGeneration()
        {
            ItmFreqNode node = new ItmFreqNode() { FileType = ARCFileType.MiscData };
            node.Name = "Item Generation";
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public BLOCNode NewBLOC()
        {
            BLOCNode node = new BLOCNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public MSBinNode NewMSBin()
        {
            MSBinNode node = new MSBinNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create SCLA
        public SCLANode NewSCLA(uint index)
        {
            SCLANode node = new SCLANode(index) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create STDT
        public STDTNode NewSTDT(int numEntries)
        {
            STDTNode node = new STDTNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create STPM
        public STPMNode NewSTPM()
        {
            STPMNode node = new STPMNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create TBCL
        public TBCLNode NewTBCL(int numEntries)
        {
            TBCLNode node = new TBCLNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create TBGC
        public TBGCNode NewTBGC(int numEntries)
        {
            TBGCNode node = new TBGCNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create TBGD
        public TBGDNode NewTBGD(int numEntries)
        {
            TBGDNode node = new TBGDNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create TBGM
        public TBGMNode NewTBGM(int numEntries)
        {
            TBGMNode node = new TBGMNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create TBLV
        public TBLVNode NewTBLV(int numEntries)
        {
            TBLVNode node = new TBLVNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create TBRM
        public TBRMNode NewTBRM(int numEntries)
        {
            TBRMNode node = new TBRMNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        
        // StageBox create TBST
        public TBSTNode NewTBST(int numEntries)
        {
            TBSTNode node = new TBSTNode(null, numEntries) { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        // StageBox create Havok
        public HavokNode NewHavok(int numEntries)
        {
            HavokNode node = new HavokNode() {Name = _resource.FindName("NewHavokData"), FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public void ImportARC()
        {
            string path;
            if (Program.OpenFile("ARChive (*.pac,*.pcs)|*.pac;*.pcs", out path) > 0)
                NewARC().Replace(path);
        }
        public void ImportBRES()
        {
            string path;
            if (Program.OpenFile(FileFilters.BRES, out path) > 0)
                NewBRES().Replace(path);
        }
        public void ImportBLOC()
        {
            string path;
            if (Program.OpenFile(FileFilters.BLOC, out path) > 0)
                NewBLOC().Replace(path);
        }
        public void ImportCollision()
        {
            string path;
            if (Program.OpenFile(FileFilters.CollisionDef, out path) > 0)
                NewCollision().Replace(path);
        }
        public void ImportMSBin()
        {
            string path;
            if (Program.OpenFile(FileFilters.MSBin, out path) > 0)
                NewMSBin().Replace(path);
        }
        
        // StageBox import SCLA
        public void ImportSCLA()
        {
            string path;
            if (Program.OpenFile(FileFilters.SCLA, out path) > 0)
                NewSCLA(0).Replace(path);
        }
        
        // StageBox import STDT
        public void ImportSTDT()
        {
            string path;
            if (Program.OpenFile(FileFilters.STDT, out path) > 0)
                NewSTDT(1).Replace(path);
        }
        
        // StageBox import STPM
        public void ImportSTPM()
        {
            string path;
            if (Program.OpenFile(FileFilters.STPM, out path) > 0)
                NewSTPM().Replace(path);
        }
        
        // StageBox import TBCL
        public void ImportTBCL()
        {
            string path;
            if (Program.OpenFile(FileFilters.TBCL, out path) > 0)
                NewTBCL(1).Replace(path);
        }
        
        // StageBox import TBGC
        public void ImportTBGC()
        {
            string path;
            if (Program.OpenFile(FileFilters.TBGC, out path) > 0)
                NewTBGC(1).Replace(path);
        }
        
        // StageBox import TBGD
        public void ImportTBGD()
        {
            string path;
            if (Program.OpenFile(FileFilters.TBGD, out path) > 0)
                NewTBGD(1).Replace(path);
        }
        
        // StageBox import TBGM
        public void ImportTBGM()
        {
            string path;
            if (Program.OpenFile(FileFilters.TBGM, out path) > 0)
                NewTBGM(1).Replace(path);
        }
        
        // StageBox import TBLV
        public void ImportTBLV()
        {
            string path;
            if (Program.OpenFile(FileFilters.TBLV, out path) > 0)
                NewTBLV(1).Replace(path);
        }
        
        // StageBox import TBRM
        public void ImportTBRM()
        {
            string path;
            if (Program.OpenFile(FileFilters.TBRM, out path) > 0)
                NewTBRM(1).Replace(path);
        }
        
        // StageBox import TBST
        public void ImportTBST()
        {
            string path;
            if (Program.OpenFile(FileFilters.TBST, out path) > 0)
                NewTBST(1).Replace(path);
        }

        // StageBox import HavokData
        public void ImportHavok()
        {
            string path;
            if (Program.OpenFile(FileFilters.Havok, out path) > 0)
                NewHavok(1).Replace(path);
        }
        
        public override void OnExport(string outPath, int filterIndex)
        {
            switch (filterIndex)
            {
                case 1: ((ARCNode)_resource).Export(outPath); break;
                case 2: ((ARCNode)_resource).ExportPCS(outPath); break;
                case 3: ((ARCNode)_resource).ExportPair(outPath); break;
                case 4: ((ARCNode)_resource).ExportMarioPast(outPath); break;
                case 5: ((ARCNode)_resource).ExportMetalGear(outPath); break;
                case 6: ((ARCNode)_resource).ExportVillage(outPath); break;
                case 7: ((ARCNode)_resource).ExportTengan(outPath); break;
                case 8: ((ARCNode)_resource).ExportAsMRG(outPath); break;
            }
        }

        private void LoadModels(ResourceNode node, List<IModel> models, List<CollisionNode> collisions)
        {
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.U8:
                case ResourceType.U8Folder:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        LoadModels(n, models, collisions);
                    break;
                case ResourceType.MDL0:
                    models.Add((IModel)node);
                    break;
                case ResourceType.CollisionDef:
                    collisions.Add((CollisionNode)node);
                    break;
            }
        }

        public void PreviewAll()
        {
            List<IModel> models = new List<IModel>();
            List<CollisionNode> collisions = new List<CollisionNode>();
            LoadModels(_resource, models, collisions);
            new ModelForm().Show(_owner, models, collisions);
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            bool hasModels = false;
            bool hasTextures = false;
            foreach (ARCEntryNode b in _resource.Children)
            {
                if (b is BRRESNode)
                {
                    foreach (BRESGroupNode e in b.Children)
                    {
                        if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                            hasTextures = true;
                        else if (e.Type == BRESGroupNode.BRESGroupType.Models)
                            hasModels = true;
                        if (hasModels && hasTextures)
                            break;
                    }
                }
            }

            string extensionTEX0 = ".tex0";
            string extensionMDL0 = ".mdl0";

            if (hasTextures)
            {
                ExportAllFormatDialog dialog = new ExportAllFormatDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                    extensionTEX0 = dialog.SelectedExtension;
                else
                    return;
            }
            if (hasModels)
            {
                ExportAllFormatDialog dialog = new ExportAllFormatDialog(true);

                if (dialog.ShowDialog() == DialogResult.OK)
                    extensionMDL0 = dialog.SelectedExtension;
                else
                    return;
            }
            ((ARCNode)_resource).ExtractToFolder(path, extensionTEX0, extensionMDL0);
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;
            ExportAllFormatDialog dialog = new ExportAllFormatDialog();
            dialog.Text = "Replace All";
            dialog.label1.Text = "Input format for textures:";

            if (dialog.ShowDialog() == DialogResult.OK)
                ((ARCNode)_resource).ReplaceFromFolder(path, dialog.SelectedExtension);
        }
    }
}
