using System;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace BrawlCrate.NodeWrappers
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
            _menu.Items.Add(new ToolStripMenuItem("Generate &PAT0", null, GeneratePAT0Action));
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
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Convert Stock System", null, ConvertStockAction));
            _menu.Items[1].Visible = _menu.Items[2].Visible = _menu.Items[13].Visible = _menu.Items[14].Visible = false;
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ReEncodeAction(object sender, EventArgs e) { GetInstance<TEX0Wrapper>().ReEncode(); }
        protected static void GeneratePAT0Action(object sender, EventArgs e) { GetInstance<TEX0Wrapper>().GeneratePAT0(); }
        protected static void DeleteTEX0Action(object sender, EventArgs e) { GetInstance<TEX0Wrapper>().DeleteTEX0(); }
        protected static void ConvertStockAction(object sender, EventArgs e) { GetInstance<TEX0Wrapper>().ConvertStocks(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[8].Enabled = _menu.Items[9].Enabled = _menu.Items[12].Enabled = true;
            _menu.Items[1].Visible = _menu.Items[2].Visible = _menu.Items[13].Visible = _menu.Items[14].Visible = false;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            TEX0Wrapper w = GetInstance<TEX0Wrapper>();
            _menu.Items[2].Enabled = _menu.Items[5].Enabled = _menu.Items[12].Enabled = w.Parent != null;
            _menu.Items[1].Visible = _menu.Items[2].Visible = true; //(Regex.Match(w._resource.Name, @"(\.\d+)?$").Success && w._resource.Name.LastIndexOf(".") > 0 && w._resource.Name.LastIndexOf(".") <= w._resource.Name.Length && int.TryParse(w._resource.Name.Substring(w._resource.Name.LastIndexOf(".") + 1, w._resource.Name.Length - (w._resource.Name.LastIndexOf(".") + 1)), out int n));
            _menu.Items[6].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[8].Enabled = w.PrevNode != null;
            _menu.Items[9].Enabled = w.NextNode != null;
            _menu.Items[13].Visible = _menu.Items[14].Visible = false;
            if (w._resource.Name.StartsWith("InfStc.") && Regex.Match(w._resource.Name, @"(\.\d+)?$").Success && w._resource.Name.LastIndexOf(".") > 0 && w._resource.Name.LastIndexOf(".") <= w._resource.Name.Length && int.TryParse(w._resource.Name.Substring(w._resource.Name.LastIndexOf(".") + 1, w._resource.Name.Length - (w._resource.Name.LastIndexOf(".") + 1)), out int n))
            {
                _menu.Items[13].Visible = _menu.Items[14].Visible = true;
                _menu.Items[14].Text = w._resource.Name.Length == 10 ? "Convert to Expanded 40-Stock System" : "Convert to Default Stock System";
            }
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

        public void GeneratePAT0()
        {
            if (Parent == null)
                return;

            if (_resource.Parent is BRESGroupNode && _resource.Parent.Parent != null && _resource.Parent.Parent is BRRESNode)
            {
                // Check if this is part of a sequence
                if (Regex.Match(_resource.Name, @"(\.\d+)?$").Success && _resource.Name.LastIndexOf(".") > 0 && _resource.Name.LastIndexOf(".") <= _resource.Name.Length && int.TryParse(_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1, _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)), out int n))
                {
                    //Console.WriteLine(_resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + " is part of a sequence");
                    //Console.WriteLine(_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1, _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)));
                    // Determine the name to match
                    string matchName = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + ".";
                    BRESGroupNode paletteGroup = ((BRRESNode)_resource.Parent.Parent).GetFolder<PLT0Node>();
                    List<string> textureList = new List<string>();
                    List<PLT0Node> paletteList = new List<PLT0Node>();
                    int highestNum = -1;
                    foreach (TEX0Node tx0 in _resource.Parent.Children)
                    {
                        if (tx0.Name.StartsWith(matchName) && tx0.Name.LastIndexOf(".") > 0 && tx0.Name.LastIndexOf(".") <= tx0.Name.Length && int.TryParse(tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1, tx0.Name.Length - (tx0.Name.LastIndexOf(".") + 1)), out int n2) && n2 >= 0 && !textureList.Contains(tx0.Name))
                        {
                            // Add the matching texture to the texture list for the PAT0
                            textureList.Add(tx0.Name);
                            // Determine the highest number used
                            if (n2 > highestNum)
                                highestNum = n2;
                            Console.WriteLine(tx0.Name);
                            Console.WriteLine(tx0.HasPalette);
                            if (tx0.HasPalette)
                            {
                                paletteList.Add(tx0.GetPaletteNode());
                            }
                            else
                            {
                                paletteList.Add(null);
                            }
                        }
                    }
                    if (textureList.Count <= 0)
                        return;
                    PAT0Node newPat0 = new PAT0Node();
                    newPat0.Name = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")).Equals("InfStc") ? "InfStockface_TopN__0" : _resource.Name.Substring(0, _resource.Name.LastIndexOf("."));
                    newPat0._numFrames = highestNum + 1;
                    PAT0EntryNode pat0Entry = new PAT0EntryNode();
                    newPat0.AddChild(pat0Entry);
                    pat0Entry.Name = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")).Equals("InfStc") ? "lambert87" : _resource.Name.Substring(0, _resource.Name.LastIndexOf("."));
                    PAT0TextureNode pat0Tex = new PAT0TextureNode((PAT0Flags)7, 0);
                    pat0Entry.AddChild(pat0Tex);
                    if(((BRRESNode)(_resource.Parent.Parent)).GetFolder<PAT0Node>().FindChildrenByName(newPat0.Name).Length > 0)
                    {
                        DialogResult d = MessageBox.Show("Would you like to replace the currently existing \"" + newPat0.Name + "\" PAT0 animation?", "PAT0 Generator", MessageBoxButtons.YesNoCancel);
                        if (d == DialogResult.Cancel || d == DialogResult.Abort)
                            return;
                        if (d == DialogResult.Yes)
                            while (((BRRESNode)(_resource.Parent.Parent)).GetFolder<PAT0Node>().FindChildrenByName(newPat0.Name).Length > 0)
                                ((BRRESNode)(_resource.Parent.Parent)).GetFolder<PAT0Node>().FindChildrenByName(newPat0.Name)[0].Remove();
                    }
                    if (matchName == "InfStc." && highestNum < 1000 && !textureList.Contains("InfStc.000"))
                    {
                        textureList.Add("InfStc.000");
                        paletteList.Add(null);
                    }
                    else if(matchName == "InfStc." && !textureList.Contains("InfStc.0000"))
                    {
                        textureList.Add("InfStc.0000");
                        paletteList.Add(null);
                    }
                    //foreach(string s in textureList)
                    for(int i = 0; i < textureList.Count; ++i)
                    {
                        string s = textureList[i];
                        if(float.TryParse(s.Substring(s.LastIndexOf(".") + 1, s.Length - (s.LastIndexOf(".") + 1)), out float fr))
                        {
                            PAT0TextureEntryNode pat0texEntry = new PAT0TextureEntryNode();
                            pat0Tex.AddChild(pat0texEntry);
                            pat0texEntry.Name = s;
                            pat0texEntry._frame = fr;
                            if (paletteList[i] != null)
                            {
                                pat0Tex.HasPalette = true;
                                pat0texEntry._plt = paletteList[i].Name;
                            }
                            else if ((s == "InfStc.000" || s == "InfStc.0000") && pat0Tex.HasPalette)
                                pat0texEntry._plt = s;
                            if (fr == 0)
                            {
                                PAT0TextureEntryNode pat0texEntryFinal = new PAT0TextureEntryNode();
                                pat0Tex.AddChild(pat0texEntryFinal);
                                pat0texEntryFinal.Name = s;
                                pat0texEntryFinal._frame = highestNum + 1;
                                if (paletteList[i] != null)
                                {
                                    pat0Tex.HasPalette = true;
                                    pat0texEntryFinal.Palette = paletteList[i].Name;
                                }
                                else if ((s == "InfStc.000" || s == "InfStc.0000") && pat0Tex.HasPalette)
                                    pat0texEntryFinal._plt = s;
                            }
                        }
                        //newPat0.AddChild
                    }
                    pat0Tex._children = pat0Tex._children.OrderBy(o => ((PAT0TextureEntryNode)o)._frame).ToList();
                    if (_resource.Name.Substring(0, _resource.Name.LastIndexOf(".")).Equals("InfStc") && newPat0.FrameCount < 501)
                        newPat0.FrameCount = 501;
                    ((BRRESNode)_resource.Parent.Parent).GetOrCreateFolder<PAT0Node>().AddChild(newPat0);
                    MainForm.Instance.TargetResource(newPat0);
                }
                else
                {
                    PAT0Node newPat0 = new PAT0Node();
                    newPat0.Name = _resource.Name;
                    newPat0._numFrames = 1;
                    PAT0EntryNode pat0Entry = new PAT0EntryNode();
                    newPat0.AddChild(pat0Entry);
                    pat0Entry.Name = _resource.Name;
                    PAT0TextureNode pat0Tex = new PAT0TextureNode((PAT0Flags)7, 0);
                    pat0Entry.AddChild(pat0Tex);
                    PAT0TextureEntryNode pat0texEntry = new PAT0TextureEntryNode();
                    pat0Tex.AddChild(pat0texEntry);
                    pat0texEntry.Name = _resource.Name;
                    pat0texEntry._frame = 0;
                    if (((TEX0Node)(_resource)).HasPalette)
                    {
                        pat0Tex.HasPalette = true;
                        pat0texEntry.Palette = ((TEX0Node)_resource).GetPaletteNode().Name;
                    }
                    ((BRRESNode)_resource.Parent.Parent).GetOrCreateFolder<PAT0Node>().AddChild(newPat0);
                    MainForm.Instance.TargetResource(newPat0);
                }
            }
        }

        public void DeleteTEX0()
        {
            if (Parent == null || (MainForm.Instance != null && Form.ActiveForm != null && Form.ActiveForm != MainForm.Instance))
                return;

            if (((TEX0Node)_resource).HasPalette)
            {
                PLT0Node plt0 = ((TEX0Node)_resource).GetPaletteNode();
                if (MessageBox.Show("Would you like to delete the associated PLT0?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    plt0.Parent.RemoveChild(plt0);
            }

            _resource.Dispose();
            _resource.Remove();
        }

        internal protected override void OnPropertyChanged(ResourceNode node)
        {
            RefreshView(node);
        }

        public void ConvertStocks()
        {
            if (Parent == null)
                return;

            if (_resource.Parent is BRESGroupNode && _resource.Parent.Parent != null && _resource.Parent.Parent is BRRESNode)
            {
                // Check if this is part of a sequence
                if (Regex.Match(_resource.Name, @"(\.\d+)?$").Success && _resource.Name.LastIndexOf(".") > 0 && _resource.Name.LastIndexOf(".") <= _resource.Name.Length && int.TryParse(_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1, _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)), out int n))
                {
                    if (_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1, _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)).Length == 3)
                    {
                        ConvertToStock40();
                        return;
                    }
                    else if (_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1, _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)).Length == 4)
                    {
                        ConvertToStockDefault();
                        return;
                    }
                }
            }
        }

        public void ConvertToStock40()
        {
            string matchName = _resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + ".";
            List<TEX0Node> texList = new List<TEX0Node>();
            foreach (TEX0Node tx0 in _resource.Parent.Children)
            {
                if (tx0.Name.StartsWith(matchName) && tx0.Name.LastIndexOf(".") > 0 && tx0.Name.LastIndexOf(".") < tx0.Name.Length && tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1).Length == 3 && int.TryParse(tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1, tx0.Name.Length - (tx0.Name.LastIndexOf(".") + 1)), out int x) && x >= 0)
                {
                    // WarioMan edge case (should pre-program)
                    if (x >= 475)
                        tx0.texSortNum = 9001 + (x % 475);
                    else
                    {
                        tx0.texSortNum = ((int)(Math.Floor(((Double)x - 1) / 10.0)) * 40) + (x % 10);

                        if (x % 10 == 0)
                            tx0.texSortNum += 10;

                        if ((x >= 201 && x <= 205) || // Ganon Edge Case
                            (x >= 351 && x <= 355) || // ROB Edge Case
                            (x >= 381 && x <= 384) || // Wario Edge Case
                            (x >= 411 && x <= 415) || // Toon Link Edge Case
                            (x >= 471 && x <= 474))   // Sonic Edge Case
                            tx0.texSortNum -= 30;
                    }
                    if (tx0.HasPalette)
                        tx0.GetPaletteNode().Name = "InfStc." + tx0.texSortNum.ToString("0000");
                    tx0.Name = "InfStc." + tx0.texSortNum.ToString("0000");
                }
            }
        }

        public void ConvertToStockDefault()
        {

        }
    }
}
