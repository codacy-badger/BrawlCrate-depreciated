using BrawlLib.IO;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Compression;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ARCNode : ARCEntryNode
    {
        internal ARCHeader* Header { get { return (ARCHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.ARC; } }
        public override Type[] AllowedChildTypes
        {
            get
            {
                return new Type[] { typeof(ARCEntryNode) };
            }
        }

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsPair { get { return _isPair; } set { _isPair = value; } }
        private bool _isPair;

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsStage { get { return _isStage; } }//set { _isStage = value; } }
        private bool _isStage;

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsCharacter { get { return _isCharacter; } }// set { _isCharacter = value; } }
        private bool _isCharacter;

#if !DEBUG
        [Browsable(false)]
#endif
        public bool IsItemTable { get { return _isItemTable; } }// set { _isCharacter = value; } }
        private bool _isItemTable;

        [Browsable(true)]
        public string SpecialARC {
            get {
                if (IsCharacter)
                {
                    return "Fighter";
                }
                else if (IsStage)
                {
                    return "Stage";
                }
                else if (IsItemTable)
                {
                    return "Item Table";
                }
                else if (Parent != null && Parent is ARCNode)
                {
                    if (((ARCNode)Parent).SpecialARC.EndsWith("SubNode") || ((ARCNode)Parent).SpecialARC.Equals("<None>"))
                        return ((ARCNode)Parent).SpecialARC;
                    return ((ARCNode)Parent).SpecialARC + " SubNode";
                }
                return "<None>";
            }
        }

        [Category("Models")]
        public int NumModels
        {
            get
            {
                if (BrawlLib.Properties.Settings.Default.CompatibilityMode)
                    return -1;
                //Populate();
                if (_children == null)
                    return 0;
                int count = 0;
                foreach (ARCEntryNode b in Children)
                    if (b is BRRESNode)
                        count += ((BRRESNode)b).NumModels;
                    else if (b is ARCNode && ((ARCNode)b).NumModels > -1)
                        count += ((ARCNode)b).NumModels;
                return count;
            }
        }
        [Category("Models"), Description("How many points are stored in the models in this ARC and sent to the GPU every frame. A lower value is better.")]
        public int NumFacepoints
        {
            get
            {
                if (BrawlLib.Properties.Settings.Default.CompatibilityMode)
                    return -1;
                //Populate();
                if (_children == null)
                    return 0;
                int count = 0;
                foreach (ARCEntryNode b in Children)
                    if (b is BRRESNode)
                        count += ((BRRESNode)b).NumFacepoints;
                    else if (b is ARCNode && ((ARCNode)b).NumModels > -1)
                        count += ((ARCNode)b).NumFacepoints;
                return count;
            }
        }
        [Browsable(true)]
        [Category("Models"), Description("How many individual vertices models in this ARC have. A vertex in this case is only a point in space with its associated influence.")]
        public int NumVertices
        {
            get
            {
                if (BrawlLib.Properties.Settings.Default.CompatibilityMode)
                    return -1;
                //Populate();
                if (_children == null)
                    return 0;
                int count = 0;
                foreach (ARCEntryNode b in Children)
                    if (b is BRRESNode)
                        count += ((BRRESNode)b).NumVertices;
                    else if (b is ARCNode && ((ARCNode)b).NumModels > -1)
                        count += ((ARCNode)b).NumVertices;
                return count;
            }
        }
        [Category("Models"), Description("The total number of individual triangle faces models in this ARC have.")]
        public int NumTriangles
        {
            get
            {
                if (BrawlLib.Properties.Settings.Default.CompatibilityMode)
                    return -1;
                //Populate();
                if (_children == null)
                    return 0;
                int count = 0;
                foreach (ARCEntryNode b in Children)
                    if (b is BRRESNode)
                        count += ((BRRESNode)b).NumTriangles;
                    else if (b is ARCNode && ((ARCNode)b).NumModels > -1)
                        count += ((ARCNode)b).NumTriangles;
                return count;
            }
        }
        [Category("Models"), Description("The total number of matrices used in this ARC (bones + weighted influences).")]
        public int NumNodes
        {
            get
            {
                if (BrawlLib.Properties.Settings.Default.CompatibilityMode)
                    return -1;
                //Populate();
                if (_children == null)
                    return 0;
                int count = 0;
                foreach (ARCEntryNode b in Children)
                    if (b is BRRESNode)
                        count += ((BRRESNode)b).NumNodes;
                    else if (b is ARCNode && ((ARCNode)b).NumModels > -1)
                        count += ((ARCNode)b).NumNodes;
                return count;
            }
        }

        public override void OnPopulate()
        {
            ARCFileHeader* entry = Header->First;
            for (int i = 0; i < Header->_numFiles; i++, entry = entry->Next)
            {
                DataSource source = new DataSource(entry->Data, entry->Length);
                if ((entry->Length == 0) || (NodeFactory.FromSource(this, source) == null))
                    new ARCEntryNode().Initialize(this, source);
            }
            FindUnloadedChildren();
        }

        public override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);
            if (_origPath != null)
            {
                string path = Path.Combine(Path.GetDirectoryName(_origPath), Path.GetFileNameWithoutExtension(_origPath));
                _isPair = File.Exists(path + ".pac") && File.Exists(path + ".pcs");
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Header->Name;
            _isStage = false;
            _isCharacter = false;
            _isItemTable = false;
            if (_name.Length >= 3 && AbsoluteIndex == -1)
            {
                if (_name.Substring(0, 3).Equals("STG", StringComparison.OrdinalIgnoreCase))
                {
                    _isStage = true;
                    Console.WriteLine(_name + " Generating MetaData");
                }
                else if (_name.Substring(0, 3).Equals("FIT", StringComparison.OrdinalIgnoreCase))
                {
                    _isCharacter = true;
                }
            }
            if (_name.StartsWith("ItmMelee", StringComparison.OrdinalIgnoreCase))
            {
                _isItemTable = true;
            }

            if (Compression == "LZ77" && Header->_numFiles > 0)
            {
                if (_parent != null)
                {
                    if (_parent is ARCNode)
                    {
                        if (((ARCNode)_parent).IsStage && BrawlLib.Properties.Settings.Default.AutoCompressStages)
                        {
                            // Console.WriteLine(_parent._name);
                            CompressionType type;
                            if (Enum.TryParse("ExtendedLZ77", out type))
                            {
                                _compression = type;
                                SignalPropertyChange();
                            }
                        }
                    }
                }
            }
            FindUnloadedChildren();
            return Header->_numFiles > 0;
        }

        public void ExtractToFolder(string outFolder) { ExtractToFolder(outFolder, ".tex0", ".mdl0"); }
        public void ExtractToFolder(string outFolder, string imageExtension) { ExtractToFolder(outFolder, imageExtension, ".mdl0"); }
        public void ExtractToFolder(string outFolder, string imageExtension, string modelExtension)
        {
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            foreach (ARCEntryNode entry in Children)
                if (entry is ARCNode)
                    ((ARCNode)entry).ExtractToFolder(Path.Combine(outFolder, (entry.Name == null || entry.Name.Contains("<Null>", StringComparison.InvariantCultureIgnoreCase)) ? "Null" : entry.Name), imageExtension, modelExtension);
                else if (entry is BRRESNode)
                    ((BRRESNode)entry).ExportToFolder(Path.Combine(outFolder, (entry.Name == null || entry.Name.Contains("<Null>", StringComparison.InvariantCultureIgnoreCase)) ? "Null" : entry.Name), imageExtension, modelExtension);
        }

        public void ReplaceFromFolder(string inFolder) { ReplaceFromFolder(inFolder, ".tex0"); }
        public void ReplaceFromFolder(string inFolder, string imageExtension)
        {
            DirectoryInfo dir = new DirectoryInfo(inFolder);
            DirectoryInfo[] dirs;
            foreach (ARCEntryNode entry in Children)
            {
                if (entry is ARCNode)
                {
                    dirs = dir.GetDirectories(entry.Name);
                    if (dirs.Length > 0)
                    {
                        ((ARCNode)entry).ReplaceFromFolder(dirs[0].FullName, imageExtension);
                        continue;
                    }
                }
                else if (entry is BRRESNode)
                {
                    ((BRRESNode)entry).ReplaceFromFolder(inFolder, imageExtension);
                    continue;
                }
            }
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            int size = ARCHeader.Size + (Children.Count * 0x20);
            foreach (ResourceNode node in Children)
            {
                if (rebuilding)
                    size += node.CalculateSize(force).Align(0x20);
                else
                    size += (int)node.uncompSize;
            }
            return size;
        }

        public override void OnRebuild(VoidPtr address, int size, bool force)
        {
            ARCHeader* header = (ARCHeader*)address;
            *header = new ARCHeader((ushort)Children.Count, Name);

            ARCFileHeader* entry = header->First;
            foreach (ARCEntryNode node in Children)
            {
                *entry = new ARCFileHeader(node.FileType, node.FileIndex, node._calcSize, node.GroupID, node._redirectIndex);
                node.Rebuild(entry->Data, entry->Length, force);
                entry = entry->Next;
            }

            FindUnloadedChildren();
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".pair", StringComparison.OrdinalIgnoreCase))
                ExportPair(outPath);
            else if (outPath.EndsWith(".mrg", StringComparison.OrdinalIgnoreCase))
                ExportAsMRG(outPath);
            else if (outPath.EndsWith(".pcs", StringComparison.OrdinalIgnoreCase))
                ExportPCS(outPath);
            else if (outPath.EndsWith(".mariopast", StringComparison.OrdinalIgnoreCase))
                ExportMarioPast(outPath);
            else if (outPath.EndsWith(".metalgear", StringComparison.OrdinalIgnoreCase))
                ExportMetalGear(outPath);
            else if (outPath.EndsWith(".village", StringComparison.OrdinalIgnoreCase))
                ExportVillage(outPath);
            else if (outPath.EndsWith(".tengan", StringComparison.OrdinalIgnoreCase))
                ExportTengan(outPath);
            else if (outPath.EndsWith(".pac", StringComparison.OrdinalIgnoreCase) && IsCharacter && BrawlLib.Properties.Settings.Default.AutoDecompressFighterPAC)
                ExportPAC(outPath);
            else
                base.Export(outPath);
        }

        public override void FindUnloadedChildren()
        {
            /*foreach (ResourceNode node in Children)
            {
                if (node.loadedInGame == true)
                {
                    Console.WriteLine("Found resource " + node + " in ARC: " + _name);
                    string nodeName = node._name;
                    Console.WriteLine("Node name is: " + nodeName);
                    foreach (ResourceNode node2 in Children)
                    {
                        if (node2._name == nodeName)
                        {
                            Console.WriteLine("Extra instance of " + nodeName + " deactivated");
                            node2.loadedInGame = false;
                        }
                    }
                }
            }*/
            //for (int i = 0; i < _children.Count; i++)
            foreach(ResourceNode nodeTest in Children)
            {
                nodeTest.loadedInGame = true;
            }
            int i = 0;
            foreach (ResourceNode node in Children)
            {
                ++i;
                if (node.loadedInGame == true)
                {
                    //Console.WriteLine("Found resource " + node + " in ARC: " + _name);
                    string nodeName = node._name;
                    //Console.WriteLine("Node name is: " + nodeName);
                    for (int j = i; j < _children.Count; j++)
                    {
                        ResourceNode node2 = _children[j];
                        if (node2._name == nodeName)
                        {
                            Console.WriteLine("Extra instance of " + nodeName + " deactivated");
                            node2.loadedInGame = false;
                        }
                    }
                }
            }
        }

        public void ExportAsMRG(string path)
        {
            MRGNode node = new MRGNode();
            node._children = Children;
            node._changed = true;
            node.Export(path);
        }

        public void ExportPair(string path)
        {
            if (Path.HasExtension(path))
                path = path.Substring(0, path.LastIndexOf('.'));

            ExportPAC(path + ".pac");
            ExportPCS(path + ".pcs");
        }
        // STGMARIOPAST uses 00/01
        public void ExportMarioPast(string path)
        {
            if (Path.HasExtension(path))
                path = path.Substring(0, path.LastIndexOf('.'));
            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length >= 2 && path[path.Length - 2] == '_')
                aslIndicator = path.ToCharArray()[path.Length - 1];
            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if(aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGMARIOPAST_00_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGMARIOPAST_01_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGMARIOPAST_00.pac");
                ExportPAC(path + "\\STGMARIOPAST_01.pac");
            }
        }
        // STGMETALGEAR uses 00, 01, and 02
        public void ExportMetalGear(string path)
        {
            if (Path.HasExtension(path))
                path = path.Substring(0, path.LastIndexOf('.'));
            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length > 1 && path[path.Length - 2] == '_')
                aslIndicator = path.ToCharArray()[path.Length - 1];
            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if (aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGMETALGEAR_00_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGMETALGEAR_01_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGMETALGEAR_02_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGMETALGEAR_00.pac");
                ExportPAC(path + "\\STGMETALGEAR_01.pac");
                ExportPAC(path + "\\STGMETALGEAR_02.pac");
            }
        }
        // STGVILLAGE uses 00, 01, 02, and 03
        public void ExportVillage(string path)
        {
            if (Path.HasExtension(path))
                path = path.Substring(0, path.LastIndexOf('.'));
            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length >= 2 && path[path.Length - 2] == '_')
                aslIndicator = path.ToCharArray()[path.Length - 1];
            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if (aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGVILLAGE_00_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_01_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_02_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_03_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGVILLAGE_04_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGVILLAGE_00.pac");
                ExportPAC(path + "\\STGVILLAGE_01.pac");
                ExportPAC(path + "\\STGVILLAGE_02.pac");
                ExportPAC(path + "\\STGVILLAGE_03.pac");
                ExportPAC(path + "\\STGVILLAGE_04.pac");
            }
        }
        // STGTENGAN uses 1, 2, 3
        public void ExportTengan(string path)
        {
            if (Path.HasExtension(path))
                path = path.Substring(0, path.LastIndexOf('.'));
            char aslIndicator = '\0';
            // Check if ASL is used
            if (path.Contains("_") && path.Length >= 2 && path[path.Length - 2] == '_')
                aslIndicator = path.ToCharArray()[path.Length - 1];
            // Check to make sure they meant this as ASL and not as a type indicator
            if (path.LastIndexOf('\\') < path.Length - 1 && path.Substring(path.LastIndexOf('\\') + 1).StartsWith("STGTENGAN_", StringComparison.OrdinalIgnoreCase) && (aslIndicator == '1' || aslIndicator == '2' || aslIndicator == '3') && path.LastIndexOf('_') == path.IndexOf('_'))
                if (MessageBox.Show("Would you like to use the detected '" + aslIndicator + "' as the ASL indicator for the three files?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    aslIndicator = '\0';
            path = path.Substring(0, path.LastIndexOf('\\'));
            // Export with or without ASL depending on if the file used ASL or not
            if (aslIndicator != '\0')
            {
                ExportPAC(path + "\\STGTENGAN_1_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGTENGAN_2_" + aslIndicator + ".pac");
                ExportPAC(path + "\\STGTENGAN_3_" + aslIndicator + ".pac");
            }
            else
            {
                ExportPAC(path + "\\STGTENGAN_1.pac");
                ExportPAC(path + "\\STGTENGAN_2.pac");
                ExportPAC(path + "\\STGTENGAN_3.pac");
            }
        }
        public void ExportPAC(string outPath)
        {
            Rebuild();
            ExportUncompressed(outPath);
        }
        public void ExportPCS(string outPath)
        {
            Rebuild();
            if (_compression != CompressionType.None || !BrawlLib.Properties.Settings.Default.AutoCompressFighterPCS)
                base.Export(outPath);
            else
            {
                using (FileStream inStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x8, FileOptions.SequentialScan | FileOptions.DeleteOnClose))
                using (FileStream outStream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.SequentialScan))
                {
                    Compressor.Compact(CompressionType.ExtendedLZ77, WorkingUncompressed.Address, WorkingUncompressed.Length, inStream, this);
                    outStream.SetLength(inStream.Length);
                    using (FileMap map = FileMap.FromStream(inStream))
                    using (FileMap outMap = FileMap.FromStream(outStream))
                        Memory.Move(outMap.Address, map.Address, (uint)map.Length);
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((ARCHeader*)source.Address)->_tag == ARCHeader.Tag ? new ARCNode() : null; }
    }

    public unsafe class ARCEntryGroup : ResourceNode
    {
        internal byte _group;
        [Category("ARC Group")]
        public byte GroupID { get { return _group; } set { _group = value; SignalPropertyChange(); UpdateName(); } }

        public ARCEntryGroup(byte group)
        {
            _group = group;
            UpdateName();
        }

        protected void UpdateName()
        {
            Name = String.Format("[{0}]Group", _group);
        }
    }

    public unsafe class ARCEntryNode : U8EntryNode
    {
        public override ResourceType ResourceType { get { return _resourceType; }  }
        public ResourceType _resourceType = ResourceType.ARCEntry;

        [Browsable(true), TypeConverter(typeof(DropDownListCompression))]
        public override string Compression
        {
            get { return base.Compression; }
            set { base.Compression = value; }
        }

        internal ARCFileType _fileType;
        [Category("ARC Entry")]
        public ARCFileType FileType { get { return _fileType; } set { _fileType = value; SignalPropertyChange(); UpdateName(); } }

        internal short _fileIndex;
        [Category("ARC Entry")]
        public short FileIndex { get { return _fileIndex; } set { _fileIndex = value; SignalPropertyChange(); UpdateName(); } }
        
        internal byte _group;
        [Category("ARC Entry")]
        public byte GroupID { get { return _group; } set { _group = value; SignalPropertyChange(); UpdateName(); } }

        [Category("ARC Entry"), Browsable(true)]
        public int AbsoluteIndex { get { return base.Index; } }

        internal short _redirectIndex = -1;

        [Category("ARC Entry")]
        public short RedirectIndex
        {
            get { return _redirectIndex; }
            set
            {
                if (value == Index || value == _redirectIndex)
                    return;

                if ((_redirectIndex = (short)((int)value).Clamp(-1, Parent.Children.Count - 1)) < 0)
                {
                    _resourceType = ResourceType.ARCEntry;
                    Name = GetName();
                }
                else
                {
                    _resourceType = ResourceType.Redirect;
                    Name = "Redirect → " + _redirectIndex;
                }
            } 
        }

        protected virtual string GetName()
        {
            return String.Format("{0} [{1}]", fullTypeName(), _fileIndex);
        }
        
        // Makes everything use spaces
        protected virtual string fullTypeName()
        {
            switch(String.Format("{0}", _fileType)) {
                case "None":
                    return "None";
                case "MiscData":
                    return "Misc Data";
                case "ModelData":
                    return "Model Data";
                case "TextureData":
                    return "Texture Data";
                case "AnimationData":
                    return "Animation Data";
                case "SceneData":
                    return "Scene Data";
                case "Type6":
                    return "Type 6";
                case "GroupedArchive":
                    return "Grouped Archive";
                case "EffectData":
                    return "Effect Data";
                default:
                    return "Error Parsing Filetype";
            }
        }

        protected virtual string GetName(string fileType)
        {
            string s = string.Format("{0} [{1}]", fileType, _fileIndex);
            if (_group != 0)
                s += "[Group " + _group + "]";
            return s;
        }

        protected void UpdateName()
        {
            if (!(this is ARCNode))
                Name = GetName();
        }

        public override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);

            if (parent != null && (parent is MRGNode || RootNode is U8Node))
            {
                _fileType = 0;
                _fileIndex = (short)Parent._children.IndexOf(this);
                _group = 0;
                _redirectIndex = 0;

                if (_name == null)
                    _name = GetName();
            }
            else if (parent != null && !(parent is FileScanNode))
            {
                ARCFileHeader* header = (ARCFileHeader*)(origSource.Address - 0x20);
                _fileType = header->FileType;
                _fileIndex = header->_index;
                _group = header->_groupIndex;
                _redirectIndex = header->_redirectIndex;

                if (_name == null)
                    if (_redirectIndex != -1)
                    {
                        _resourceType = ResourceType.Redirect;
                        _name = "Redirect → " + _redirectIndex;
                    }
                    else
                        _name = GetName();
            }
            else if (_name == null)
                _name = Path.GetFileName(_origPath);
        }

        //public override unsafe void Export(string outPath)
        //{
        //    ExportUncompressed(outPath);
        //}
    }
}
