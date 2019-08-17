﻿using BrawlLib.IO;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ModuleSectionNode : ModuleDataNode
    {
        private ObjectParser _parser;

        [Browsable(false)]
        [DisplayName("Uncompressed Size (Bytes)")]
        public override uint uncompSize => _dataSize;

        internal VoidPtr Header => WorkingUncompressed.Address;
        public override ResourceType ResourceType => ResourceType.RELSection;

        [Browsable(false)] public override uint ASMOffset => (uint) _dataOffset;

        public bool _isCodeSection = false;
        public bool _isBSSSection = false;
        public int _dataOffset = 0;
        public uint _endBufferSize = 0x0;
        public uint _dataSize;
        public int _dataAlign;

        public string DataAlign => "0x" + _dataAlign.ToString("X");

        public string EndBufferSize
        {
            get => "0x" + _endBufferSize.ToString("X");
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                if (Convert.ToByte(field0, fromBase) % 4 != 0 &&
                    MessageBox.Show(
                        "Buffers should generally be multiples of 0x4, are you sure you want to set this? (It may make the module unreadable!)",
                        "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                _endBufferSize = Convert.ToByte(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("REL Section")] public bool HasCommands => _manager._commands.Count > 0;

        [Category("REL Section")]
        [Browsable(true)]
        public override bool HasCode => _isCodeSection;

        [Category("REL Section")] public bool IsBSS => _isBSSSection;

        public ModuleSectionNode()
        {
        }

        public ModuleSectionNode(uint size)
        {
            InitBuffer(size);
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                if (_dataSize > 0)
                {
                    _name = string.Format("Section [{0}]", Index);
                }
                else
                {
                    _name = string.Format("null [{0}]", Index);
                }
            }

            if (_dataOffset == 0 && WorkingUncompressed.Length != 0)
            {
                _isBSSSection = true;
                InitBuffer(_dataSize);
            }
            else
            {
                _isBSSSection = false;
                InitBuffer(_dataSize, Header);
                if (Index == 5)
                {
                    _parser = new ObjectParser(this);
                }
            }

            return _parser != null;
        }

        public override void OnPopulate()
        {
            _parser.Parse();
            _parser.Populate();
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            return _dataBuffer.Length + (int) _endBufferSize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Memory.Move(address, _dataBuffer.Address, (uint) length);
            address += _dataBuffer.Length;
            if (_endBufferSize > 0)
            {
                Memory.Fill(address, _endBufferSize, 0x00);
            }
        }

        public override void Dispose()
        {
            if (_dataBuffer != null)
            {
                _dataBuffer.Dispose();
            }

            base.Dispose();
        }

        //public unsafe void ExportInitialized(string outPath)
        //{
        //    using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
        //    {
        //        stream.SetLength(_dataBuffer.Length);
        //        using (FileMap map = FileMap.FromStream(stream))
        //        {
        //            buint* addr = (buint*)map.Address;
        //            foreach (Relocation loc in Relocations)
        //                *addr++ = loc.SectionOffset;
        //        }
        //    }
        //}

        public override unsafe void Export(string outPath)
        {
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(_dataBuffer.Length);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Memory.Move(map.Address, _dataBuffer.Address, (uint) _dataBuffer.Length);
                }
            }
        }
    }

    //public class RELObjectSectionNode : ModuleSectionNode
    //{
    //    ObjectParser _objectParser;

    //    public void ParseObjects()
    //    {
    //        (_objectParser = new ObjectParser(this)).Parse();
    //    }

    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        return _objectParser._objects.Count > 0;
    //    }

    //    public override void OnPopulate()
    //    {
    //        _objectParser.Populate();
    //    }
    //}

    //public unsafe class RELConstantsSectionNode : ModuleSectionNode
    //{
    //    float[] _values;
    //    public float[] Values { get { return _values; } set { _values = value; } }

    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        _values = new float[_dataBuffer.Length / 4];
    //        bfloat* values = (bfloat*)_dataBuffer.Address;
    //        for (int i = 0; i < _values.Length; i++)
    //            _values[i] = values[i];
    //        return false;
    //    }
    //    public override void OnPopulate()
    //    {
    //        _values = new float[_dataBuffer.Length / 4];
    //        bfloat* values = (bfloat*)_dataBuffer.Address;
    //        for (int i = 0; i < _values.Length; i++)
    //            _values[i] = values[i];
    //    }

    //public override int OnCalculateSize(bool force, bool rebuilding = true)
    //{
    //    return _values.Length * 4;
    //}

    //public override void OnRebuild(VoidPtr address, int length, bool force)
    //{
    //    bfloat* values = (bfloat*)address;
    //    for (int i = 0; i < _values.Length; i++)
    //        values[i] = _values[i];
    //}
    //}

    //public class RELStructorSectionNode : ModuleSectionNode
    //{
    //    public bool _destruct;
    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        for (int i = 0; i < _relocations.Count; i++)
    //            if (_relocations[i].RelOffset > 0)
    //                return true;
    //        return false;
    //    }
    //    public override void OnPopulate()
    //    {
    //        for (int i = 0; i < _relocations.Count; i++)
    //            if (_relocations[i].RelOffset > 0)
    //                new RELDeConStructorNode() { _destruct = _destruct, _index = i }.Initialize(this, (VoidPtr)BaseAddress + _relocations[i].RelOffset, 0);
    //    }
    //}
}