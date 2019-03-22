using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using System.Drawing;
using BrawlLib.IO;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEX0Node : BRESEntryNode, IImageSource
    {
        internal TEX0v1* Header1 { get { return (TEX0v1*)WorkingUncompressed.Address; } }
        internal TEX0v2* Header2 { get { return (TEX0v2*)WorkingUncompressed.Address; } }
        internal TEX0v3* Header3 { get { return (TEX0v3*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return SharesData ? ResourceType.SharedTEX0 : ResourceType.TEX0; } }
        public override int DataAlign { get { return 0x20; } }
        public override int[] SupportedVersions { get { return new int[] { 1, 2, 3 }; } }

        public TEX0Node() { _version = 1; }

        // Texture number used for stuff like stocks. Used for sorting purposes.
        public int texSortNum = -1;

        public bool _sharesData;
        int _headerLen;
        int _width, _height;
        WiiPixelFormat _format;
        int _lod;
        bool _hasPalette;

        public static bool _updating = false;

        // Could improve performance by caching, and making sure to clear the cache when needed.
        // For now, prefer the simplicity of not identifying every situation where clearing the cache would be needed.
        TEX0Node SourceNode { get { return FindSourceNode(); } }

        [Category("G3D Node")]
        public bool SharesData {
            get
            {
                return _sharesData;
            }
            set
            {
                if (_updating)
                {
                    _sharesData = value;
                    SignalPropertyChange();
                    return;
                }
                bool needsSmashing = false;
                Bitmap bmp = GetImage(0);
                if (MessageBox.Show("Would you like to attempt to automatically change Color Smash state of this texture as well?" + (value ? " (Choose no if you're using the old Color Smash method)" : ""), "Color Smasher", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    _sharesData = value;
                    SignalPropertyChange();
                    if (value == false)
                    {
                        using (System.Windows.Forms.TextureConverterDialog dlg = new System.Windows.Forms.TextureConverterDialog())
                        {
                            dlg.LoadImages(bmp);
                            dlg.ShowDialog(null, this, true, true);
                        }
                    }
                    return;
                }
                if (value == true)
                {
                    needsSmashing = true;
                    TEX0Node t = this;
                    List<TEX0Node> texList = new List<TEX0Node>();
                    texList.Add(t);
                    /*while (t.PrevSibling() != null && ((TEX0Node)t.PrevSibling()).SharesData)
                    {
                        texList.Add(t.PrevSibling() as TEX0Node);
                        t = t.PrevSibling() as TEX0Node;
                    }
                    t = this;*/
                    while (t.NextSibling() != null && ((TEX0Node)t.NextSibling()).SharesData)
                    {
                        texList.Add(t.NextSibling() as TEX0Node);
                        t = t.NextSibling() as TEX0Node;
                    }
                    if(t.NextSibling() != null)
                        texList.Add(t.NextSibling() as TEX0Node);
                    ColorSmash(texList);
                }
                else if (value == false)
                {
                    if (PrevSibling() != null && ((TEX0Node)PrevSibling()).SharesData)
                    {
                        // Needs to color smash
                        TEX0Node t = this;
                        List<TEX0Node> texList = new List<TEX0Node>();
                        texList.Add(t);
                        while (t.PrevSibling() != null && ((TEX0Node)t.PrevSibling()).SharesData)
                        {
                            texList.Add(t.PrevSibling() as TEX0Node);
                            t = t.PrevSibling() as TEX0Node;
                        }
                        ColorSmash(texList);
                    }
                }
                _sharesData = value;
                if (value == false)
                {
                    using (System.Windows.Forms.TextureConverterDialog dlg = new System.Windows.Forms.TextureConverterDialog())
                    {
                        dlg.LoadImages(bmp);
                        dlg.ShowDialog(null, this, true, true);
                    }
                }
                SignalPropertyChange();
            }
        }
        
        public static void ColorSmash(List<TEX0Node> texList)
        {
            if (_updating)
                return;
            TEX0Node._updating = true;
            texList.Sort((x, y) => x.Index.CompareTo(y.Index));
            int curindex = texList[0].Index;
            int parentCount = texList[0].Parent.Children.Count;
            BRRESNode brparent = texList[0].Parent.Parent as BRRESNode;
            List<string> texNames = new List<string>();
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\");
            DirectoryInfo outputDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\out\\");
            bool usesOnlyCI4 = true;
            for (int i = 0; i < texList.Count; i++)
            {
                TEX0Node tex = texList[i];
                texNames.Add(tex.Name);
                tex.Export(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + i + ".png");
                if (tex.HasPalette)
                {
                    tex.GetPaletteNode().Remove();
                }
                if (tex.Format != BrawlLib.Wii.Textures.WiiPixelFormat.CI4)
                    usesOnlyCI4 = false;
                tex.Remove();
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
            for (int j = 0; j < texNames.Count; j++)
            {
                Console.WriteLine(j);
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\out\\" + j + ".png"))
                {
                    using (TextureConverterDialog dlg = new TextureConverterDialog())
                    {
                        dlg.ImageSource = AppDomain.CurrentDomain.BaseDirectory + "\\cs\\out\\" + j + ".png";
                        if (dlg.ShowDialog(null, brparent, true, true, texNames[j], usesOnlyCI4, curindex) == DialogResult.OK)
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
                        attemptRegardless = false;//(MessageBox.Show("One or more images threw an error when converting. Would you like to try to color smash these regardless? (As opposed to keeping them seperate)\n" + AppDomain.CurrentDomain.BaseDirectory + "\\cs\\out\\" + j + ".png", "Color Smash", MessageBoxButtons.YesNo) == DialogResult.Yes);
                    }
                    if (attemptRegardless)
                    {
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + j + ".png"))
                        {
                            using (TextureConverterDialog dlg = new TextureConverterDialog())
                            {
                                dlg.ImageSource = AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + j + ".png";
                                if (dlg.ShowDialog(null, brparent, true, true, texNames[j], usesOnlyCI4, curindex) == DialogResult.OK)
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
                    Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + remainingIDs[j] + ".png");
                    using (TextureConverterDialog dlg = new TextureConverterDialog())
                    {
                        dlg.ImageSource = AppDomain.CurrentDomain.BaseDirectory + "\\cs\\" + remainingIDs[j] + ".png";
                        if (dlg.ShowDialog(null, brparent, false, true, texNames[remainingIDs[j]], false, curindex) == DialogResult.OK)
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
                try { outputDir.Delete(); } catch { }
                foreach (FileInfo tex in Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\").GetFiles())
                    try { tex.Delete(); } catch { }
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\cs\\");
            }
            catch
            {

            }
            TEX0Node._updating = false;
            brparent.SignalPropertyChange();
        }

        [Category("G3D Texture")]
        public int Width { get { return SharesData ? SourceNode.Width : _width; } }
        [Category("G3D Texture")]
        public int Height { get { return SharesData ? SourceNode.Height : _height; } }
        [Category("G3D Texture")]
        public WiiPixelFormat Format { get { return SharesData ? SourceNode.Format : _format; } }
        [Category("G3D Texture")]
        public int LevelOfDetail { get { return SharesData ? SourceNode.LevelOfDetail : _lod; } }
        [Category("G3D Texture")]
        public bool HasPalette { get { return SharesData ? SourceNode.HasPalette : _hasPalette; } }

        public PLT0Node GetPaletteNode() { return ((_parent == null) || (!HasPalette)) ? null : Parent._parent.FindChild("Palettes(NW4R)/" + this.Name, false) as PLT0Node; }


        [Browsable(false)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; if (HasPalette && GetPaletteNode() != null) { GetPaletteNode().Name = value; } }
        }

        int HeaderSize()
        {
            int headerLen = TEX0v1.Size;
            if (Version == 3)
                headerLen += _userEntries.GetSize();
            return headerLen;
        }
        int DataSize()
        {
            return TextureConverter.Get(Format).GetMipOffset(Width, Height, LevelOfDetail + 1);
        }
        // The offset from the start of the header to the start of its data.
        // Equal to the length of its header if not shared, the length of all headers between it and its data if shared.
        int OffsetToData()
        {
            int offset = 0;
            ResourceNode node = this;

            while (node is TEX0Node)
            {
                TEX0Node texNode = node as TEX0Node;
                offset += texNode.HeaderSize();
                if (!texNode.SharesData)
                    return offset;
                node = node.NextSibling();
            }
            return 0;
        }
        // The size of this entry, not including any shared data.
        // Equals the size of the header if shared, header + data if not shared.
        int ExclusiveEntrySize()
        {
            int size = HeaderSize();
            if (!SharesData)
                size += DataSize();
            return size;
        }
        // This size of this entry, including any shared data and other entries' headers between it and its data.
        int InclusiveEntrySize()
        {
            return OffsetToData() + DataSize();
        }

        // A TEX0 node we use instead of null when we share with a non-existent node.
        // This way we don't have to write null checks on the return value of SourceNode.
        private static TEX0Node _nullTEX0Node;
        private static TEX0Node NullTEX0Node()
        {
            if (_nullTEX0Node == null)
            {
                TEX0v1 NullTEX0Header = new TEX0v1(0, 0, WiiPixelFormat.CMPR, 1);
                _nullTEX0Node = new TEX0Node();
                _nullTEX0Node.Initialize(null, &NullTEX0Header, sizeof(TEX0v1));
            }
            return _nullTEX0Node;
        }

        TEX0Node FindSourceNode()
        {
            ResourceNode candidate = this;
            while (candidate is TEX0Node)
            {
                TEX0Node texNode = candidate as TEX0Node;
                if (!texNode.SharesData)
                    return texNode;
                candidate = candidate.NextSibling();
            }
            return NullTEX0Node();
        }


        public override bool OnInitialize()
        {
            base.OnInitialize();
            _headerLen = Header1->_headerLen;

            _sharesData = _headerLen > HeaderSize();
            if (_sharesData)
                SetSizeInternal(ExclusiveEntrySize());

            if (_version == 2)
            {
                if ((_name == null) && (Header2->_stringOffset != 0))
                    _name = Header2->ResourceString;

                _width = Header2->_width;
                _height = Header2->_height;
                _format = Header2->PixelFormat;
                _lod = Header2->_levelOfDetail;
                _hasPalette = Header2->HasPalette;
            }
            else
            {
                if ((_name == null) && (Header1->_stringOffset != 0))
                    _name = Header1->ResourceString;

                _width = Header1->_width;
                _height = Header1->_height;
                _format = Header1->PixelFormat;
                _lod = Header1->_levelOfDetail;
                _hasPalette = Header1->HasPalette;
            }

            if (_version == 3)
                (_userEntries = new UserDataCollection()).Read(Header3->UserData);

            return false;
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            return ExclusiveEntrySize();
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int offset = OffsetToData();
            if (!SharesData)
                Memory.Move(address + offset, WorkingUncompressed.Address + offset, (uint)length - (uint)offset);

            switch (Version)
            {
                case 1:
                    *(TEX0v1*)address = new TEX0v1(Width, Height, Format, LevelOfDetail, offset);
                    break;
                case 2:
                    *(TEX0v2*)address = new TEX0v2(Width, Height, Format, LevelOfDetail, offset);
                    break;
                case 3:
                    *(TEX0v3*)address = new TEX0v3(Width, Height, Format, LevelOfDetail, offset);
                    _userEntries.Write(address + TEX0v3.Size);
                    break;
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            if (_version == 3)
                _userEntries.GetStrings(table);

            if (!String.IsNullOrEmpty(_originalPath))
                table.Add(_originalPath);
        }

        [Browsable(false)]
        public int ImageCount { get { return LevelOfDetail; } }
        public Bitmap GetImage(int index)
        {
            PLT0Node plt = GetPaletteNode();
            return SharesData ? SourceNode.GetImage(index, plt) : GetImage(index, plt);
        }

        public Bitmap GetImage(int index, PLT0Node plt)
        {
            try
            {
                if (SharesData && SourceNode != this)
                    return SourceNode.GetImage(index, plt);

                if (WorkingUncompressed != DataSource.Empty)
                    if (plt != null)
                        return TextureConverter.DecodeIndexed(
                            (VoidPtr)CommonHeader + _headerLen, _width, _height, plt.Palette, index + 1, _format);
                    else
                        return TextureConverter.Decode(
                            (VoidPtr)CommonHeader + _headerLen, _width, _height, index + 1, _format);
                else
                    return null;
            }
            catch { return null; }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            if (SharesData)
            {
                BRESCommonHeader* commonHeader = (BRESCommonHeader*)dataAddress;
                commonHeader->_size = InclusiveEntrySize();
            }

            if (_version == 2)
            {
                TEX0v2* header = (TEX0v2*)dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!String.IsNullOrEmpty(_originalPath))
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
            }
            else
            {
                TEX0v1* header = (TEX0v1*)dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!String.IsNullOrEmpty(_originalPath))
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
            }
        }

        public void Replace(Bitmap bmp)
        {
            FileMap tMap, pMap;
            if (HasPalette)
            {
                PLT0Node pn = GetPaletteNode();
                tMap = TextureConverter.Get(Format).EncodeTextureIndexed(bmp, LevelOfDetail, pn.Colors, pn.Format, QuantizationAlgorithm.MedianCut, out pMap);
                pn.ReplaceRaw(pMap);
            }
            else
                tMap = TextureConverter.Get(Format).EncodeTEX0Texture(bmp, LevelOfDetail);
            ReplaceRaw(tMap);
        }

        public override unsafe void Replace(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (!String.Equals(ext, ".tex0", StringComparison.OrdinalIgnoreCase))
                using (System.Windows.Forms.TextureConverterDialog dlg = new System.Windows.Forms.TextureConverterDialog())
                {
                    dlg.ImageSource = fileName;
                    dlg.ShowDialog(null, this);
                }
            else
                base.Replace(fileName);
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".png"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Png);
            else if (outPath.EndsWith(".tga"))
                using (Bitmap bmp = GetImage(0)) bmp.SaveTGA(outPath);
            else if (outPath.EndsWith(".tiff") || outPath.EndsWith(".tif"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Tiff);
            else if (outPath.EndsWith(".bmp"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Bmp);
            else if (outPath.EndsWith(".jpg") || outPath.EndsWith(".jpeg"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Jpeg);
            else if (outPath.EndsWith(".gif"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Gif);
            else
                base.Export(outPath);
        }

        internal static ResourceNode TryParse(DataSource source) { return ((TEX0v1*)source.Address)->_header._tag == TEX0v1.Tag ? new TEX0Node() : null; }
    }
}
