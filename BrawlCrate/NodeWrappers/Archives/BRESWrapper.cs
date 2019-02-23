﻿using System;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;

namespace BrawlCrate
{
    [NodeWrapper(ResourceType.BRES)]
    class BRESWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static BRESWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("Model", null, NewModelAction),
                new ToolStripMenuItem("Model Animation", null, NewChrAction),
                new ToolStripMenuItem("Texture Animation", null, NewSrtAction),
                new ToolStripMenuItem("Texture Pattern", null, NewPatAction),
                new ToolStripMenuItem("Visibility Sequence", null, NewVisAction),
                new ToolStripMenuItem("Vertex Morph", null, NewShpAction),
                new ToolStripMenuItem("Color Sequence", null, NewClrAction),
                new ToolStripMenuItem("Scene Settings", null, NewScnAction)
                ));
            _menu.Items.Add(new ToolStripMenuItem("&Import", null,
                new ToolStripMenuItem("Texture", null, ImportTextureAction),
                new ToolStripMenuItem("Color Smashable Textures", null, ImportCSTexturesAction),
                new ToolStripMenuItem("Model", null, ImportModelAction),
                new ToolStripMenuItem("Model Animation", null, ImportChrAction),
                new ToolStripMenuItem("Texture Animation", null, ImportSrtAction),
                new ToolStripMenuItem("Texture Pattern", null, ImportPatAction),
                new ToolStripMenuItem("Visibility Sequence", null, ImportVisAction),
                new ToolStripMenuItem("Vertex Morph", null, ImportShpAction),
                new ToolStripMenuItem("Color Sequence", null, ImportClrAction),
                new ToolStripMenuItem("Scene Settings", null, ImportScnAction),
                new ToolStripMenuItem("Folder", null, ImportFolderAction),
                new ToolStripMenuItem("Animated GIF", null, ImportGIFAction),
                new ToolStripMenuItem("Common Files (From Internet)", null,
                    new ToolStripMenuItem("Static (Empty) Model", null, ImportCommonModelStaticAction),
                    new ToolStripMenuItem("Character Spy Textures", null, ImportCommonTextureSpyAction),
                    new ToolStripMenuItem("Stage Shadow Texture", null, ImportCommonTextureShadowAction)
                    /*new ToolStripMenuItem("Models", null,
                        new ToolStripMenuItem("Static (Empty) Model", null, ImportCommonModelStaticAction)
                        ),
                    new ToolStripMenuItem("Textures", null,
                        new ToolStripMenuItem("Spy", null, ImportCommonTextureSpyAction),
                        new ToolStripMenuItem("Stage Shadow", null, ImportCommonTextureShadowAction)
                    )*/
                )
                ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Preview All Models", null, PreviewAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Edit All", null, EditAllAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ImportCSTexturesAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportCSTextures(); }
        protected static void ImportTextureAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportTexture(); }
        protected static void ImportModelAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportModel(); }
        protected static void ImportChrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportChr(); }
        protected static void ImportSrtAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportSrt(); }
        protected static void ImportPatAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportPat(); }
        protected static void ImportVisAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportVis(); }
        protected static void ImportShpAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportShp(); }
        protected static void ImportScnAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportScn(); }
        protected static void ImportClrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportClr(); }
        
        protected static void NewModelAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewModel(); }
        protected static void NewChrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewChr(); }
        protected static void NewSrtAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewSrt(); }
        protected static void NewPatAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewPat(); }
        protected static void NewVisAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewVis(); }
        protected static void NewShpAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewShp(); }
        protected static void NewScnAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewScn(); }
        protected static void NewClrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewClr(); }
        
        protected static void ExportAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ExportAll(); }
        protected static void ImportFolderAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportFolder(); }
        protected static void ReplaceAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ReplaceAll(); }
        protected static void EditAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().EditAll(); }
        protected static void PreviewAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().PreviewAll(); }
        protected static void ImportGIFAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportGIF(); }

        // Common import files
        protected static void ImportCommonModelStaticAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportCommonModelStatic(); }
        protected static void ImportCommonTextureSpyAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportCommonTextureSpy(); }
        protected static void ImportCommonTextureShadowAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportCommonTextureShadow(); }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[11].Enabled = _menu.Items[13].Enabled = _menu.Items[14].Enabled = _menu.Items[16].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BRESWrapper w = GetInstance<BRESWrapper>();

            _menu.Items[9].Enabled = _menu.Items[11].Enabled = _menu.Items[16].Enabled = w.Parent != null;
            _menu.Items[10].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[13].Enabled = w.PrevNode != null;
            _menu.Items[14].Enabled = w.NextNode != null;
        }

        #endregion

        public override string ExportFilter { get { return FileFilters.BRES; } }

        public BRESWrapper() { ContextMenuStrip = _menu; }

        public override ResourceNode Duplicate()
        {
            if (_resource._parent == null)
            {
                return null;
            }
            _resource.Rebuild();
            BRRESNode newNode = NodeFactory.FromAddress(null, _resource.WorkingUncompressed.Address, _resource.WorkingUncompressed.Length) as BRRESNode;
            _resource._parent.InsertChild(newNode, true, _resource.Index + 1);
            newNode.Populate();
            newNode.FileType = ((BRRESNode)_resource).FileType;
            newNode.FileIndex = ((BRRESNode)_resource).FileIndex;
            newNode.GroupID = ((BRRESNode)_resource).GroupID;
            newNode.RedirectIndex = ((BRRESNode)_resource).RedirectIndex;
            newNode.Compression = ((BRRESNode)_resource).Compression;
            newNode.Name = _resource.Name;
            return newNode;
        }

        public void ImportGIF()
        {
            string path;
            int index = Program.OpenFile("Animated GIF (*.gif)|*.gif", out path);
            if (index > 0)
                ((BRRESNode)_resource).ImportGIF(path);
        }

        public void ImportTexture()
        {
            string path;
            int index = Program.OpenFile(FileFilters.TEX0, out path);
            if (index == 8)
            {
                TEX0Node node = NodeFactory.FromFile(null, path) as TEX0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                string palette = Path.ChangeExtension(path, ".plt0");
                if (File.Exists(palette) && node.HasPalette)
                {
                    PLT0Node n = NodeFactory.FromFile(null, palette) as PLT0Node;
                    ((BRRESNode)_resource).GetOrCreateFolder<PLT0Node>().AddChild(n);
                }

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
            else if (index > 0)
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = path;
                    if (dlg.ShowDialog(MainForm.Instance, ResourceNode as BRRESNode) == DialogResult.OK)
                    {
                        BaseWrapper w = this.FindResource(dlg.TEX0TextureNode, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
        }
        
        public void ImportCSTextures()
        {
            OpenFileDialog _openDlg = new OpenFileDialog();
            _openDlg.Multiselect = true;
            _openDlg.Filter = "PNG (*.png)|*.png";
            List<string> texNames = new List<string>();
            if (_openDlg.ShowDialog() == DialogResult.OK)
            {
                TEX0Node._updating = true;
                DialogResult dr = MessageBox.Show("Have these textures already been sent through the Color Smash Tool?", "Color Smash", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    // Don't color smash them again
                    int curindex = ((BRRESNode)_resource).GetOrCreateFolder<TEX0Node>().Children.Count;
                    int j = 0;
                    foreach (string file in _openDlg.FileNames)
                    {
                        FileInfo f = new FileInfo(file);
                        using (TextureConverterDialog dlg = new TextureConverterDialog())
                        {
                            dlg.ImageSource = file;
                            if (dlg.ShowDialog(null, (BRRESNode)_resource, true, true, f.Name.Substring(0, f.Name.ToLower().LastIndexOf(".png")), false, curindex) == DialogResult.OK)
                            {
                                if (j < _openDlg.FileNames.Length - 1)
                                    dlg.TEX0TextureNode.SharesData = true;
                                curindex++;
                            }
                            j++;
                        }
                    }
                }
                else if (dr == DialogResult.No)
                {
                    // Color Smash them
                    DirectoryInfo inputDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\");
                    DirectoryInfo outputDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\out\\");
                    try
                    {
                        foreach (FileInfo tex in outputDir.GetFiles())
                            try { tex.Delete(); } catch { }
                        foreach (FileInfo tex in Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\").GetFiles())
                            try { tex.Delete(); } catch { }
                    }
                    catch
                    {

                    }
                    int i = 0;
                    foreach (string file in _openDlg.FileNames)
                    {
                        FileInfo f = new FileInfo(file);
                        texNames.Add(f.Name.Substring(0, f.Name.ToLower().LastIndexOf(".png")));
                        f.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + i + ".png");
                        i++;
                    }
                    Process csmash = Process.Start(new ProcessStartInfo()
                    {
                        FileName = AppDomain.CurrentDomain.BaseDirectory + "color_smash.exe",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = String.Format("-c RGB5A3"),
                    });
                    csmash.WaitForExit();
                    List<int> remainingIDs = new List<int>();
                    bool errorThrown = false;
                    bool attemptRegardless = false;
                    int curindex = ((BRRESNode)_resource).GetOrCreateFolder<TEX0Node>().Children.Count;
                    for (int j = 0; j < texNames.Count; j++)
                    {
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\out\\" + j + ".png"))
                        {
                            using (TextureConverterDialog dlg = new TextureConverterDialog())
                            {
                                dlg.ImageSource = AppDomain.CurrentDomain.BaseDirectory + "\\cs\\out\\" + j + ".png";
                                if (dlg.ShowDialog(MainForm.Instance, (BRRESNode)_resource, true, true, texNames[j], false, curindex) == DialogResult.OK)
                                {
                                    if (j < texNames.Count - 1)
                                        dlg.TEX0TextureNode.SharesData = true;
                                    curindex++;
                                }
                            }
                        }
                        else
                        {
                            if (!errorThrown)
                            {
                                errorThrown = true;
                                attemptRegardless = (MessageBox.Show("One or more images threw an error when converting. Would you like to try to color smash these regardless? (As opposed to keeping them seperate)", "Color Smash", MessageBoxButtons.YesNo) == DialogResult.Yes);
                            }
                            if (attemptRegardless)
                            {
                                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + j + ".png"))
                                {
                                    using (TextureConverterDialog dlg = new TextureConverterDialog())
                                    {
                                        dlg.ImageSource = AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + j + ".png";
                                        if (dlg.ShowDialog(null, (BRRESNode)_resource, true, true, texNames[j], false, curindex) == DialogResult.OK)
                                        {
                                            if (j < texNames.Count - 1)
                                                dlg.TEX0TextureNode.SharesData = true;
                                            curindex++;
                                        }
                                    }
                                }
                            }
                            else
                                remainingIDs.Add(j);
                        }
                    }
                    for (int j = 0; j < remainingIDs.Count; j++)
                    {
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + remainingIDs[j] + ".png"))
                        {
                            using (TextureConverterDialog dlg = new TextureConverterDialog())
                            {
                                dlg.ImageSource = AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + remainingIDs[j] + ".png";
                                if (dlg.ShowDialog(MainForm.Instance, (BRRESNode)_resource, false, true, texNames[remainingIDs[j]], false, curindex) == DialogResult.OK)
                                {
                                    //BaseWrapper w = this.FindResource(dlg.TEX0TextureNode, true);
                                    curindex++;
                                }
                            }
                        }
                    }
                    try
                    {
                        foreach (FileInfo tex in outputDir.GetFiles())
                            try { tex.Delete(); } catch { }
                        outputDir.Delete();
                        foreach (FileInfo tex in Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\").GetFiles())
                            try { tex.Delete(); } catch { }
                        Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\");
                    }
                    catch
                    {

                    }
                }
            }
            TEX0Node._updating = false;
        }

        public void ImportModel()
        {
            string path;
            if (Program.OpenFile(FileFilters.MDL0Import, out path) > 0)
            {
                MDL0Node node = MDL0Node.FromFile(path);
                if (node != null)
                {
                    ((BRRESNode)_resource).GetOrCreateFolder<MDL0Node>().AddChild(node);

                    BaseWrapper w = this.FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportChr()
        {
            string path;
            if (Program.OpenFile(FileFilters.CHR0Import, out path) > 0)
            {
                CHR0Node node = CHR0Node.FromFile(path);
                ((BRRESNode)_resource).GetOrCreateFolder<CHR0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportVis()
        {
            string path;
            if (Program.OpenFile(FileFilters.VIS0, out path) > 0)
            {
                VIS0Node node = NodeFactory.FromFile(null, path) as VIS0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<VIS0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportShp()
        {
            string path;
            if (Program.OpenFile(FileFilters.SHP0, out path) > 0)
            {
                SHP0Node node = NodeFactory.FromFile(null, path) as SHP0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<SHP0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportSrt()
        {
            string path;
            if (Program.OpenFile(FileFilters.SRT0, out path) > 0)
            {
                SRT0Node node = NodeFactory.FromFile(null, path) as SRT0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<SRT0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportPat()
        {
            string path;
            if (Program.OpenFile(FileFilters.PAT0, out path) > 0)
            {
                PAT0Node node = NodeFactory.FromFile(null, path) as PAT0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<PAT0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportScn()
        {
            string path;
            if (Program.OpenFile(FileFilters.SCN0, out path) > 0)
            {
                SCN0Node node = NodeFactory.FromFile(null, path) as SCN0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<SCN0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void ImportClr()
        {
            string path;
            if (Program.OpenFile(FileFilters.CLR0, out path) > 0)
            {
                CLR0Node node = NodeFactory.FromFile(null, path) as CLR0Node;
                ((BRRESNode)_resource).GetOrCreateFolder<CLR0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }

        public void NewChr()
        {
            CHR0Node node = ((BRRESNode)_resource).CreateResource<CHR0Node>("NewCHR");
            node.Version = 4;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewSrt()
        {
            SRT0Node node = ((BRRESNode)_resource).CreateResource<SRT0Node>("NewSRT");
            node.Version = 4;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewPat()
        {
            PAT0Node node = ((BRRESNode)_resource).CreateResource<PAT0Node>("NewPAT");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewShp()
        {
            SHP0Node node = ((BRRESNode)_resource).CreateResource<SHP0Node>("NewSHP");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewVis()
        {
            VIS0Node node = ((BRRESNode)_resource).CreateResource<VIS0Node>("NewVIS");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewScn()
        {
            SCN0Node node = ((BRRESNode)_resource).CreateResource<SCN0Node>("NewSCN");
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewClr()
        {
            CLR0Node node = ((BRRESNode)_resource).CreateResource<CLR0Node>("NewCLR");
            node.Version = 3;
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void NewModel()
        {
            MDL0Node node = ((BRRESNode)_resource).CreateResource<MDL0Node>("NewModel");
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            bool hasModels = false;
            bool hasTextures = false;
            foreach (BRESGroupNode e in _resource.Children)
            {
                if (e.Type == BRESGroupNode.BRESGroupType.Textures)
                    hasTextures = true;
                else if (e.Type == BRESGroupNode.BRESGroupType.Models)
                    hasModels = true;
                if (hasModels && hasTextures)
                    break;
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
            ((BRRESNode)_resource).ExportToFolder(path, extensionTEX0, extensionMDL0);
        }

        public void EditAll()
        {
            EditAllDialog ctd = new EditAllDialog();
            ctd.ShowDialog(_owner, _resource);
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
                ((BRRESNode)_resource).ReplaceFromFolder(path, dialog.SelectedExtension);
        }

        public void ImportFolder()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((BRRESNode)_resource).ImportFolder(path);
        }

        private void LoadModels(ResourceNode node, List<MDL0Node> models)
        {
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.U8:
                case ResourceType.MRG:
                case ResourceType.BRES:
                case ResourceType.U8Folder:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        LoadModels(n, models);
                    break;
                case ResourceType.MDL0:
                    models.Add((MDL0Node)node);
                    break;
            }
        }

        public static string commonDirectory = AppDomain.CurrentDomain.BaseDirectory + '\\' + "CommonFiles";
        public void ImportCommonModelStatic()
        {
            Directory.CreateDirectory(commonDirectory);
            Directory.CreateDirectory(commonDirectory + '\\' + "Models");
            string endLocation = (commonDirectory + '\\' + "Models" + '\\' + "Static.mdl0");
            if(!File.Exists(endLocation))
            {
                // Use TLS 1.2, used by GitHub
                ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://github.com/soopercool101/BrawlCommonFiles/raw/master/Models/Static/Static.mdl0",
                                        @endLocation);
                }
            }

            if (File.Exists(endLocation))
            {
                MDL0Node node = MDL0Node.FromFile(endLocation);
                if (node != null)
                {
                    ((BRRESNode)_resource).GetOrCreateFolder<MDL0Node>().AddChild(node);

                    BaseWrapper w = this.FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void ImportCommonTextureShadow()
        {
            Directory.CreateDirectory(commonDirectory);
            Directory.CreateDirectory(commonDirectory + '\\' + "Textures");
            string endLocation = (commonDirectory + '\\' + "Textures" + '\\' + "TShadow1.tex0");
            if (!File.Exists(endLocation))
            {
                // Use TLS 1.2, used by GitHub
                ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://github.com/soopercool101/BrawlCommonFiles/raw/master/Textures/Stages/Shadow/TShadow1.tex0",
                                        @endLocation);
                }
            }

            if (File.Exists(endLocation))
            {
                TEX0Node node = NodeFactory.FromFile(null, endLocation) as TEX0Node;
                if(node != null)
                {
                    ((BRRESNode)_resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                    string palette = Path.ChangeExtension(endLocation, ".plt0");
                    if (File.Exists(palette) && node.HasPalette)
                    {
                        PLT0Node n = NodeFactory.FromFile(null, palette) as PLT0Node;
                        ((BRRESNode)_resource).GetOrCreateFolder<PLT0Node>().AddChild(n);
                    }

                    BaseWrapper w = this.FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }
        
        public void ImportCommonTextureSpy()
        {
            Directory.CreateDirectory(commonDirectory);
            Directory.CreateDirectory(commonDirectory + '\\' + "Textures");
            string endLocationFB = (commonDirectory + '\\' + "Textures" + '\\' + "FB.tex0");
            string endLocationSpyCloak = (commonDirectory + '\\' + "Textures" + '\\' + "spycloak00.tex0");
            if (!File.Exists(endLocationFB))
            {
                // Use TLS 1.2, used by GitHub
                ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://github.com/soopercool101/BrawlCommonFiles/raw/master/Textures/Characters/Spy/FB.tex0",
                                        @endLocationFB);
                }
            }
            if (!File.Exists(endLocationSpyCloak))
            {
                // Use TLS 1.2, used by GitHub
                ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://github.com/soopercool101/BrawlCommonFiles/raw/master/Textures/Characters/Spy/spycloak00.tex0",
                                        @endLocationSpyCloak);
                }
            }

            if (File.Exists(endLocationFB))
            {
                TEX0Node node = NodeFactory.FromFile(null, endLocationFB) as TEX0Node;

                if (node != null)
                {
                    ((BRRESNode)_resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                    string palette = Path.ChangeExtension(endLocationFB, ".plt0");
                    if (File.Exists(palette) && node.HasPalette)
                    {
                        PLT0Node n = NodeFactory.FromFile(null, palette) as PLT0Node;
                        ((BRRESNode)_resource).GetOrCreateFolder<PLT0Node>().AddChild(n);
                    }

                    BaseWrapper w = this.FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
            if (File.Exists(endLocationSpyCloak))
            {
                TEX0Node node = NodeFactory.FromFile(null, endLocationSpyCloak) as TEX0Node;
                if (node != null)
                {
                    ((BRRESNode)_resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                    string palette = Path.ChangeExtension(endLocationSpyCloak, ".plt0");
                    if (File.Exists(palette) && node.HasPalette)
                    {
                        PLT0Node n = NodeFactory.FromFile(null, palette) as PLT0Node;
                        ((BRRESNode)_resource).GetOrCreateFolder<PLT0Node>().AddChild(n);
                    }

                    BaseWrapper w = this.FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }

        public void PreviewAll()
        {
            new ModelForm().Show(_owner, ModelPanel.CollectModels(_resource));
        }
    }
}
