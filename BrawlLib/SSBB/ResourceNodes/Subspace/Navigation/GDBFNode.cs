using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GDBFNode : ResourceNode
    {
        internal GDBF* Header { get { return (GDBF*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.GDBF; } }

        private int _doors;
        [Category("GDBF")]
        public int Doors { get { return _doors; } }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new GDBFEntryNode().Initialize(this, source);

            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (_name == null)
                _name = "Factory Doors";
            _doors = Header->_count;

            return Header->_count > 0;
        }
        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            int size = GDBF.Size + (Children.Count * 4);
            foreach (ResourceNode node in Children)
                size += node.CalculateSize(force);
            return size;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GDBF* header = (GDBF*)address;
            *header = new GDBF(Children.Count);
            uint offset = (uint)(0x08 + (Children.Count * 4));
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0) { offset += (uint)(Children[i - 1].CalculateSize(false)); }
                *(buint*)((VoidPtr)address + 0x08 + i * 4) = offset;
                _children[i].Rebuild((VoidPtr)address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((GDBF*)source.Address)->_tag == GDBF.Tag ? new GDBFNode() : null; }
    }

    public unsafe class GDBFEntryNode : ResourceNode
    {
        internal GDBFEntry* Header { get { return (GDBFEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        private string _doorID;
        [Category("Door Info")]
        [DisplayName("Door ID (File)")]
        public string DoorID { get { return _doorID; } set { _doorID = value; SignalPropertyChange(); } }

        public int _doorIndex;
        [Category("Door Info")]
        [DisplayName("Door Index")]
        public int DoorIndex { get { return _doorIndex; } set { _doorIndex = value; Name = String.Format("Door[{0}]", _doorIndex); SignalPropertyChange(); } }

        private float _posunk1;
        [Category("Position Info")]
        [DisplayName("Unknown 1")]
        public float PosUnknown1 { get { return _posunk1; } set { _posunk1 = value; SignalPropertyChange(); } }

        private float _posunk2;
        [Category("Position Info")]
        [DisplayName("Unknown 2")]
        public float PosUnknown2 { get { return _posunk2; } set { _posunk2 = value; SignalPropertyChange(); } }

        private float _posunk3;
        [Category("Position Info")]
        [DisplayName("Unknown 3")]
        public float PosUnknown3 { get { return _posunk3; } set { _posunk3 = value; SignalPropertyChange(); } }

        private float _xpos;
        [Category("Position Info")]
        [DisplayName("X Position")]
        public float XPosition { get { return _xpos; } set { _xpos = value; SignalPropertyChange(); } }

        private float _ypos;
        [Category("Position Info")]
        [DisplayName("Y Position")]
        public float YPosition { get { return _ypos; } set { _ypos = value; SignalPropertyChange(); } }

        private int _unkInt;
        [Category("Door Info")]
        [DisplayName("Unknown")]
        public int UnkInt { get { return _unkInt; } set { _unkInt = value; SignalPropertyChange(); } }

        private int _unk0;
        [Category("Unknown Field")]
        [DisplayName("Unknown 0")]
        public int Unknown0 { get { return _unk0; } set { _unk0 = value; SignalPropertyChange(); } }

        private int _unk1;
        [Category("Unknown Field")]
        [DisplayName("Unknown 1")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }

        private int _unk2;
        [Category("Unknown Field")]
        [DisplayName("Unknown 2")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }

        private int _unk3;
        [Category("Unknown Field")]
        [DisplayName("Unknown 3")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }

        private int _unk4;
        [Category("Model Field")]
        [DisplayName("Unknown 0")]
        public int UnkMdlField0 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }

        private int _unk5;
        [Category("Model Field")]
        [DisplayName("Unknown 1")]
        public int UnkMdlField1 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }

        private int _mdlIndex;
        [Category("Model Field")]
        [DisplayName("Model Index")]
        public int MdlIndex { get { return _mdlIndex; } set { _mdlIndex = value; SignalPropertyChange(); } }

        private int _unk6;
        [Category("Model Field")]
        [DisplayName("Unknown 2")]
        public int UnkMdlField2 { get { return _unk6; } set { _unk6 = value; SignalPropertyChange(); } }

        private string _trigger0;
        [Category("Event info")]
        [DisplayName("Trigger 1")]
        public string Trigger0 { get { return _trigger0; } set { _trigger0 = value; SignalPropertyChange(); } }

        private string _trigger1;
        [Category("Event info")]
        [DisplayName("Trigger 2")]
        public string Trigger1 { get { return _trigger1; } set { _trigger1 = value; SignalPropertyChange(); } }

        private string _trigger2;
        [Category("Event info")]
        [DisplayName("Unlock Trigger?")]
        public string Trigger2 { get { return _trigger2; } set { _trigger2 = value; SignalPropertyChange(); } }

        private int _unk;
        public int Unk { get { return _unk; } set { _unk = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {


            _doorID = Header->DoorID;
            _doorIndex = Header->_doorIndex;
            _posunk1 = Header->_posunk1;
            _posunk2 = Header->_posunk2;
            _posunk3 = Header->_posunk3;
            _xpos = Header->_xpos;
            _ypos = Header->_ypos;
            _unkInt = Header->_unkInt;
            _unk = Header->_unk0;
            _unk0 = Header->_unk0;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            _unk5 = Header->_unk5;
            _unk6 = Header->_unk6;
            _mdlIndex = Header->_mdlIndex;
            _trigger0 = Header->Trigger0;
            _trigger1 = Header->Trigger1;
            _trigger2 = Header->Trigger2;
            if (_name == null)
                _name = String.Format("Door[{0}]", _doorIndex);

            return false;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GDBFEntry* header = (GDBFEntry*)address;
            *header = new GDBFEntry();

            header->DoorID = _doorID;
            header->_doorIndex = (byte)_doorIndex;
            header->_posunk1 = _posunk1;
            header->_posunk2 = _posunk2;
            header->_posunk3 = _posunk3;
            header->_xpos = _xpos;
            header->_ypos = _ypos;
            header->_unkInt = _unkInt;
            header->_unk7 = (byte)_unk;
            header->_unk0 = (byte)_unk0;
            header->_unk1 = (byte)_unk1;
            header->_unk2 = (byte)_unk2;
            header->_unk3 = (byte)_unk3;
            header->_unk4 = (byte)_unk4;
            header->_unk5 = (byte)_unk5;
            header->_unk6 = (byte)_unk6;
            header->_unk8 = header->_unk9 = header->_unk10 = 0;
            header->_nulls = 0xFFFFFFFF;
            header->_mdlIndex = (byte)_mdlIndex;
            header->Trigger0 = _trigger0;
            header->Trigger1 = _trigger1;
            header->Trigger2 = _trigger2;
            header->_pad0 = 1.0f;
            header->_pad1 = header->Pad2 = 0;

            header->Pad4 = 1.0f;
            
        }
        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            return GDBFEntry.Size;
        }
    }
}
